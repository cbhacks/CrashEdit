namespace Crash
{
    public struct T4PolygonID
    {
        private short id;
        private short world;

        public T4PolygonID(short id,short world)
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
