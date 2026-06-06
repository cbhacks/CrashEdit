using CrashEdit.Crash.GOOLIns;
using System.Windows.Documents;
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
            GOOLDecompBlock block = new(name, begin, end);
            block.Instructions.AddRange(gool.Instructions.Skip(block.OfsBegin).Take(block.OfsEnd - block.OfsBegin));
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
            GOOLDecompBlock block = new(name, -1, -1);
            blocks.Add(block);
            return block;
        }

        // Add a function to the decompiler, if one does not already exist at that offset.
        // Automatically creates an entry point block that has no statements.
        private GOOLDecompFunction? AddFuncIfNoExistAt(string name, int offset, bool trans = false)
        {
            if (funcs.Any(f => f.Offset == offset)) return null;
            GOOLDecompFunction func = new(name) { Offset = offset, Trans = trans };
            func.start = AddBlockNoInstructions("entry_" + name);
            func.start.Type = GoolBranchType.None;
            funcs.Add(func);
            return func;
        }

        // Set a block as the function's entry point. Block will ONLY connect to the function entry point, and entry point will ONLY connect to block.
        private void SetFuncFirstBlock(GOOLDecompFunction func, GOOLDecompBlock block)
        {
            func.start.Next.Clear();
            func.start.Next.Add(block);
            block.Prev.Clear();
            block.Prev.Add(func.start);
        }

        private GOOLDecompBlock GetBlockFromInsIndex(int index)
        {
            return blocks.Find(b => b.OfsBegin == index)!;
        }

        private GOOLDecompFunction? FindFuncWithBlock(GOOLDecompBlock block)
        {
            return funcs.Find((func) => func.BlockList.Contains(block));
        }

        private void StructureIfElse(IGOOLDecompBlockIterator lister)
        {
            HashSet<GOOLDecompBlock> unresolved = [];
            var as_po = lister.AsPostOrderList();
            SortedDictionary<GOOLDecompBlock, GOOLDecompBlock> if_follow_map = new(Comparer<GOOLDecompBlock>.Create((a, b) => a.PostOrderID - b.PostOrderID));

            foreach (var block in as_po)
            {
                if (block is IGOOLDecompBlockIterator bl)
                {
                    StructureIfElse(bl);
                }
                else if (block.Type == GoolBranchType.If && block.Next.Count == 2 && (lister is not GOOLDecompBlockDoWhile dw || block != dw.Tail))
                {
                    var follow = as_po.Find(n => block.ImmediateDominates(n) && n.Prev.Count >= 2);
                    if (follow != null)
                    {
                        if (unresolved.Any(x => x.Next.Count <= 1 || x.Next[1].ImmDom != x))
                        {
                            Console.WriteLine($"short-circuiting detected in if: {block.Name} -> {follow.Name}");
                            unresolved.RemoveWhere(x => x != follow && follow.PostDominates(x));
                            continue;
                        }
                        if_follow_map.Add(block, follow);
                        foreach (var x in unresolved)
                        {
                            if (x != follow && follow.PostDominates(x))
                            {
                                if_follow_map.Add(x, follow);
                            }
                        }
                        unresolved.RemoveWhere(if_follow_map.ContainsKey);
                    }
                    else
                    {
                        unresolved.Add(block);
                    }
                }
            }

            if (unresolved.Count > 0)
            {
                Console.WriteLine("failed to structure ifs: not all ifs were resolved!");
            }

            // contains a map of header nodes --> formal if blocks. used to patch other nodes that were If header nodes so that this function isn't called recursively.
            Dictionary<GOOLDecompBlock, GOOLDecompBlock> processed_ifs = new();
            foreach (var kvp in if_follow_map)
            {
                var if_node = kvp.Key;
                var follow_node = kvp.Value;
                if (processed_ifs.TryGetValue(follow_node, out GOOLDecompBlock? value))
                {
                    follow_node = value;
                }

                // we can do this because we use a consistent format when setting up successors
                var bbranch = if_node.Next[0];
                var bfallthru = if_node.Next[1];

                GOOLDecompBlockIf ifblock;
                if (if_node.Next.Contains(follow_node))
                {
                    // if, no else
                    ifblock = new GOOLDecompBlockIf("if_ne_" + if_node.Name, if_node, follow_node, bfallthru, null);
                }
                else
                {
                    // if, else
                    ifblock = new GOOLDecompBlockIf("if_e_" + if_node.Name, if_node, follow_node, bfallthru, bbranch);
                }
                blocks.Add(ifblock);
                ifblock.GenerateCFG();
                ifblock.GenerateDominationTree();
                processed_ifs.Add(if_node, ifblock);

                lister.GenerateCFG();
                lister.GenerateDominationTree();
            }

            if (lister is GOOLDecompBlockSubGraph g)
            {
                g.PatchStructuredIf(processed_ifs);
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
                    AddFuncIfNoExistAt($"state_{i}_trans", t, true);
                }
                if (c != 0x3FFF)
                {
                    AddFuncIfNoExistAt($"state_{i}_code", c);
                }
                if (e != 0x3FFF)
                {
                    AddFuncIfNoExistAt($"state_{i}_event", e);
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
                        AddFuncIfNoExistAt($"func_{ofs}", ofs);
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

            for (int i = blocks.Count - 1; i >= 0; --i)
            {
                var block = blocks[i];
                if (block.Instructions.Count == 0) continue;
                var ins = block.Instructions.Last();
                int o = gool.Instructions.IndexOf(ins);
                if (ins.GetName() == "BRA" && ins.Args['I'].Value != 0)
                {
                    int branch = ins.Args['I'].Value;
                    block.Next.Add(GetBlockFromInsIndex(o + 1 + branch));
                    block.Type = GoolBranchType.Goto;
                    // dead code detection: assume unconditional backwards-facing branches have code in front of them
                    // we still consider the branch unconditional!
                    if (branch < 0)
                    {
                        block.Next.Add(GetBlockFromInsIndex(o + 1));
                    }
                }
                else if (ins.GetName() == "BNEZ" || ins.GetName() == "BEQZ")
                {
                    block.Next.Add(GetBlockFromInsIndex(o + 1 + ins.Args['I'].Value));
                    block.Next.Add(GetBlockFromInsIndex(o + 1));
                    block.Type = GoolBranchType.If;
                }
                else if ((ins.Type == typeof(Rjev) || ins.Type == typeof(Acev)) && ins.Args['T'].Value == 0)
                {
                    int branch = ins.Args['I'].Value;
                    block.Next.Add(GetBlockFromInsIndex(o + 1 + branch));
                    if (ins.Args['C'].Value != 0)
                    {
                        block.Next.Add(GetBlockFromInsIndex(o + 1));
                        block.Type = GoolBranchType.If;
                    }
                    else
                    {
                        block.Type = GoolBranchType.Goto;
                        // dead code detection: assume unconditional backwards-facing branches have code in front of them
                        // we still consider the branch unconditional!
                        if (branch < 0)
                        {
                            block.Next.Add(GetBlockFromInsIndex(o + 1));
                        }
                    }
                }
                else if (ins.GetName() == "RET")
                {
                    block.Type = GoolBranchType.Return;
                }
                else
                {
                    block.Next.Add(GetBlockFromInsIndex(o + 1));
                    block.Type = GoolBranchType.None;
                    if (ins.GetName() == "BRA" && ins.Args['I'].Value == 0)
                    {
                        int stackpop = ins.Args['V'].Value;
                        while (stackpop > 1)
                        {
                            var extra_block = AddBlock($"B{block_id++}", block.OfsBegin, block.OfsEnd);
                            extra_block.Type = GoolBranchType.None;
                            extra_block.StackPop = 1;
                            extra_block.Next.Add(block.Next[0]);
                            block.Next[0] = extra_block;
                            stackpop--;
                        }
                        block.StackPop = stackpop;
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
                    if (block.Instructions.Count >= 3 && block.Type == GoolBranchType.Goto && block.Next.Count == 1 &&
                        block.Instructions[^2].GetName() == "SETF" &&
                        block.Instructions[^3].GetName() == "ADD" &&
                        block.Instructions[^2].Arguments == "tpc,[sp]" &&
                       // so lazy!!
                       (block.Instructions[^3].Arguments == "pc,(8)" || block.Instructions[^3].Arguments == "pc,8"))
                    {
                        block.Next.Clear();
                        block.Instructions.RemoveLast();
                        block.Instructions.RemoveLast();
                        block.Instructions.RemoveLast();
                        var trans_func = AddFuncIfNoExistAt(func.Name, block.OfsEnd, true)!;
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

            // split existing blocks based on statements that signify beginning and end of variable scopes
            for (int i = blocks.Count - 1; i >= 0; --i)
            {
                var block = blocks[i];
                if (block.StackPop != -1 && block.Type == GoolBranchType.None)
                {
                    if (block.Statements.Count > 0)
                    {
                        // last statement is a single BRA pop instruction
                        if (block.OfsEnd == block.OfsBegin + 1)
                        {
                            // avoid creating zero-sized blocks
                            block.Type = GoolBranchType.StackPop;
                        }
                        else
                        {
                            var func = FindFuncWithBlock(block)!;
                            var new_block = new GOOLDecompBlock($"B{block_id++}_branch_pop{block.StackPop}", block.OfsEnd - 1, block.OfsEnd);
                            new_block.Instructions.Add(block.Instructions.Last());
                            new_block.Statements.Add(block.Statements.Last());
                            new_block.Type = GoolBranchType.StackPop;
                            new_block.StackPop = block.StackPop;
                            new_block.Next.AddRange(block.Next);
                            blocks.Add(new_block);
                            block.Next.Clear();
                            block.Next.Add(new_block);
                            block.OfsEnd--;
                            block.Instructions.RemoveLast();
                            block.Statements.RemoveLast();
                            block.StackPop = -1;
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
                        if (stmt.Type == GoolStatementType.LetEnd || stmt.Type == GoolStatementType.StackStarve)
                        {
                            var func = FindFuncWithBlock(block)!;
                            var instruction_count = stmt.GetRealInstructionCount();
                            var post_statements = block.Statements.Skip(j + 1).Take(block.Statements.Count - (j + 1));
                            var post_instruction_count = post_statements.Sum(x => x.GetRealInstructionCount());
                            block.OfsEnd -= post_instruction_count + instruction_count;
                            // make two blocks: one for the statement, one for after the statement.
                            var stmt_block = new GOOLDecompBlock($"{block.Name}_B{block_id++}_stmt_pop", block.OfsEnd, block.OfsEnd + instruction_count);
                            stmt_block.Statements.Add(stmt);
                            blocks.Add(stmt_block);
                            var next_block = stmt_block;
                            if (post_instruction_count > 0)
                            {
                                var post_block = new GOOLDecompBlock($"{block.Name}_B{block_id++}_post_pop", block.OfsEnd + instruction_count, block.OfsEnd + instruction_count + post_instruction_count);
                                post_block.Statements.AddRange(post_statements);
                                stmt_block.Next.Add(post_block);
                                blocks.Add(post_block);
                                next_block = post_block;
                            }
                            next_block.Next.AddRange(block.Next);
                            next_block.Type = block.Type;
                            block.Statements.RemoveRange(j, block.Statements.Count - j);
                            block.Next.Clear();
                            block.Next.Add(stmt_block);
                            block.Type = GoolBranchType.None;
                            if (block.Statements.Count == 0)
                            {
                                // block became empty... delete.
                                foreach (var x in block.Prev)
                                {
                                    for (int p = 0; p < x.Next.Count; ++p)
                                    {
                                        if (x.Next[p] == block) x.Next[p] = stmt_block;
                                    }
                                }
                                block.Prev.Clear();
                                blocks.Remove(block);
                            }
                            else
                            {
                                block.Next.Add(stmt_block);
                            }
                            func.GenerateCFG();
                            continue;
                        }
                        else if (stmt.Type == GoolStatementType.StackPush)
                        {
                            var func = FindFuncWithBlock(block)!;
                            var instruction_count = stmt.GetRealInstructionCount();
                            var post_statements = block.Statements.Skip(j + 1).Take(block.Statements.Count - (j + 1));
                            var post_instruction_count = post_statements.Sum(x => x.GetRealInstructionCount());
                            block.OfsEnd -= post_instruction_count + instruction_count;
                            // make two blocks: one for after the statement, one for the statement.
                            var stmt_block = new GOOLDecompBlock($"{block.Name}_B{block_id++}_stmt_push", block.OfsEnd, block.OfsEnd + instruction_count);
                            stmt_block.Statements.Add(stmt);
                            blocks.Add(stmt_block);
                            var next_block = stmt_block;
                            if (post_instruction_count > 0)
                            {
                                // this can even be a branch in the case of (let ((var (if t 0 1)) ..) ..)
                                var post_block = new GOOLDecompBlock($"{block.Name}_B{block_id++}_post_push", block.OfsEnd + instruction_count, block.OfsEnd + instruction_count + post_instruction_count);
                                post_block.Statements.AddRange(post_statements);
                                stmt_block.Next.Add(post_block);
                                blocks.Add(post_block);
                                next_block = post_block;
                            }
                            next_block.Next.AddRange(block.Next);
                            next_block.Type = block.Type;
                            block.Type = GoolBranchType.None;
                            block.Statements.RemoveRange(j, block.Statements.Count - j);
                            block.Next.Clear();
                            if (block.Statements.Count == 0)
                            {
                                // block became empty... delete.
                                foreach (var x in block.Prev)
                                {
                                    for (int p = 0; p < x.Next.Count; ++p)
                                    {
                                        if (x.Next[p] == block) x.Next[p] = stmt_block;
                                    }
                                }
                                block.Prev.Clear();
                                blocks.Remove(block);
                            }
                            else
                            {
                                block.Next.Add(stmt_block);
                            }
                            func.GenerateCFG();
                            continue;
                        }
                    }
                }
            }

            int l = 0;
            foreach (var func in funcs)
            {
                func.GenerateDominationTree();

                // step 1: recursively structure lets
                // ---------------
                func.StructureLets();

                // step 2: recursively structure loops
                // ---------------
                func.StructureLoops();

                StructureIfElse(func);
                try
                {
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
                string fout = "(defgfun " + func.Name + " ()\n";
                int indent = 2;
                func.start.PrintLispOut(indent, ref fout, new());
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
            }
            debug += "}\n";

            File.WriteAllText("test-decomp.txt", debug);
            //Console.Write(debug);
            Clipboard.SetText(debug);
            Console.WriteLine("Copied to clipboard!");
        }
    }
}
