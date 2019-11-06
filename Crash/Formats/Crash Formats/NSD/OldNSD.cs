using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldNSD
    {
        public static OldNSD Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 1032)
            {
                ErrorManager.SignalError("OldNSD: Data is too short");
            }
            int entrycount = BitConv.FromInt32(data,1028);
            if (data.Length < 1312 + entrycount * 8)
            {
                ErrorManager.SignalError("OldNSD: Data is too short");
            }
            int[] unknown1 = new int [256];
            for (int i = 0;i < 256;i++)
            {
                unknown1[i] = BitConv.FromInt32(data,i * 4);
            }
            int chunkcount = BitConv.FromInt32(data,1024);
            int[] leveldata = new int [4];
            for (int i = 0;i < 4;i++)
            {
                leveldata[i] = BitConv.FromInt32(data, 1032 + i * 4);
            }
            int unknown2 = BitConv.FromInt32(data,1048);
            int preludecount = BitConv.FromInt32(data,1052);
            int[] unknown3 = new int [64];
            for (int i = 0;i < 64;i++)
            {
                unknown3[i] = BitConv.FromInt32(data,1056 + i * 4);
            }
            if (chunkcount < 0)
            {
                ErrorManager.SignalError("OldNSD: Chunk count is negative");
            }
            if (entrycount < 0)
            {
                ErrorManager.SignalError("OldNSD: Entry count is negative");
            }
            if (data.Length < 1312 + 8 * entrycount)
            {
                ErrorManager.SignalError("OldNSD: Data is too short");
            }
            NSDLink[] index = new NSDLink [entrycount];
            for (int i = 0;i < entrycount;i++)
            {
                int chunkid = BitConv.FromInt32(data,1312 + 8 * i);
                int entryid = BitConv.FromInt32(data,1316 + 8 * i);
                index[i] = new NSDLink(chunkid,entryid);
            }
            int magic = BitConv.FromInt32(data,1312 + 8 * entrycount);
            if (magic != 1)
            {
                ErrorManager.SignalIgnorableError("OldNSD: Magic number is wrong");
                magic = 1;
            }
            int id = BitConv.FromInt32(data,1316 + 8 * entrycount);
            int startzone = BitConv.FromInt32(data,1320 + 8 * entrycount);
            int camera = BitConv.FromInt32(data,1324 + 8 * entrycount);
            int unknown4 = BitConv.FromInt32(data,1328 + 8 * entrycount);
            int extralength = data.Length - (1332 + 8 * entrycount);
            byte[] extradata = new byte [extralength];
            Array.Copy(data,1328 + 8 * entrycount,extradata,0,extralength);
            return new OldNSD(unknown1,entrycount,chunkcount,leveldata,unknown2,preludecount,unknown3,index,magic,id,startzone,camera,unknown4,extradata);
        }

        private List<NSDLink> index;

        public OldNSD(int[] unknown1,int entrycount,int chunkcount,int[] leveldata,int unknown2,int preludecount,int[] unknown3,IEnumerable<NSDLink> index,int magic,int id,int startzone,int camera,int unknown4,byte[] extradata)
        {
            if (unknown1 == null)
                throw new ArgumentNullException("unknown1");
            if (unknown3 == null)
                throw new ArgumentNullException("unknown2");
            if (index == null)
                throw new ArgumentNullException("index");
            if (unknown1.Length != 256)
                throw new ArgumentException("Value must be 256 ints long.","unknown1");
            if (unknown3.Length != 64)
                throw new ArgumentException("Value must be 64 ints long.","unknown3");
            Unknown1 = unknown1;
            EntryCount = entrycount;
            ChunkCount = chunkcount;
            PreludeCount = preludecount;
            Unknown2 = unknown2;
            LevelData = leveldata ?? throw new ArgumentNullException("leveldata");
            Unknown3 = unknown3;
            this.index = new List<NSDLink>(index);
            Magic = magic;
            ID = id;
            StartZone = startzone;
            Camera = camera;
            Unknown4 = unknown4;
            ExtraData = extradata ?? throw new ArgumentNullException("extradata");
        }

        public int[] Unknown1 { get; }
        public int EntryCount { get; set; }
        public int ChunkCount { get; set; }
        public int Unknown2 { get; set; }
        public int PreludeCount { get; set; }
        public int[] LevelData { get; }
        public int[] Unknown3 { get; }
        public IList<NSDLink> Index => index;
        public int Magic { get; set; }
        public int ID { get; set; }
        public int StartZone { get; set; }
        public int Camera { get; set; }
        public int Unknown4 { get; set; }
        public byte[] ExtraData { get; }

        public byte[] Save()
        {
            int entrycount = Index.Count;
            byte[] result = new byte [1332 + (8 * entrycount) + ExtraData.Length];
            for (int i = 0;i < 256;i++)
            {
                BitConv.ToInt32(result,i * 4,Unknown1[i]);
            }
            BitConv.ToInt32(result,1024,ChunkCount);
            BitConv.ToInt32(result,1028,entrycount);
            for (int i = 0; i < LevelData.Length;i++)
            {
                BitConv.ToInt32(result,1032 + i * 4,LevelData[i]);
            }
            // Keeps preludes
            //BitConv.ToInt32(result,1032,unknown2);
            //BitConv.ToInt32(result,1036,preludecount);
            BitConv.ToInt32(result, 1312 + 8 * entrycount, Magic);
            BitConv.ToInt32(result,1316 + 8 * entrycount,ID);
            BitConv.ToInt32(result,1320 + 8 * entrycount,StartZone);
            BitConv.ToInt32(result,1324 + 8 * entrycount,Camera);
            BitConv.ToInt32(result,1328 + 8 * entrycount,Unknown4);
            // Keeps preludes
            /*for (int i = 0;i < 64;i++)
            {
                BitConv.ToInt32(result,1056 + i * 4,unknown3[i]);
            }*/
            for (int i = 0;i < entrycount;i++)
            {
                BitConv.ToInt32(result,1312 + i * 8,index[i].ChunkID);
                BitConv.ToInt32(result,1316 + i * 8,index[i].EntryID);
            }
            Array.Copy(ExtraData,0,result,1328 + entrycount * 8,ExtraData.Length);
            return result;
        }
    }
}
