namespace Crash
{
    public struct OldEntityPosition : IPosition
    {
        public OldEntityPosition(short x,short y,short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;
    }
}
