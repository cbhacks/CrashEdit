namespace CrashEdit.Crash
{
    public class GOOLDecompLoop(GOOLDecompBlock header, GOOLDecompBlock tail)
    {
        // (while...) blocks are pre-tested by having an unconditional branch straight to the test
        public GOOLDecompBlock? PreBranch { get; set; } = null;
        public GOOLDecompBlock Header { get; set; } = header;
        public GOOLDecompBlock Tail { get; set; } = tail;
        public List<GOOLDecompBlock> BlockList { get; } = new();
        public int LoopDepth { get; set; } = -1;
        public GOOLDecompLoop Parent { get; set; } = null;
        public List<GOOLDecompLoop> Children { get; } = new();
    }
}
