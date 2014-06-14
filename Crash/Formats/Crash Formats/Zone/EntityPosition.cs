namespace Crash
{
    public struct EntityPosition : IPosition
    {
        private short x;
        private short y;
        private short z;

        public EntityPosition(short x,short y,short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public short X
        {
            get { return x; }
        }

        public short Y
        {
            get { return y; }
        }

        public short Z
        {
            get { return z; }
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
