namespace CrashEdit.Crash
{
    public readonly struct FrameVertex
    {
        public FrameVertex(byte x, byte y, byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public byte X { get; }
        public byte Y { get; }
        public byte Z { get; }
    }
}
