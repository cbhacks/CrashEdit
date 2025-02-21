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
    }
}
