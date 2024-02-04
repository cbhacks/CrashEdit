namespace Crash
{
    public readonly struct Position : IPosition
    {
        public static readonly Position Unit = new Position(1, 1, 1);

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(IPosition pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Position operator -(Position a, Position b) => new Position(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Position operator *(Position a, Position b) => new Position(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Position operator /(Position a, Position b) => new Position(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Position operator *(Position a, float b) => new Position(a.X * b, a.Y * b, a.Z * b);
        public static Position operator *(float b, Position a) => new Position(a.X * b, a.Y * b, a.Z * b);
        public static Position operator /(Position a, float b) => new Position(a.X / b, a.Y / b, a.Z / b);
        public static Position operator /(float b, Position a) => new Position(a.X / b, a.Y / b, a.Z / b);
    }
}
