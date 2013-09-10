namespace Crash
{
    public struct Position : IPosition
    {
        private double x;
        private double y;
        private double z;

        public Position(double x,double y,double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }

        public double Z
        {
            get { return z; }
        }
    }
}
