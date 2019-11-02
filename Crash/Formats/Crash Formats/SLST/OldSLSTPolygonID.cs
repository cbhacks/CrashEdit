namespace Crash
{
    public struct OldSLSTPolygonID
    {
        public OldSLSTPolygonID(short id,byte world,byte copy)
        {
            ID = id;
            World = world;
            Copy = copy;
        }

        public short ID { get; }
        public byte World { get; }
        public byte Copy { get; }
    }
}
