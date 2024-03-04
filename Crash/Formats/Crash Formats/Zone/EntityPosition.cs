namespace CrashEdit.Crash
{
    public readonly struct EntityPosition
    {
        public EntityPosition(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public EntityPosition(Position p)
        {
            X = (short)p.X;
            Y = (short)p.Y;
            Z = (short)p.Z;
        }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }
    }
}
