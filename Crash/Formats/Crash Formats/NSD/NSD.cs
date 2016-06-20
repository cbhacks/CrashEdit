using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NSD
    {
        public static NSD Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 1032)
            {
                ErrorManager.SignalError("NSD: Data is too short");
            }
            int entrycount = BitConv.FromInt32(data,1028);
            if (data.Length < 1312 + entrycount * 8)
            {
                if (data.Length < (1032 + entrycount * 8))
                {
                    ErrorManager.SignalError("NSD: Data is too short");
                }
            }
            int[] unknown1 = new int [256];
            for (int i = 0;i < 256;i++)
            {
                unknown1[i] = BitConv.FromInt32(data,i * 4);
            }
            int chunkcount = BitConv.FromInt32(data,1024);
            int[] unknown2 = new int [70];
            for (int i = 0;i < 70;i++)
            {
                unknown2[i] = BitConv.FromInt32(data,1032 + i * 4);
            }
            if (chunkcount < 0)
            {
                ErrorManager.SignalError("NSD: Chunk count is negative");
            }
            if (entrycount < 0)
            {
                ErrorManager.SignalError("NSD: Entry count is negative");
            }
            if (data.Length < 1312 + 8 * entrycount)
            {
                ErrorManager.SignalError("NSD: Data is too short");
            }
            NSDLink[] index = new NSDLink [entrycount];
            for (int i = 0;i < entrycount;i++)
            {
                int chunkid = BitConv.FromInt32(data,1312 + 8 * i);
                int entryid = BitConv.FromInt32(data,1316 + 8 * i);
                index[i] = new NSDLink(chunkid,entryid);
            }
            int spawnpoints = BitConv.FromInt32(data,1312 + 8 * entrycount);
            if (spawnpoints < 1)
            {
                ErrorManager.SignalError("NSD: Too little spawn points");
            }
            int nsdblank = BitConv.FromInt32(data,1316 + 8 * entrycount);
            if (nsdblank != 0)
            {
                ErrorManager.SignalIgnorableError("NSD: Blank value is not blank");
                nsdblank = 0;
            }
            int levelid = BitConv.FromInt32(data,1320 + 8 * entrycount);
            int objcount = BitConv.FromInt32(data,1324 + 8 * entrycount);
            int extralength = data.Length - (1328 + (8 * entrycount) + (24 * spawnpoints));
            byte[] extradata = new byte [extralength];
            NSDSpawnPoint[] spawnpointindex = new NSDSpawnPoint [spawnpoints];
            for (int i = 0;i < spawnpoints;i++)
            {
                int zoneeid = BitConv.FromInt32(data,1328 + 24 * i + 8 * entrycount + extradata.Length);
                int spawnx = BitConv.FromInt32(data,1340 + 24 * i + 8 * entrycount + extradata.Length);
                int spawny = BitConv.FromInt32(data,1344 + 24 * i + 8 * entrycount + extradata.Length);
                int spawnz = BitConv.FromInt32(data,1348 + 24 * i + 8 * entrycount + extradata.Length);
                spawnpointindex[i] = new NSDSpawnPoint(zoneeid,spawnx,spawny,spawnz);
            }
            Array.Copy(data,1328 + 8 * entrycount,extradata,0,extralength);
            return new NSD(unknown1,entrycount,chunkcount,unknown2,index,spawnpoints,nsdblank,levelid,objcount,extradata,spawnpointindex);
        }

        private int[] unknown1;
        private int entrycount;
        private int chunkcount;
        private int[] unknown2;
        private List<NSDLink> index;
        private int spawnpoints;
        private int nsdblank;
        private int levelid;
        private int objcount;
        private byte[] extradata;
        private List<NSDSpawnPoint> spawnpointindex;

        public NSD(int[] unknown1,int entrycount,int chunkcount,int[] unknown2,IEnumerable<NSDLink> index,int spawnpoints,int nsdblank,int levelid,int objcount,byte[] extradata,IEnumerable<NSDSpawnPoint> spawnpointindex)
        {
            if (unknown1 == null)
                throw new ArgumentNullException("unknown1");
            if (unknown2 == null)
                throw new ArgumentNullException("unknown2");
            if (index == null)
                throw new ArgumentNullException("index");
            if (extradata == null)
                throw new ArgumentNullException("extradata");
            if (unknown1.Length != 256)
                throw new ArgumentException("Value must be 256 ints long.","unknown1");
            if (unknown2.Length != 70)
                throw new ArgumentException("Value must be 70 ints long.","unknown2");
            this.unknown1 = unknown1;
            this.entrycount = entrycount;
            this.chunkcount = chunkcount;
            this.unknown2 = unknown2;
            this.index = new List<NSDLink>(index);
            this.spawnpoints = spawnpoints;
            this.nsdblank = nsdblank;
            this.levelid = levelid;
            this.objcount = objcount;
            this.extradata = extradata;
            this.spawnpointindex = new List<NSDSpawnPoint>(spawnpointindex);
        }

        public int[] Unknown1
        {
            get { return unknown1; }
        }

        public int EntryCount
        {
            get { return entrycount; }
            set { entrycount = value; }
        }

        public int ChunkCount
        {
            get { return chunkcount; }
            set { chunkcount = value; }
        }

        public int[] Unknown2
        {
            get { return unknown2; }
        }

        public IList<NSDLink> Index
        {
            get { return index; }
        }

        public int SpawnPoints
        {
            get { return spawnpoints; }
            set { spawnpoints = value; }
        }

        public int NSDBlank
        {
            get { return nsdblank; }
            set { nsdblank = value; }
        }

        public int LevelID
        {
            get { return levelid; }
            set { levelid = value; }
        }

        public int ObjCount
        {
            get { return objcount; }
            set { objcount = value; }
        }

        public byte[] ExtraData
        {
            get { return extradata; }
        }
        public IList<NSDSpawnPoint> SpawnPointIndex
        {
            get { return spawnpointindex; }
        }

        public byte[] Save()
        {
            byte[] result = new byte [1328 + (8 * entrycount) + (24 * spawnpoints) + extradata.Length];
            for (int i = 0;i < 256;i++)
            {
                BitConv.ToInt32(result,i * 4,unknown1[i]);
            }
            BitConv.ToInt32(result,1024,chunkcount);
            BitConv.ToInt32(result,1028,entrycount);
            BitConv.ToInt32(result,1312 + 8 * entrycount,spawnpoints);
            BitConv.ToInt32(result,1316 + 8 * entrycount,nsdblank);
            BitConv.ToInt32(result,1320 + 8 * entrycount,levelid);
            BitConv.ToInt32(result,1324 + 8 * entrycount,objcount);
            for (int i = 0;i < 70;i++)
            {
                BitConv.ToInt32(result,1032 + i * 4,unknown2[i]);
            }
            for (int i = 0;i < entrycount;i++)
            {
                BitConv.ToInt32(result,1312 + i * 8,index[i].ChunkID);
                BitConv.ToInt32(result,1316 + i * 8,index[i].EntryID);
            }
            for (int i = 0;i < spawnpoints;i++)
            {
                BitConv.ToInt32(result,1328 + (entrycount * 8) + i * 24 + extradata.Length,spawnpointindex[i].ZoneEID);
                BitConv.ToInt32(result,1340 + (entrycount * 8) + i * 24 + extradata.Length,spawnpointindex[i].SpawnX);
                BitConv.ToInt32(result,1344 + (entrycount * 8) + i * 24 + extradata.Length,spawnpointindex[i].SpawnY);
                BitConv.ToInt32(result,1348 + (entrycount * 8) + i * 24 + extradata.Length,spawnpointindex[i].SpawnZ);
            }
            Array.Copy(extradata,0,result,1328 + entrycount * 8,extradata.Length);
            return result;
        }
    }
}
