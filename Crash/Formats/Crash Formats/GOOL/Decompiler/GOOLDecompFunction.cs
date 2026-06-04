using System;

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
            }, postVisit: (block) =>
            {
                block.PostOrderID = poid++;
            });
            BlockList.Sort((a, b) => a.begin - b.begin);
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
                if (block.next.Count == 0)
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
                foreach (var block in BlockList)
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

        public void StructureLets(GOOLDecompiler decompiler);
        public void StructureLetsInt(GOOLDecompiler decompiler, List<GOOLDecompBlock> polist)
        {
            for (int i = 0; i < polist.Count; ++i)
            {
                var block = polist[i];
                if (block.Type == GoolBranchType.StackPop || block.IsLetEnd())
                {
                    GOOLDecompBlock let_end = block;
                    GOOLDecompBlock let_begin = null;
                    List<GOOLDecompBlock> let_region = new();
                    int stack_depth = block.IsLetEnd() ? 1 : block.stackpop;
                    for (int j = i + 1; j < polist.Count; ++j)
                    {
                        var region_block = polist[j];
                        if (region_block.IsLetBegin())
                        {
                            stack_depth -= 1;
                        }
                        else if (region_block.IsLetEnd())
                        {
                            stack_depth += 1;
                        }
                        else if (region_block.Type == GoolBranchType.StackPop)
                        {
                            stack_depth += region_block.stackpop;
                        }

                        if (stack_depth == 0)
                        {
                            let_begin = region_block;
                            break;
                        }
                        else
                        {
                            let_region.Insert(0, region_block);
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
                        else if (let_region.Any(x => x.prev.Any(p => p != let_begin && !let_region.Contains(p)) || x.next.Any(n => n != let_end && !let_region.Contains(n))))
                        {
                            Console.WriteLine("Failed to create region for let: some node escapes the region");
                        }
                        else
                        {
                            // valid region! we can create it now.
                            var new_region = new GOOLDecompBlockRegion($"let_{let_begin.Name}_{let_end.Name}", let_begin, let_end, let_region);
                            decompiler.blocks.Add(new_region);
                            new_region.GenerateCFG();
                            new_region.GenerateDominationTree();
                            new_region.StructureLets(decompiler);
                            // our graph was changed. re-generate it and try to structure more!
                            GenerateCFG();
                            decompiler.MakeBlockPrevLists();
                            GenerateDominationTree();
                            StructureLets(decompiler);
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
    }

    public class GOOLDecompFunction(string name) : IGOOLDecompBlockIterator
    {
        public int Offset { get; set; }
        public bool Trans { get; set; }
        public string Name { get; set; } = name;
        public List<GOOLDecompBlock> BlockList { get; } = new();
        public List<GOOLDecompLoop> LoopList { get; } = new();

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

        public void StructureLets(GOOLDecompiler decompiler)
        {
            (this as IGOOLDecompBlockIterator).StructureLetsInt(decompiler, (this as IGOOLDecompBlockIterator).AsPostOrderList());
        }
    }
}
