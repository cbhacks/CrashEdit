namespace Crash
{
    public struct SLSTPolygonID
    {
        public SLSTPolygonID(short id, byte state, byte world)
        {
            ID = id;
            State = state;
            World = world;
        }

        public short ID { get; }
        public byte State { get; }
        public byte World { get; }
    }
}
