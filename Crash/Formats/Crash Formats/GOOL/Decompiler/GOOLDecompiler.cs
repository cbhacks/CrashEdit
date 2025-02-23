using CrashEdit.Crash.GOOLIns;
using System;
using System.Windows.Forms;

namespace CrashEdit.Crash
{
    public class GOOLDecompiler(GOOLEntry gool)
    {
        private GOOLEntry gool = gool;
        private List<GOOLDecompFunction> funcs = new();
        private List<int> labels = new();
        private List<GOOLDecompBlock> blocks = new();

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
            }
        }

        private GOOLDecompFunction AddFunc(string name, int offset, bool trans = false)
        {
            GOOLDecompFunction func = new(name) { Offset = offset, Trans = trans };
            func.start = AddBlockNoInstructions("entry_" + name);
            func.start.Type = GoolBranchType.None;
            funcs.Add(func);
            return func;
        }

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

        private GOOLDecompFunction? FindFuncWithBlock(string name)
        {
            foreach (var func in funcs)
            {
                if (func.BlockList.Any((block) => block.name == name))
                    return func;
            }
            return null;
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

                    head.next.Remove(follow);

                    var noelse = head.next[0];
                    GOOLDecompBlock tail = follow.prev.Find(a => a != head && noelse.Dominates(a));
                    var newif = new GOOLDecompBlockContainer("if_ne_" + noelse.name, noelse, tail!, follow);
                    var ifblock = new GOOLDecompBlockIf("if_" + head.name, head, follow, newif);
                    blocks.Add(ifblock);
                    blocks.Add(newif);
                    ReplaceBlock(ifblock, head);
                    newif.GenerateCFG();
                    newif.GenerateDominationTree();
                }
                else
                {
                    // if, else

                    GOOLDecompBlock tailbranch = follow.prev.Find(a => bbranch.Dominates(a));
                    GOOLDecompBlock tailfallthru = follow.prev.Find(a => bfallthru.Dominates(a));
                    var newbranch = new GOOLDecompBlockContainer("if_b_" + bbranch.name, bbranch, tailbranch!, follow);
                    var newelse = new GOOLDecompBlockContainer("if_e_" + bfallthru.name, bfallthru, tailfallthru!, follow);
                    var ifblock = new GOOLDecompBlockIf("if_" + head.name, head, follow, newbranch, newelse);
                    blocks.Add(ifblock);
                    blocks.Add(newbranch);
                    blocks.Add(newelse);
                    ReplaceBlock(ifblock, head);
                    newbranch.GenerateCFG();
                    newbranch.GenerateDominationTree();
                    newelse.GenerateCFG();
                    newelse.GenerateDominationTree();
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

            funcs.Clear();
            labels.Clear();
            blocks.Clear();

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

            for (int i = 0; i < gool.Instructions.Count; ++i)
            {
                var ins = gool.Instructions[i];
                if ((ins.GetName() == "BRA" && ins.Args['I'].Value != 0) || ins.GetName() == "BNEZ" || ins.GetName() == "BEQZ")
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
                    int ofs = ins.Args['I'].Value;
                    TryAddLabel(ofs);
                    if (funcs.Find(x => x.Offset == ofs) == null)
                    {
                        AddFunc($"func_{ofs}", ofs);
                    }
                }
                else if (ins.GetName() == "RET")
                {
                    TryAddLabel(i + 1);
                }
            }

            labels.Sort();
            TryAddLabel(gool.Instructions.Count);

            for (int i = 0; i < labels.Count - 1; ++i)
            {
                AddBlock($"B{i}_{labels[i]}_{labels[i + 1]}", labels[i], labels[i + 1]);
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
                }

                foreach (var next in block.next)
                {
                    next.prev.Add(block);
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
                        block.next[0].prev.Remove(block);
                        block.next.Clear();
                        block.Instructions.RemoveLast();
                        block.Instructions.RemoveLast();
                        block.Instructions.RemoveLast();
                        var trans_func = AddFunc(func.Name, block.end, true);
                        SetFuncFirstBlock(trans_func, GetBlockFromInsIndex(trans_func.Offset));
                        func.Trans = false;
                        func.Name = func.Name.Replace("trans", "enter");
                        func.start.name = func.start.name.Replace("trans", "enter");
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


            int l = 0;
            foreach (var func in funcs)
            {
                func.GenerateDominationTree();

                // check for loops
                // ---------------

                //find loops in function
                for (int i = 1; i < func.BlockList.Count; ++i)
                {
                    var block = func.BlockList[i];
                    foreach (var next in block.next)
                    {
                        // this successor dominates its predecessor, so it might be a back edge?
                        if (next.Dominates(block))
                        {
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
                    var do_while = new GOOLDecompBlockDoWhile("dowhile_" + l++ + "_" + loop.BlockList[0].begin, loop, loop.BlockList[^1]);
                    blocks.Add(do_while);
                    do_while.StructureBreakContinue();
                    do_while.GenerateCFG();
                    do_while.GenerateDominationTree();
                }

                // regenerate CFG since we changed control flow
                func.GenerateCFG();
                func.GenerateDominationTree();
                StructureIfElse(func);
            }

            foreach (var func in funcs)
            {
                string fout = "(defgfun " + func.Name + " ()\n";
                int indent = 2;
                func.start.PrintLispOut(indent, ref fout);
                fout += "  )\n\n";
                Console.Write(fout);
            }

            // DebugPrint();
        }

        private void DebugPrint()
        {
            string debug = "digraph CFG {\n";
            debug += "  node [style=filled shape=ellipse]\n";
            debug += "\n";
            foreach (var func in funcs)
            {
                debug += $"  {func.Name} [fillcolor=pink shape=box fontsize = \"32pt\"]\n";
                debug += $"  {func.Name} -> {func.start.name} [color=purple]\n";
                debug += func.start.PrintRecursiveAsRoot();
                foreach (var block in func.BlockList)
                {
                    break;
                    for (int i = 0; i < func.BlockList.Count; ++i)
                    {
                        var targetblock = func.BlockList[i];
                        // every node dominates and postdominates itself
                        if (block == targetblock) continue;
                        if (targetblock.Dominates(block))
                        {
                            debug += $"  {targetblock.name} -> {block.name} [color=cyan]\n";
                        }
                        if (targetblock.PostDominates(block))
                        {
                            debug += $"  {targetblock.name} -> {block.name} [color=blue]\n";
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
                        break;
                        for (int i = 0; i < bl.BlockList.Count; ++i)
                        {
                            var targetblock = bl.BlockList[i];
                            // every node dominates and postdominates itself
                            if (lblock == targetblock) continue;
                            if (targetblock.Dominates(lblock))
                            {
                                debug += $"  {targetblock.name} -> {lblock.name} [color=cyan]\n";
                            }
                            if (targetblock.PostDominates(lblock))
                            {
                                debug += $"  {targetblock.name} -> {lblock.name} [color=blue]\n";
                            }
                        }
                    }
                }
                if (block is GOOLDecompBlockDoWhile dw)
                {
                    debug += $"  subgraph cluster_{dw.name} {{\n";
                    debug += $"    label = \"{dw.name}\\nheader: {dw.Header.name}\\ntail: {dw.Tail.name}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = lightyellow;\n";
                    debug += dw.Header.PrintRecursiveAsRoot();
                    debug += $"  }}\n";
                }
                else if (block is GOOLDecompBlockIf bi)
                {
                    debug += $"  subgraph cluster_{bi.name} {{\n";
                    debug += $"    label = \"{bi.name}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = lightyellow;\n";
                    debug += bi.Header.PrintRecursiveAsRoot();
                    debug += $"  }}\n";
                }
                else if (block is GOOLDecompBlockContainer bc)
                {
                    debug += $"  subgraph cluster_{bc.name} {{\n";
                    debug += $"    label = \"{bc.name}\";\n";
                    debug += $"    style = filled;\n";
                    debug += $"    fontsize = \"25pt\";\n";
                    debug += $"    fillcolor = lightyellow;\n";
                    debug += bc.Header.PrintRecursiveAsRoot();
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
                //    debug += istr + $"    label = \"loop_{li}\\nheader: {loop.Header.name}\\ntail: {loop.Tail.name}\";\n";
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
                //            debug += istr + $"    {block.name} [fillcolor=cyan]\n";
                //        else if (block == loop.Header && block == loop.Tail)
                //            debug += istr + $"    {block.name} [fillcolor=yellow]\n";
                //        else if (block == loop.Tail)
                //            debug += istr + $"    {block.name} [fillcolor=pink]\n";
                //        else if (block == loop.Header)
                //            debug += istr + $"    {block.name} [fillcolor=lightgreen]\n";
                //        else
                //            debug += istr + $"    {block.name}\n";
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
