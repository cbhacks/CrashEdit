using System.Windows.Forms;

namespace CrashEdit.Crash
{
    public class GOOLDecompFunction(string name)
    {
        public int Offset { get; set; }
        public bool Trans { get; set; }
        public string Name { get; set; } = name;
        public List<GOOLDecompBlock> BlockList { get; } = new();
        public List<GOOLDecompLoop> LoopList { get; } = new();

        public GOOLDecompBlock start;

        public void GenerateCFG()
        {
            start.GenerateCFGAsRoot(BlockList);
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
            }
        }
    }
}
