namespace Crash
{
    public struct Position : IPosition
    {
        public static readonly Position Unit = new Position(1,1,1);

        public Position(double x,double y,double z)
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

        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Position operator -(Position a, Position b) => new Position(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Position operator *(Position a, Position b) => new Position(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Position operator /(Position a, Position b) => new Position(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Position operator *(Position a, double b) => new Position(a.X * b, a.Y * b, a.Z * b);
        public static Position operator *(double b, Position a) => new Position(a.X * b, a.Y * b, a.Z * b);
        public static Position operator /(Position a, double b) => new Position(a.X / b, a.Y / b, a.Z / b);
        public static Position operator /(double b, Position a) => new Position(a.X / b, a.Y / b, a.Z / b);
    }
}
