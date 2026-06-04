using CrashEdit.Crash.GOOLIns;
using System.Windows.Forms;

namespace CrashEdit.Crash
{
    public class GOOLDecompiler(GOOLEntry gool)
    {
        private GOOLEntry gool = gool;
        private List<GOOLDecompFunction> funcs = new();
        private List<int> labels = new();
        public List<GOOLDecompBlock> blocks = new();
        private int block_id = 0;

        // Adds a new 'label' to the decompiler, if one does not already exist. A label is simply a reference to an offset in the code file.
        private void TryAddLabel(int ofs)
        {
            if (!labels.Contains(ofs))
                labels.Add(ofs);
        }

        private GOOLDecompBlock AddBlock(string name, int begin, int end)
        {
            GOOLDecompBlock block = new(name);
            block.begin = begin;
            block.end = end;
            block.Instructions.AddRange(gool.Instructions.Skip(block.begin).Take(block.end - block.begin));
            for (int i = block.Instructions.Count - 1; i >= 0; --i)
            {
                var ins = block.Instructions[i];
                if (ins.Type == typeof(Push) && !ins.IsNullRef('B'))
                {
                    // split into two single push instructions. what a hack! but it makes everything SO much easier...
                    // and there would be an edge case either way. so best handle it now than everywhere else later.
                    // instead of push A, B we do push A + push B, by adding the push B and masking the push A
                    // we mark one of them as fake so that it can be accounted for in later calculations of back edges and such
                    int a = ins.Args['A'].Value;
                    int b = ins.Args['B'].Value;
                    block.Instructions[i] = new GOOLInstruction(a | (GOOLInstruction.NullRef << 12) | (22 << 24), ins.GOOL, ins.Type, true);
                    block.Instructions.Insert(i+1, new GOOLInstruction(b | (GOOLInstruction.NullRef << 12) | (22 << 24), ins.GOOL, ins.Type));
                }
            }
            blocks.Add(block);
            return block;
        }

        private GOOLDecompBlock AddBlockNoInstructions(string name)
        {
            GOOLDecompBlock block = new(name);
            block.begin = -1;
            block.end = -1;
            blocks.Add(block);
            return block;
        }

        private void ReplaceBlock(GOOLDecompBlock dst, GOOLDecompBlock src)
        {
            foreach (var block in blocks)
            {
                if (block == src || block == dst)
                    continue;
                for (int i = 0; i < block.prev.Count; ++i)
                {
                    if (src == block.prev[i]) block.prev[i] = dst;
                }
                for (int i = 0; i < block.next.Count; ++i)
                {
                    if (src == block.next[i]) block.next[i] = dst;
                }
                if (block is GOOLDecompBlockDoWhile dw)
                {
                    if (src == dw.Break) dw.Break = dst;
                    if (src == dw.Continue) dw.Continue = dst;
                    if (src == dw.Header) dw.Loop.Header = dst;
                    if (src == dw.Tail) dw.Loop.Tail = dst;
                    for (int i = 0; i < dw.BlockList.Count; ++i)
                    {
                        if (src == dw.BlockList[i]) dw.BlockList[i] = dst;
                    }
                }
                else if (block is GOOLDecompBlockIf cond)
                {
                    if (src == cond.Header) cond.Header = dst;
                    for (int i = 0; i < cond.Clauses.Length; ++i)
                    {
                        if (src == cond.Clauses[i]) cond.Clauses[i] = dst;
                    }
                }
                else if (block is GOOLDecompBlockContainer container)
                {
                    if (src == container.Header) container.Header = dst;
                    if (src == container.Tail) container.Tail = dst;
                    for (int i = 0; i < container.BlockList.Count; ++i)
                    {
                        if (src == container.BlockList[i]) container.BlockList[i] = dst;
                    }
                }
            }
        }

        // Add a function to the decompiler. Automatically creates an entry point block that has no statements.
        private GOOLDecompFunction AddFunc(string name, int offset, bool trans = false)
        {
            GOOLDecompFunction func = new(name) { Offset = offset, Trans = trans };
            func.start = AddBlockNoInstructions("entry_" + name);
            func.start.Type = GoolBranchType.None;
            funcs.Add(func);
            return func;
        }

        // Set a block as the function's entry point. Block will ONLY connect to the function entry point, and entry point will ONLY connect to block.
        private void SetFuncFirstBlock(GOOLDecompFunction func, GOOLDecompBlock block)
        {
            func.start.next.Clear();
            func.start.next.Add(block);
            block.prev.Clear();
            block.prev.Add(func.start);
        }

        private GOOLDecompBlock GetBlockFromInsIndex(int index)
        {
            return blocks.Find(b => b.begin == index)!;
        }

        private GOOLDecompFunction? FindFuncWithBlock(GOOLDecompBlock block)
        {
            return funcs.Find((func) => func.BlockList.Contains(block));
        }

        public void MakeBlockPrevLists()
        {
            blocks.ForEach(block => block.prev.Clear());
            blocks.ForEach(block => block.next.ForEach(next => next.prev.Add(block)));
        }

        private GOOLDecompLoop? CreateLoopFromEdge(GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock? prebranch = null)
        {
            if (header.Type == GoolBranchType.Goto && header.next.All(a => a.begin > header.begin))
                return null;
            Stack<GOOLDecompBlock> workList = new();
            GOOLDecompLoop loop = new(header, tail);
            loop.BlockList.Add(header);
            if (header != tail)
            {
                loop.BlockList.Add(tail);
                workList.Push(tail);
            }
            while (workList.Count > 0)
            {
                var block = workList.Pop();
                foreach (var prev in block.prev)
                {
                    if (!loop.BlockList.Contains(prev) && prev != prebranch)
                    {
                        loop.BlockList.Add(prev);
                        workList.Push(prev);
                    }
                }
            }
            if (prebranch != null)
            {
                loop.BlockList.Add(prebranch);
                loop.PreBranch = prebranch;
            }
            loop.BlockList.Sort((a, b) => a.begin - b.begin);
            return loop;
        }

        private void StructureIfElse(IGOOLDecompBlockIterator lister)
        {
            HashSet<GOOLDecompBlock> unresolved = [];
            var as_po = lister.AsPostOrderList();
            Dictionary<GOOLDecompBlock, GOOLDecompBlock> follows = [];

            foreach (var block in as_po)
            {
                if (block.Type == GoolBranchType.If && block.next.Count == 2)
                {
                    var follow = as_po.Find(n => block.ImmediateDominates(n) && n.prev.Count >= 2);
                    if (follow != null)
                    {
                        follows.Add(block, follow);
                        foreach (var x in unresolved)
                        {
                            follows.Add(x, follow);
                        }
                        unresolved.Clear();
                    }
                    else
                    {
                        unresolved.Add(block);
                    }
                }
                else if (block is IGOOLDecompBlockIterator bl && block is not GOOLDecompBlockContainer)
                {
                    StructureIfElse(bl);
                }
            }

            var heads = new List<GOOLDecompBlock>(follows.Keys);
            heads.Sort((a, b) => a.PostOrderID - b.PostOrderID);
            foreach (var head in heads)
            {
                var follow = follows[head];
                // we can do this because we use a consistent format when setting up successors
                var bbranch = head.next[0];
                var bfallthru = head.next[1];

                if (head.next.Contains(follow))
                {
                    // if, no else
                    Console.WriteLine($"Making an if no-else block out of {head.Name}");

                    head.next.Remove(follow);

                    var noelse = head.next[0];
                    GOOLDecompBlock tail = follow.prev.Find(a => a != head && noelse.Dominates(a));
                    var newif = new GOOLDecompBlockContainer("if_ne_" + noelse.Name, noelse, tail!, follow);
                    var ifblock = new GOOLDecompBlockIf("if_" + head.Name, head, follow, newif);
                    blocks.Add(ifblock);
                    blocks.Add(newif);
                    ReplaceBlock(ifblock, head);
                    //newif.GenerateCFG();
                    //newif.GenerateDominationTree();
                }
                else
                {
                    // if, else

                    GOOLDecompBlock tailbranch = follow.prev.Find(a => bbranch.Dominates(a));
                    GOOLDecompBlock tailfallthru = follow.prev.Find(a => bfallthru.Dominates(a));
                    var newbranch = new GOOLDecompBlockContainer("if_b_" + bbranch.Name, bbranch, tailbranch!, follow);
                    var newelse = new GOOLDecompBlockContainer("if_e_" + bfallthru.Name, bfallthru, tailfallthru!, follow);
                    var ifblock = new GOOLDecompBlockIf("if_" + head.Name, head, follow, newbranch, newelse);
                    blocks.Add(ifblock);
                    blocks.Add(newbranch);
                    blocks.Add(newelse);
                    ReplaceBlock(ifblock, head);
                    //newbranch.GenerateCFG();
                    //newbranch.GenerateDominationTree();
                    //newelse.GenerateCFG();
                    //newelse.GenerateDominationTree();
                }

                lister.GenerateCFG();
                lister.GenerateDominationTree();

                StructureIfElse(lister);
                break;
            }
        }

        public void Decompile()
        {
            if (gool.Format == 0 || gool.Instructions.Any(ins => ins is MIPSInstruction))
                return;

            // clear decompiler state
            funcs.Clear();
            labels.Clear();
            blocks.Clear();
            block_id = 0;

            // create functions for all state handlers
            for (int i = 0; i < gool.StateDescriptors.Count; ++i)
            {
                var state = gool.StateDescriptors[i];
                int t = state.TransHook & 0x3FFF;
                int c = state.CodeHook & 0x3FFF;
                int e = state.EventHook & 0x3FFF;
                if (t != 0x3FFF)
                {
                    AddFunc($"state_{i}_trans", t, true);
                }
                if (c != 0x3FFF)
                {
                    AddFunc($"state_{i}_code", c);
                }
                if (e != 0x3FFF)
                {
                    AddFunc($"state_{i}_event", e);
                }
            }

            foreach (var func in funcs)
            {
                TryAddLabel(func.Offset);
            }

            // Navigate every GOOL instruction and add a label to ALL referenced offsets.
            for (int i = 0; i < gool.Instructions.Count; ++i)
            {
                var ins = gool.Instructions[i];
                if (ins.GetName() == "BRA" || ins.GetName() == "BNEZ" || ins.GetName() == "BEQZ")
                {
                    TryAddLabel(i + 1 + ins.Args['I'].Value);
                    TryAddLabel(i + 1);
                }
                else if ((ins.Type == typeof(Rjev) || ins.Type == typeof(Acev)) && ins.Args['T'].Value == 0)
                {
                    TryAddLabel(i + 1 + ins.Args['I'].Value);
                    TryAddLabel(i + 1);
                }
                else if (ins.GetName() == "CALL" || ins.GetName() == "SETC")
                {
                    // These instructions reference functions.
                    int ofs = ins.Args['I'].Value;
                    TryAddLabel(ofs);
                    if (funcs.Find(x => x.Offset == ofs) == null)
                    {
                        AddFunc($"func_{ofs}", ofs);
                    }
                }
                else if (ins.GetName() == "RET")
                {
                    // Marks the end of a function, potentially.
                    TryAddLabel(i + 1);
                }
            }

            // Sort all labels. Add a label that marks the end of the code.
            labels.Sort();
            TryAddLabel(gool.Instructions.Count);

            // Create a basic block that encompasses EVERY region between labels. Since there is no padding in GOOL bytecode, this is guaranteed to be valid.
            for (int i = 0; i < labels.Count - 1; ++i)
            {
                AddBlock($"B{block_id++}", labels[i], labels[i + 1]);
            }

            foreach (var block in blocks)
            {
                if (block.Instructions.Count == 0) continue;
                var ins = block.Instructions.Last();
                int i = gool.Instructions.IndexOf(ins);
                if (ins.GetName() == "BRA" && ins.Args['I'].Value != 0)
                {
                    int branch = ins.Args['I'].Value;
                    block.next.Add(GetBlockFromInsIndex(i + 1 + branch));
                    block.Type = GoolBranchType.Goto;
                    // dead code detection: assume unconditional backwards-facing branches have code in front of them
                    // we still consider the branch unconditional!
                    if (branch < 0)
                    {
                        block.next.Add(GetBlockFromInsIndex(i + 1));
                    }
                }
                else if (ins.GetName() == "BNEZ" || ins.GetName() == "BEQZ")
                {
                    block.next.Add(GetBlockFromInsIndex(i + 1 + ins.Args['I'].Value));
                    block.next.Add(GetBlockFromInsIndex(i + 1));
                    block.Type = GoolBranchType.If;
                }
                else if ((ins.Type == typeof(Rjev) || ins.Type == typeof(Acev)) && ins.Args['T'].Value == 0)
                {
                    int branch = ins.Args['I'].Value;
                    block.next.Add(GetBlockFromInsIndex(i + 1 + branch));
                    if (ins.Args['C'].Value != 0)
                    {
                        block.next.Add(GetBlockFromInsIndex(i + 1));
                        block.Type = GoolBranchType.If;
                    }
                    else
                    {
                        block.Type = GoolBranchType.Goto;
                        // dead code detection: assume unconditional backwards-facing branches have code in front of them
                        // we still consider the branch unconditional!
                        if (branch < 0)
                        {
                            block.next.Add(GetBlockFromInsIndex(i + 1));
                        }
                    }
                }
                else if (ins.GetName() == "RET")
                {
                    block.Type = GoolBranchType.Return;
                }
                else
                {
                    block.next.Add(GetBlockFromInsIndex(i + 1));
                    block.Type = GoolBranchType.None;
                    if (ins.GetName() == "BRA" && ins.Args['I'].Value == 0)
                    {
                        block.stackpop = ins.Args['V'].Value;
                    }
                }
            }

            foreach (var func in funcs)
            {
                SetFuncFirstBlock(func, GetBlockFromInsIndex(func.Offset));
            }

            // generate block list and (post-)dominators for each function's blocks and check for this
            // we do this for each function instead of every block in file because O(n^2) scaling and there is no inter-procedural CFG (that we care about)
            foreach (var func in funcs)
            {
                func.GenerateCFG();
            }

            for (int i = funcs.Count - 1; i >= 0; --i)
            {
                var func = funcs[i];
                if (!func.Trans)
                    continue;
                bool regen = false;
                foreach (var block in func.BlockList)
                {
                    if (block.Instructions.Count >= 3 && block.Type == GoolBranchType.Goto && block.next.Count == 1 &&
                        block.Instructions[^2].GetName() == "SETF" &&
                        block.Instructions[^3].GetName() == "ADD" &&
                        block.Instructions[^2].Arguments == "tpc,[sp]" &&
                       // so lazy!!
                       (block.Instructions[^3].Arguments == "pc,(8)" || block.Instructions[^3].Arguments == "pc,8"))
                    {
                        block.next.Clear();
                        block.Instructions.RemoveLast();
                        block.Instructions.RemoveLast();
                        block.Instructions.RemoveLast();
                        var trans_func = AddFunc(func.Name, block.end, true);
                        SetFuncFirstBlock(trans_func, GetBlockFromInsIndex(trans_func.Offset));
                        func.Trans = false;
                        func.Name = func.Name.Replace("trans", "enter");
                        func.start.PatchNameTransToEnter();
                        // regenerate CFG since we changed its structure
                        trans_func.GenerateCFG();
                        regen = true;
                        break;
                    }
                }
                if (regen)
                    func.GenerateCFG();
            }

            // generate statements in each block
            foreach (var block in blocks)
            {
                block.GenerateStatements();
            }

            MakeBlockPrevLists();

            // split existing blocks based on statements that signify beginning and end of variable scopes
            for (int i = blocks.Count - 1; i >= 0; --i)
            {
                var block = blocks[i];
                if (block.stackpop != -1 && block.Type == GoolBranchType.None)
                {
                    if (block.Statements.Count > 0)
                    {
                        // last statement is a single BRA pop instruction
                        if (block.end == block.begin + 1)
                        {
                            // avoid creating zero-sized blocks
                            block.Type = GoolBranchType.StackPop;
                        }
                        else
                        {
                            var func = FindFuncWithBlock(block)!;
                            var new_block = new GOOLDecompBlock($"B{block_id++}_branch_pop{block.stackpop}");
                            new_block.begin = block.end - 1;
                            new_block.end = block.end;
                            new_block.Instructions.Add(block.Instructions.Last());
                            new_block.Statements.Add(block.Statements.Last());
                            new_block.Type = GoolBranchType.StackPop;
                            new_block.stackpop = block.stackpop;
                            new_block.next.AddRange(block.next);
                            blocks.Add(new_block);
                            block.next.Clear();
                            block.next.Add(new_block);
                            block.end--;
                            block.Instructions.RemoveLast();
                            block.Statements.RemoveLast();
                            block.stackpop = -1;
                            block.Type = GoolBranchType.None;
                            func.GenerateCFG();
                        }
                        continue;
                    }
                }
                else
                {
                    for (int j = block.Statements.Count - 1; j >= 0; --j)
                    {
                        var stmt = block.Statements[j];
                        if (stmt.Type == GoolStatementType.LetEnd)
                        {
                            var func = FindFuncWithBlock(block)!;
                            var instruction_count = stmt.GetRealInstructionCount();
                            var post_statements = block.Statements.Skip(j + 1).Take(block.Statements.Count - (j + 1));
                            var post_instruction_count = post_statements.Sum(x => x.GetRealInstructionCount());
                            block.end -= post_instruction_count + instruction_count;
                            // make two blocks: one for after the statement, one for the statement.
                            var stmt_block = new GOOLDecompBlock($"B{block_id++}_stmt_pop_{j}");
                            stmt_block.begin = block.end;
                            stmt_block.end = block.end + instruction_count;
                            stmt_block.Statements.Add(stmt);
                            blocks.Add(stmt_block);
                            var next_block = stmt_block;
                            if (post_instruction_count > 0)
                            {
                                var post_block = new GOOLDecompBlock($"B{block_id++}_post_pop_{j}");
                                post_block.begin = block.end + instruction_count;
                                post_block.end = block.end + instruction_count + post_instruction_count;
                                post_block.Statements.AddRange(post_statements);
                                stmt_block.next.Add(post_block);
                                blocks.Add(post_block);
                                next_block = post_block;
                            }
                            next_block.next.AddRange(block.next);
                            next_block.Type = block.Type;
                            block.Statements.RemoveRange(j, block.Statements.Count - j);
                            block.next.Clear();
                            block.next.Add(stmt_block);
                            block.Type = GoolBranchType.None;
                            func.GenerateCFG();
                        }
                        else if (stmt.Type == GoolStatementType.LetBegin)
                        {
                            var func = FindFuncWithBlock(block)!;
                            var instruction_count = stmt.GetRealInstructionCount();
                            var post_statements = block.Statements.Skip(j + 1).Take(block.Statements.Count - (j + 1));
                            var post_instruction_count = post_statements.Sum(x => x.GetRealInstructionCount());
                            block.end -= post_instruction_count + instruction_count;
                            // make two blocks: one for after the statement, one for the statement.
                            var stmt_block = new GOOLDecompBlock($"B{block_id++}_stmt_push_{j}");
                            stmt_block.begin = block.end;
                            stmt_block.end = block.end + instruction_count;
                            stmt_block.Statements.Add(stmt);
                            blocks.Add(stmt_block);
                            var next_block = stmt_block;
                            if (post_instruction_count > 0)
                            {
                                var post_block = new GOOLDecompBlock($"B{block_id++}_post_push_{j}");
                                post_block.begin = block.end + instruction_count;
                                post_block.end = block.end + instruction_count + post_instruction_count;
                                post_block.Statements.AddRange(post_statements);
                                stmt_block.next.Add(post_block);
                                blocks.Add(post_block);
                                next_block = post_block;
                            }
                            next_block.next.AddRange(block.next);
                            next_block.Type = block.Type;
                            block.Type = GoolBranchType.None;
                            block.Statements.RemoveRange(j, block.Statements.Count - j);
                            block.next.Clear();
                            if (block.Statements.Count == 0)
                            {
                                // block became empty... delete.
                                foreach (var x in block.prev)
                                {
                                    for (int p = 0; p < x.next.Count; ++p)
                                    {
                                        if (x.next[p] == block) x.next[p] = stmt_block;
                                    }
                                }
                                block.prev.Clear();
                                blocks.Remove(block);
                            }
                            else
                            {
                                block.next.Add(stmt_block);
                            }
                            func.GenerateCFG();
                            continue;
                        }
                    }
                }
            }

            MakeBlockPrevLists();


            int l = 0;
            foreach (var func in funcs)
            {
                func.GenerateDominationTree();

                // step 1: recursively structure lets
                // ---------------
                func.StructureLets(this);

                // check for loops
                // ---------------

                //find loops in function
                for (int i = 1; i < func.BlockList.Count; ++i)
                {
                    var block = func.BlockList[i];
                    foreach (var next in block.next)
                    {
                        // block will proceed into a different block that dominates us - i.e. if we went to the start of a loop and this was a back edge!
                        if (next.Dominates(block))
                        {
                            // that means block is the tail (where the loop ends) and the thing it goes to is the head (where the loop begins)
                            var loop = CreateLoopFromEdge(next, block);
                            if (loop == null)
                                continue;
                            foreach (var prev in next.prev)
                            {
                                if (next.ImmDom == prev && prev.Type == GoolBranchType.Goto && prev.begin < next.begin && !loop.BlockList.Contains(prev))
                                {
                                    loop.BlockList.Add(prev);
                                    loop.PreBranch = prev;
                                    break;
                                }
                            }
                            if (loop.PreBranch != null)
                            {
                                // the head of this loop is actually the immediate dominatee of the tail's back edge
                                GOOLDecompBlock? realhead = null;
                                foreach (var head in next.next)
                                {
                                    if (next.ImmediateDominates(head) && head.begin <= next.begin)
                                    {
                                        realhead = head;
                                        break;
                                    }
                                }
                                if (realhead == null)
                                    continue;
                                loop = CreateLoopFromEdge(realhead, next, loop.PreBranch);
                                if (loop == null)
                                    continue;
                            }
                            if (!func.LoopList.Any(l => l.Header == loop.Header && l.Tail == loop.Tail && l.PreBranch == loop.PreBranch))
                            {
                                func.LoopList.Add(loop);
                            }
                        }
                    }
                }

                foreach (var loop in func.LoopList)
                {
                    loop.LoopDepth = 0;
                    foreach (var otherloop in func.LoopList)
                    {
                        if (otherloop == loop) continue;
                        if (loop.BlockList.All(otherloop.BlockList.Contains))
                        {
                            loop.LoopDepth++;
                            if (loop.Parent == null || otherloop.BlockList.Count < loop.Parent.BlockList.Count)
                                loop.Parent = otherloop;
                        }
                    }
                }

                // sort loops by 'depth' (descending, so deepest first) and assign children
                func.LoopList.Sort((a, b) => b.LoopDepth - a.LoopDepth);
                foreach (var loop in func.LoopList)
                {
                    loop.Parent?.Children.Add(loop);
                }

                foreach (var loop in func.LoopList)
                {
                    break;
                    var do_while = new GOOLDecompBlockDoWhile("dowhile_" + l++ + "_" + loop.BlockList[0].begin, loop, loop.BlockList[^1]);
                    blocks.Add(do_while);
                    do_while.StructureBreakContinue();
                    // regenerate CFG since we changed control flow
                    func.GenerateCFG();
                    func.GenerateDominationTree();
                    //StructureIfElse(do_while);
                }

                //func.GenerateCFG();
                //func.GenerateDominationTree();
                try
                {
                    //StructureIfElse(func);
                }
                catch (Exception ex)
                {
                    func.HasError = true;
                    Console.WriteLine($"failed to structure function {func.Name}: {ex.Message}");
                }
            }

            // find variables


            foreach (var func in funcs)
            {
                break;
                if (func.HasError) continue;
                string fout = "(defgfun " + func.Name + " ()\n";
                int indent = 2;
                func.start.PrintLispOut(indent, ref fout);
                fout += "  )\n\n";
                Console.Write(fout);
            }

            DebugPrint();
        }

        private void DebugPrint()
        {
            string debug = "digraph CFG {\n";
            debug += "  node [style=filled shape=ellipse]\n";
            debug += "\n";
            foreach (var func in funcs)
            {
                debug += $"  {func.Name} [fillcolor=pink shape=box fontsize = \"32pt\"]\n";
                debug += $"  {func.Name} -> {func.start.Name} [color=purple]\n";
                debug += func.start.PrintRecursiveAsRoot();
                foreach (var block in func.BlockList)
                {
                    continue;
                    for (int i = 0; i < func.BlockList.Count; ++i)
                    {
                        var targetblock = func.BlockList[i];
                        // every node dominates and postdominates itself
                        if (block == targetblock) continue;
                        if (targetblock.Dominates(block))
                        {
                            debug += $"  {targetblock.Name} -> {block.Name} [color=cyan]\n";
                        }
                        if (targetblock.PostDominates(block))
                        {
                            debug += $"  {targetblock.Name} -> {block.Name} [color=blue]\n";
                        }
                    }
                }
            }
            foreach (var block in blocks)
            {
                if (block is IGOOLDecompBlockIterator bl)
                {
                    foreach (var lblock in bl.BlockList)
                    {
                        continue;
                        for (int i = 0; i < bl.BlockList.Count; ++i)
                        {
                            var targetblock = bl.BlockList[i];
                            // every node dominates and postdominates itself
                            if (lblock == targetblock) continue;
                            if (targetblock.Dominates(lblock))
                            {
                                debug += $"  {targetblock.Name} -> {lblock.Name} [color=cyan]\n";
                            }
                            if (targetblock.PostDominates(lblock))
                            {
                                debug += $"  {targetblock.Name} -> {lblock.Name} [color=blue]\n";
                            }
                        }
                    }
                }
                if (block is GOOLDecompBlockDoWhile dw)
                {
                    debug += $"  subgraph cluster_{dw.Name} {{\n";
                    debug += $"    label = \"{dw.GetFullName()}\\nheader: {dw.Header.Name}\\ntail: {dw.Tail.Name}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = lightyellow;\n";
                    debug += dw.Header.PrintRecursiveAsRoot();
                    debug += $"  }}\n";
                }
                else if (block is GOOLDecompBlockIf bi)
                {
                    debug += $"  subgraph cluster_{bi.Name} {{\n";
                    debug += $"    label = \"{bi.GetFullName()}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = lightyellow;\n";
                    debug += bi.Header.PrintRecursiveAsRoot();
                    debug += $"  }}\n";
                }
                else if (block is GOOLDecompBlockContainer bc)
                {
                    debug += $"  subgraph cluster_{bc.Name} {{\n";
                    debug += $"    label = \"{bc.GetFullName()}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = lightyellow;\n";
                    debug += bc.Header.PrintRecursiveAsRoot();
                    debug += $"  }}\n";
                }
                else if (block is GOOLDecompBlockRegion rg)
                {
                    debug += $"  subgraph cluster_{rg.Name} {{\n";
                    debug += $"    label = \"{rg.GetFullName()}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = cyan3;\n";
                    debug += rg.Entry.PrintRecursiveAsRoot();
                    debug += $"  }}\n";
                }
            }
            int li = 0;
            foreach (var func in funcs)
            {
                //void print_loop(int indent, GOOLDecompLoop loop)
                //{
                //    var istr = new string(' ', indent);
                //    debug += istr + $"  subgraph cluster_loop_{li} {{\n";
                //    debug += istr + $"    label = \"loop_{li}\\nheader: {loop.Header.Name}\\ntail: {loop.Tail.Name}\";\n";
                //    li++;
                //    debug += istr + $"    style = filled;\n";
                //    debug += istr + $"    fontsize = \"25pt\";\n";
                //    debug += istr + $"    fillcolor = lightyellow;\n";
                //    foreach (var ch in loop.Children)
                //    {
                //        print_loop(indent + 2, ch);
                //    }
                //    foreach (var block in loop.BlockList.Where(a => !loop.Children.Any(c => c.BlockList.Contains(a))))
                //    {
                //        if (block == loop.PreBranch)
                //            debug += istr + $"    {block.Name} [fillcolor=cyan]\n";
                //        else if (block == loop.Header && block == loop.Tail)
                //            debug += istr + $"    {block.Name} [fillcolor=yellow]\n";
                //        else if (block == loop.Tail)
                //            debug += istr + $"    {block.Name} [fillcolor=pink]\n";
                //        else if (block == loop.Header)
                //            debug += istr + $"    {block.Name} [fillcolor=lightgreen]\n";
                //        else
                //            debug += istr + $"    {block.Name}\n";
                //    }
                //    debug += istr + $"  }}\n";
                //}
                //foreach (var loop in func.LoopList)
                //{
                //    if (loop.Parent == null) print_loop(0, loop);
                //}
            }
            debug += "}\n";

            File.WriteAllText("test-decomp.txt", debug);
            Console.Write(debug);
            Clipboard.SetText(debug);
            Console.WriteLine("Copied to clipboard!");
        }
    }
}
