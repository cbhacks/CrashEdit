namespace CrashEdit.Crash
{
    public class GOOLDecompLoop(GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock? prebranch)
    {
        // (while...) blocks are pre-tested by having an unconditional branch straight to the test
        public GOOLDecompBlock? PreBranch { get; set; } = prebranch;
        public GOOLDecompBlock Header { get; set; } = header;
        public GOOLDecompBlock Tail { get; set; } = tail;
        public List<GOOLDecompBlock> BlockList { get; } = new();
        public int LoopDepth { get; set; } = -1;
    }
}
