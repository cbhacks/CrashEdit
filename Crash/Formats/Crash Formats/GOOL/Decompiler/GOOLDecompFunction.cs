namespace CrashEdit.Crash
{
    public interface IGOOLDecompBlockIterator
    {
        public List<GOOLDecompBlock> BlockList { get; }

        public void GenerateCFG();
        public void GenerateCFGInt(GOOLDecompBlock start)
        {
            // generate block list
            BlockList.Clear();
            int poid = 0;
            start.VisitForward(preVisit: (block) =>
            {
                block.DomID = BlockList.Count;
                BlockList.Add(block);
                block.Prev.Clear();
            }, postVisit: (block) =>
            {
                block.PostOrderID = poid++;
                block.Next.ForEach(next => next.Prev.Add(block));
            });
            BlockList.Sort((a, b) => a.OfsBegin - b.OfsBegin);
            // initialize dominators and postdominators
            BlockList.ForEach((block) =>
            {
                block.Dominators = new(BlockList.Count);
                if (block == start)
                {
                    // this is the entry node
                    block.Dominators.ClearAll();
                    block.Dominators.Set(block.DomID);
                }
                else
                {
                    block.Dominators.SetAll();
                }
                block.PostDominators = new(BlockList.Count);
                if (block.Next.Count == 0)
                {
                    // this is an exit node since it cannot go any further
                    block.PostDominators.ClearAll();
                    block.PostDominators.Set(block.DomID);
                }
                else
                {
                    block.PostDominators.SetAll();
                }
            });
        }

        public void GenerateDominationTree();
        public void GenerateDominationTreeInt()
        {
            // clear stale values
            foreach (var block in BlockList)
            {
                block.ImmPostDom = null;
                block.ImmDom = null;
            }

            // generate dominators for each block
            bool changed;
            GOOLDecompDomVector tested = new(BlockList.Count);
            do
            {
                changed = false;
                foreach (var block in BlockList)
                {
                    foreach (var prev in block.Prev)
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
                foreach (var block in BlockList)
                {
                    foreach (var next in block.Next)
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
            foreach (var block in BlockList)
            {
                List<GOOLDecompBlock> pdoms = BlockList.Where(b => b.DomID != block.DomID && block.PostDominators[b.DomID]).ToList();
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
                List<GOOLDecompBlock> doms = BlockList.Where(b => b.DomID != block.DomID && block.Dominators[b.DomID]).ToList();
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

        public List<GOOLDecompBlock> AsPostOrderList()
        {
            List<GOOLDecompBlock> polist = new(BlockList);
            polist.Sort((a, b) => a.PostOrderID - b.PostOrderID);
            return polist;
        }

        public void StructureLets();
        public void StructureLetsInt(List<GOOLDecompBlock> polist)
        {
            for (int i = 0; i < polist.Count; ++i)
            {
                var block = polist[i];
                if (block.IsStackPop())
                {
                    GOOLDecompBlock let_end = block;
                    GOOLDecompBlock let_begin = null;
                    List<GOOLDecompBlock> let_region = new();
                    List<GOOLDecompBlock> pushes_waiting_for_dom = new();
                    List<GOOLDecompBlock> pending_blocks = new();
                    int stack_depth = block.Type == GoolBranchType.StackPop ? block.StackPop : 1;
                    for (int j = i + 1; j < polist.Count; ++j)
                    {
                        var region_block = polist[j];
                        if (region_block.IsLetBegin())
                        {
                            if (region_block.Dominates(let_end))
                            {
                                stack_depth -= 1;
                            }
                            else
                            {
                                // does not dominate, but comes before pop... perhaps an (if a x y) situation
                                pushes_waiting_for_dom.Add(region_block);
                            }
                        }
                        else if (region_block.IsStackPop())
                        {
                            stack_depth += 1;
                        }
                        else if (region_block.Type == GoolBranchType.StackPop)
                        {
                            stack_depth += region_block.StackPop;
                        }
                        else if (pushes_waiting_for_dom.Count > 0 && pushes_waiting_for_dom.All(region_block.Dominates))
                        {
                            stack_depth -= 1;
                            pushes_waiting_for_dom.Clear();
                            if (stack_depth > 0)
                            {
                                let_region.InsertRange(0, pending_blocks);
                                pending_blocks.Clear();
                            }
                        }

                        if (stack_depth == 0)
                        {
                            let_begin = region_block;
                            break;
                        }
                        else if (pushes_waiting_for_dom.Count == 0)
                        {
                            let_region.Insert(0, region_block);
                        }
                        else
                        {
                            pending_blocks.Insert(0, region_block);
                        }
                    }
                    if (let_begin != null)
                    {
                        // potentially valid region. check if it is a SESE region
                        if (let_region.Any(x => !let_begin.Dominates(x)))
                        {
                            Console.WriteLine("Failed to create region for let: entry did not dominate all nodes");
                        }
                        else if (let_region.Any(x => !let_end.PostDominates(x)))
                        {
                            Console.WriteLine("Failed to create region for let: exit did not postdominate all nodes");
                        }
                        else if (let_region.Any(x => x.Prev.Any(p => p != let_begin && !let_region.Contains(p)) || x.Next.Any(n => n != let_end && !let_region.Contains(n))))
                        {
                            Console.WriteLine("Failed to create region for let: some node escapes the region");
                        }
                        else
                        {
                            // valid region! we can create it now.
                            var new_region = new GOOLDecompBlockRegion($"let_{let_begin.Name}_{let_end.Name}", let_begin, let_end);
                            new_region.GenerateCFG();
                            new_region.GenerateDominationTree();
                            new_region.StructureLets();
                            // our graph was changed. re-generate it and try to structure more!
                            GenerateCFG();
                            GenerateDominationTree();
                            StructureLets();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to create region for let");
                    }
                }
            }
        }

        public void StructureLoops();
        public void StructureLoopsInt(List<GOOLDecompBlock> polist)
        {
            GOOLDecompLoop CreateLoopFromEdge(GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock? prebranch = null)
            {
                // https://www.backerstreet.com/decompiler/loop_analysis.php
                // > Then, since to reach the (tail) block execution must go through its dominator,
                // > we can traverse the predecessors of each block starting from the tail, until we reach the header block

                // this can probably be optimized to just grab a range using a postordering list instead of this stack approach?
                Stack<GOOLDecompBlock> workList = new();

                // note: in pretested loops, header is actually the tail and tail is the pre-tail block. the real header is pointed to by the tail.
                var real_header = header;
                var real_tail = tail;
                if (prebranch != null)
                {
                    real_tail = header;
                    real_header = real_tail.Next.Find(x => x.OfsBegin < real_tail.OfsEnd)!;
                }

                GOOLDecompLoop loop = new(real_header, real_tail, prebranch);

                loop.BlockList.Add(real_header);
                if (real_header != real_tail)
                {
                    loop.BlockList.Add(real_tail);
                    workList.Push(real_tail);
                }
                if (prebranch != null)
                {
                    loop.BlockList.Add(prebranch);
                }

                while (workList.Count > 0)
                {
                    var block = workList.Pop();
                    foreach (var prev in block.Prev)
                    {
                        if (!loop.BlockList.Contains(prev))
                        {
                            loop.BlockList.Add(prev);
                            workList.Push(prev);
                        }
                    }
                }

                loop.BlockList.Sort((a, b) => a.OfsBegin - b.OfsBegin);
                return loop;
            }

            // find all loops in graph
            List<GOOLDecompLoop> processed_loops = new();
            List<GOOLDecompLoop> loops = new();
            foreach (var block in BlockList)
            {
                if (block is IGOOLDecompBlockIterator it) it.StructureLoops();
                foreach (var next in block.Next)
                {
                    // block will proceed into a different block that dominates us - i.e. if we went to the start of a loop and this was a back edge!
                    // that means block is the tail (where the loop ends) and the thing it goes to is the head (where the loop begins)
                    // block = before end of loop; next = end of loop (has branch & condition check) (note: can be the same when loop is only 1 block large)
                    if (next.Dominates(block))
                    {
                        // try to find a "pre-branch". an unconditional branch to the loop condition block. this is a pre-tested loop, so (while) instead of (until)
                        // if the loop condition check is immediately dominated by an unconditional branch that came before, that's a prebranch
                        // unconditional branches also appear in if-else constructs, but they dont immediately jump to the middle of loops, so this is okay.
                        GOOLDecompBlock? prebranch = null;
                        if (next.Prev.Contains(next.ImmDom) && next.ImmDom.Type == GoolBranchType.Goto && next.ImmDom.OfsBegin < next.OfsBegin)
                        {
                            prebranch = next.ImmDom;
                        }
                        var loop = CreateLoopFromEdge(next, block, prebranch);
                        if (!loops.Any(l => l.Header == loop.Header && l.Tail == loop.Tail && l.PreBranch == loop.PreBranch))
                        {
                            loops.Add(loop);
                        }
                    }
                }
            }

            foreach (var loop in loops)
            {
                loop.LoopDepth = 0;
                foreach (var otherloop in loops)
                {
                    if (otherloop == loop) continue;
                    if (loop.BlockList.All(otherloop.BlockList.Contains))
                    {
                        loop.LoopDepth++;
                    }
                }
            }

            // sort loops by 'depth' (descending, so deepest first) and assign children
            loops.Sort((a, b) => b.LoopDepth - a.LoopDepth);

            foreach (var loop in loops)
            {
                processed_loops.Add(loop);
                var do_while = new GOOLDecompBlockDoWhile($"dowhile_{loop.Header.Name}_{loop.Tail.Name}", loop.Header, loop.Tail, loop.PreBranch);
                do_while.GenerateCFG();
                do_while.GenerateDominationTree();
                do_while.StructureBreakContinue();
                // loops can share headers (i.e. (until cond1 (until cond2 ..) ..), but not tails or prebranches (i think)
                foreach (var otherloop in loops.Except(processed_loops))
                {
                    if (otherloop.Header == loop.Header)
                    {
                        otherloop.Header = do_while;
                    }
                }
                // regenerate CFG since we changed control flow
                GenerateCFG();
                GenerateDominationTree();
            }
        }
    }

    public class GOOLDecompFunction(string name) : IGOOLDecompBlockIterator
    {
        public int Offset { get; set; }
        public bool Trans { get; set; }
        public string Name { get; set; } = name;
        public List<GOOLDecompBlock> BlockList { get; } = new();

        public GOOLDecompBlock start;

        public bool HasError { get; set; }

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(start);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public void StructureLets()
        {
            (this as IGOOLDecompBlockIterator).StructureLetsInt((this as IGOOLDecompBlockIterator).AsPostOrderList());
        }

        public void StructureLoops()
        {
            (this as IGOOLDecompBlockIterator).StructureLoopsInt((this as IGOOLDecompBlockIterator).AsPostOrderList());
        }
    }
}
