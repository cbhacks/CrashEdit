namespace Crash
{
    public struct SLSTPolygonID
    {
        private short id;
        private short world;

        public SLSTPolygonID(short id,short world)
        {
            this.id = id;
            this.world = world;
        }

        public short ID
        {
            get { return id; }
        }

        public short World
        {
            get { return world; }
        }
    }
}
