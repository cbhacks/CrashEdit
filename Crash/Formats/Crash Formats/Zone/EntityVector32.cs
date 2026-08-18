namespace CrashEdit.Crash
{
    public readonly struct EntityVector32
    {
        public EntityVector32(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public EntityVector32(Position p)
        {
            X = (int)p.X;
            Y = (int)p.Y;
            Z = (int)p.Z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
    }
}
