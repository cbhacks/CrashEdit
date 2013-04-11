namespace Crash.Game
{
    public struct EntityPosition
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
    }
}
