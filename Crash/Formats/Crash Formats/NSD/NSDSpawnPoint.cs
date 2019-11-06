namespace Crash
{
    public sealed class NSDSpawnPoint
    {
        public NSDSpawnPoint(int zoneeid,int unk1, int unk2,int spawnx,int spawny,int spawnz)
        {
            ZoneEID = zoneeid;
            Unknown1 = unk1;
            Unknown2 = unk2;
            SpawnX = spawnx;
            SpawnY = spawny;
            SpawnZ = spawnz;
        }

        public int ZoneEID { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int SpawnX { get; set; }
        public int SpawnY { get; set; }
        public int SpawnZ { get; set; }
    }
}
