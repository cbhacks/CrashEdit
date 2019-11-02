namespace Crash
{
    public struct EntityPosition : IPosition
    {
        public EntityPosition(short x,short y,short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }
        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;
    }
}
