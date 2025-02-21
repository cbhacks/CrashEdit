namespace CrashEdit.Crash
{
    public interface IGOOLDecompBlockIterator
    {
        public List<GOOLDecompBlock> BlockList { get; }

        public void GenerateCFG(GOOLDecompBlock start)
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
                if (block == this)
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

        public void GenerateDominationTree()
        {
            // recursive call and clear stale values
            foreach (var block in BlockList)
            {
                block.ImmPostDom = null;
                block.ImmDom = null;
                if (block is IGOOLDecompBlockIterator bl)
                    bl.GenerateDominationTree();
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

        public void StructureIfElse()
        {
            HashSet<GOOLDecompBlock> unresolved = [];
            var as_po = AsPostOrderList();
            Dictionary<GOOLDecompBlock, GOOLDecompBlock> follows = [];

            foreach (var block in as_po)
            {
                if (block.Type == GoolBranchType.If)
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
                else if (block is IGOOLDecompBlockIterator bl)
                {
                    bl.StructureIfElse();
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

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFG(start);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTree();
        }

        public void StructureIfElse()
        {
            (this as IGOOLDecompBlockIterator).StructureIfElse();
        }
    }
}
