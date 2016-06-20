namespace Crash
{
    public sealed class NSDSpawnPoint
    {
        private int zoneeid;
        private int spawnx;
        private int spawny;
        private int spawnz;

        public NSDSpawnPoint(int zoneeid,int spawnx,int spawny,int spawnz)
        {
            this.zoneeid = zoneeid;
            this.spawnx = spawnx;
            this.spawny = spawny;
            this.spawnz = spawnz;
        }

        public int ZoneEID
        {
            get { return zoneeid; }
            set { zoneeid = value; }
        }

        public int SpawnX
        {
            get { return spawnx; }
            set { spawnx = value; }
        }

        public int SpawnY
        {
            get { return spawny; }
            set { spawny = value; }
        }

        public int SpawnZ
        {
            get { return spawnz; }
            set { spawnz = value; }
        }
    }
}
