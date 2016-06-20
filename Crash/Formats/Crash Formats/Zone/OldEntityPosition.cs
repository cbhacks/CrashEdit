namespace Crash
{
    public struct OldEntityPosition : IPosition
    {
        private short x;
        private short y;
        private short z;

        public OldEntityPosition(short x,short y,short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public short X
        {
            get { return x; }
            set { x = value; }
        }

        public short Y
        {
            get { return y; }
            set { y = value; }
        }

        public short Z
        {
            get { return z; }
            set { z = value; }
        }

        double IPosition.X
        {
            get { return x; }
        }

        double IPosition.Y
        {
            get { return y; }
        }

        double IPosition.Z
        {
            get { return z; }
        }
    }
}
