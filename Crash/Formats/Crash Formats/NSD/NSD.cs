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
            NSDLink[] index = new NSDLink [entrycount];
            for (int i = 0;i < entrycount;i++)
            {
                int chunkid = BitConv.FromInt32(data,1312 + 8 * i);
                int entryid = BitConv.FromInt32(data,1312 + 8 * i + 4);
                index[i] = new NSDLink(chunkid,entryid);
            }
            int spawnpoints = BitConv.FromInt32(data,1312 + 8 * entrycount);
            if (spawnpoints < 1)
            {
                ErrorManager.SignalError("NSD: Too few spawn points");
            }
            if (data.Length < 1328 + 8 * entrycount + 24 * spawnpoints)
            {
                ErrorManager.SignalError("NSD: Data is too short");
            }
            int nsdblank = BitConv.FromInt32(data,1316 + 8 * entrycount);
            if (nsdblank != 0)
            {
                ErrorManager.SignalIgnorableError("NSD: Blank value is not blank");
                nsdblank = 0;
            }
            int levelid = BitConv.FromInt32(data,1320 + 8 * entrycount);
            int objcount = BitConv.FromInt32(data,1324 + 8 * entrycount);
            int extralength = data.Length - (1328 + 8 * entrycount + 24 * spawnpoints);
            byte[] extradata = new byte [extralength];
            Array.Copy(data, 1328 + 8 * entrycount, extradata, 0, extralength);
            NSDSpawnPoint[] spawnpointindex = new NSDSpawnPoint [spawnpoints];
            for (int i = 0;i < spawnpoints;i++)
            {
                int zoneeid = BitConv.FromInt32(data,1328 + 24 * i + 8 * entrycount + extradata.Length);
                int unk1 = BitConv.FromInt32(data,1332 + 24 * i + 8 * entrycount + extradata.Length);
                int unk2 = BitConv.FromInt32(data,1336 + 24 * i + 8 * entrycount + extradata.Length);
                int spawnx = BitConv.FromInt32(data,1340 + 24 * i + 8 * entrycount + extradata.Length);
                int spawny = BitConv.FromInt32(data,1344 + 24 * i + 8 * entrycount + extradata.Length);
                int spawnz = BitConv.FromInt32(data,1348 + 24 * i + 8 * entrycount + extradata.Length);
                spawnpointindex[i] = new NSDSpawnPoint(zoneeid,unk1,unk2,spawnx,spawny,spawnz);
            }
            return new NSD(unknown1,entrycount,chunkcount,unknown2,index,spawnpoints,nsdblank,levelid,objcount,extradata,spawnpointindex);
        }

        private List<NSDLink> index;
        private List<NSDSpawnPoint> spawnpointindex;

        public NSD(int[] unknown1,int entrycount,int chunkcount,int[] unknown2,IEnumerable<NSDLink> index,int spawnpoints,int nsdblank,int levelid,int objcount,byte[] extradata,IEnumerable<NSDSpawnPoint> spawnpointindex)
        {
            if (unknown1 == null)
                throw new ArgumentNullException("unknown1");
            if (unknown2 == null)
                throw new ArgumentNullException("unknown2");
            if (index == null)
                throw new ArgumentNullException("index");
            if (unknown1.Length != 256)
                throw new ArgumentException("Value must be 256 ints long.","unknown1");
            if (unknown2.Length != 70)
                throw new ArgumentException("Value must be 70 ints long.","unknown2");
            Unknown1 = unknown1;
            EntryCount = entrycount;
            ChunkCount = chunkcount;
            Unknown2 = unknown2;
            this.index = new List<NSDLink>(index);
            SpawnPoints = spawnpoints;
            NSDBlank = nsdblank;
            LevelID = levelid;
            ObjCount = objcount;
            ExtraData = extradata ?? throw new ArgumentNullException("extradata");
            this.spawnpointindex = new List<NSDSpawnPoint>(spawnpointindex);
        }

        public int[] Unknown1 { get; }
        public int EntryCount { get; set; }
        public int ChunkCount { get; set; }
        public int[] Unknown2 { get; }
        public IList<NSDLink> Index => index;
        public int SpawnPoints { get; set; }
        public int NSDBlank { get; set; }
        public int LevelID { get; set; }
        public int ObjCount { get; set; }
        public byte[] ExtraData { get; }
        public IList<NSDSpawnPoint> SpawnPointIndex => spawnpointindex;

        public byte[] Save()
        {
            byte[] result = new byte [1328 + (8 * EntryCount) + (24 * SpawnPoints) + ExtraData.Length];
            for (int i = 0;i < 256;i++)
            {
                BitConv.ToInt32(result,i * 4,Unknown1[i]);
            }
            BitConv.ToInt32(result,1024,ChunkCount);
            BitConv.ToInt32(result,1028,EntryCount);
            BitConv.ToInt32(result,1312 + 8 * EntryCount,SpawnPoints);
            BitConv.ToInt32(result,1316 + 8 * EntryCount,NSDBlank);
            BitConv.ToInt32(result,1320 + 8 * EntryCount,LevelID);
            BitConv.ToInt32(result,1324 + 8 * EntryCount,ObjCount);
            for (int i = 0;i < 70;i++)
            {
                BitConv.ToInt32(result,1032 + i * 4,Unknown2[i]);
            }
            for (int i = 0;i < EntryCount;i++)
            {
                BitConv.ToInt32(result,1312 + i * 8,index[i].ChunkID);
                BitConv.ToInt32(result,1316 + i * 8,index[i].EntryID);
            }
            Array.Copy(ExtraData, 0, result, 1328 + EntryCount * 8, ExtraData.Length);
            for (int i = 0;i < SpawnPoints;i++)
            {
                BitConv.ToInt32(result,1328 + (EntryCount * 8) + i * 24 + ExtraData.Length,spawnpointindex[i].ZoneEID);
                BitConv.ToInt32(result,1340 + (EntryCount * 8) + i * 24 + ExtraData.Length,spawnpointindex[i].SpawnX);
                BitConv.ToInt32(result,1344 + (EntryCount * 8) + i * 24 + ExtraData.Length,spawnpointindex[i].SpawnY);
                BitConv.ToInt32(result,1348 + (EntryCount * 8) + i * 24 + ExtraData.Length,spawnpointindex[i].SpawnZ);
            }
            return result;
        }
    }
}
