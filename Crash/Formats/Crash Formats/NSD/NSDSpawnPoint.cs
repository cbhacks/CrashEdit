namespace Crash
{
    public sealed class NSDSpawnPoint
    {
        public NSDSpawnPoint(int zoneeid,int camera,int unknown,int spawnx,int spawny,int spawnz)
        {
            ZoneEID = zoneeid;
            Camera = camera;
            Unknown = unknown;
            SpawnX = spawnx;
            SpawnY = spawny;
            SpawnZ = spawnz;
        }

        public int ZoneEID { get; set; }
        public int Camera { get; set; }
        public int Unknown { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }
    }
}
