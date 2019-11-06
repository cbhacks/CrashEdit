namespace Crash
{
    public struct Position : IPosition
    {
        public Position(double x,double y,double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
    }
}
