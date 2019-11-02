namespace Crash
{
    public struct FrameVertex : IPosition
    {
        public FrameVertex(byte x,byte y,byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public byte X { get; }
        public byte Y { get; }
        public byte Z { get; }

        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;
    }
}
