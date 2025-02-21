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
            if (begin >= 0 && end >= 0)
                block.instructions.AddRange(gool.Instructions.Skip(block.begin).Take(block.end - block.begin));
            blocks.Add(block);
            return block;
        }

        private GOOLDecompFunction AddFunc(string name, int offset, bool trans = false)
        {
            GOOLDecompFunction func = new(name) { Offset = offset, Trans = trans };
            func.start = AddBlock("entry_" + name, -1, -1);
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

        public static void GenerateDominationTree(List<GOOLDecompBlock> blocklist)
        {
            // recursive call and clear stale values
            foreach (var block in blocklist)
            {
                block.ImmPostDom = null;
                block.ImmDom = null;
                if (block is GOOLDecompBlockDoWhile dw)
                    dw.GenerateDominationTree();
            }

            // generate dominators for each block
            bool changed;
            GOOLDecompDomVector tested = new(blocklist.Count);
            do
            {
                changed = false;
                foreach (var block in blocklist)
                {
                    foreach (var prev in block.prev)
                    {
                        tested.Overwrite(block.Dominators);
                        block.Dominators.Mask(prev.Dominators);
                        block.Dominators.Set(block.DomID);
                        if (!block.Dominators.Equal(tested))
                        {
                            changed = true;
                        }
                    }
                }
            } while (changed);
            // generate post-dominators for each block
            do
            {
                changed = false;
                foreach (var block in blocklist)
                {
                    foreach (var next in block.next)
                    {
                        tested.Overwrite(block.PostDominators);
                        block.PostDominators.Mask(next.PostDominators);
                        block.PostDominators.Set(block.DomID);
                        if (!block.PostDominators.Equal(tested))
                        {
                            changed = true;
                        }
                    }
                }
            } while (changed);
            // grab immediate (post-)dominators
            foreach (var block in blocklist)
            {
                List<GOOLDecompBlock> pdoms = blocklist.Where(b => b.DomID != block.DomID && block.PostDominators[b.DomID]).ToList();
                int pdoms_count = block.PostDominators.Count;
                foreach (var pdom in pdoms)
                {
                    bool immediate = true;
                    foreach (var other in pdoms)
                    {
                        if (pdom.DomID != other.DomID && pdom.PostDominates(other))
                        {
                            immediate = false;
                            break;
                        }
                    }
                    if (immediate)
                    {
                        block.ImmPostDom = pdom;
                        break;
                    }
                }
                List<GOOLDecompBlock> doms = blocklist.Where(b => b.DomID != block.DomID && block.Dominators[b.DomID]).ToList();
                int doms_count = block.Dominators.Count;
                foreach (var dom in doms)
                {
                    bool immediate = true;
                    foreach (var other in doms)
                    {
                        if (dom.DomID != other.DomID && dom.Dominates(other))
                        {
                            immediate = false;
                            break;
                        }
                    }
                    if (immediate)
                    {
                        block.ImmDom = dom;
                        break;
                    }
                }
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
                if (block.instructions.Count == 0) continue;
                var ins = block.instructions.Last();
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
                    if (block.instructions.Count >= 3 && block.Type == GoolBranchType.Goto && block.next.Count == 1 &&
                        block.instructions[^2].GetName() == "SETF" &&
                        block.instructions[^3].GetName() == "ADD" &&
                        block.instructions[^2].Arguments == "tpc,[sp]" &&
                       // so lazy!!
                       (block.instructions[^3].Arguments == "pc,(8)" || block.instructions[^3].Arguments == "pc,8"))
                    {
                        block.next[0].prev.Remove(block);
                        block.next.Clear();
                        block.instructions.RemoveLast();
                        block.instructions.RemoveLast();
                        block.instructions.RemoveLast();
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


            foreach (var func in funcs)
            {
                GenerateDominationTree(func.BlockList);

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

                int l = 0;
                foreach (var loop in func.LoopList)
                {
                    var do_while = new GOOLDecompBlockDoWhile("dowhile_" + l++ + "_" + loop.BlockList[0].begin, loop, loop.BlockList[^1]);
                    blocks.Add(do_while);
                    do_while.StructureBreakContinue();
                    do_while.Header.GenerateCFGAsRoot(do_while.Loop.BlockList);
                }

                // regenerate CFG since we changed control flow
                func.GenerateCFG();
                GenerateDominationTree(func.BlockList);
                func.StructureIfElse();

                int b = 9999;
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
                debug += $"  {func.Name} -> {func.start.name} [color=purple]\n";
                debug += func.start.PrintRecursiveAsRoot();
                foreach (var block in func.BlockList)
                {
                    for (int i = 0; i < func.BlockList.Count; ++i)
                    {
                        var targetblock = func.BlockList[i];
                        // every node dominates and postdominates itself
                        if (block == targetblock) continue;
                        if (targetblock.Dominates(block))
                        {
                            //debug += $"  {targetblock.name} -> {block.name} [color=cyan]\n";
                        }
                        if (targetblock.PostDominates(block))
                        {
                            //debug += $"  {targetblock.name} -> {block.name} [color=blue]\n";
                        }
                    }
                }
            }
            foreach (var block in blocks)
            {
                // this only does top level stuff. oh well. don't care.
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
