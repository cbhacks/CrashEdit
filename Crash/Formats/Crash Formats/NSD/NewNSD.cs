using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NewNSD
    {
        public static NewNSD Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 2044)
            {
                ErrorManager.SignalError("NewNSD: Data is too short");
            }
            int chunkcount = BitConv.FromInt32(data,0x400);
            if (chunkcount < 0)
            {
                ErrorManager.SignalError("NewNSD: Chunk count is negative");
            }
            int entrycount = BitConv.FromInt32(data,0x404);
            if (entrycount < 0)
            {
                ErrorManager.SignalError("NewNSD: Entry count is negative");
            }
            if (data.Length < 2044 + entrycount * 8)
            {
                ErrorManager.SignalError("NewNSD: Data is too short");
            }
            int[] hashkeymap = new int [256];
            for (int i = 0;i < 256;i++)
            {
                hashkeymap[i] = BitConv.FromInt32(data,i*4);
            }
            int[] leveldata = new int [4];
            for (int i = 0;i < 4;i++)
            {
                leveldata[i] = BitConv.FromInt32(data,0x408+i*4);
            }
            int uncompressedchunksec = BitConv.FromInt32(data,0x418);
            int preludecount = BitConv.FromInt32(data,0x41C);
            int[] compressedchunkinfo = new int [64];
            for (int i = 0;i < 64;i++)
            {
                compressedchunkinfo[i] = BitConv.FromInt32(data,0x420 + i * 4);
            }
            NSDLink[] index = new NSDLink [entrycount];
            for (int i = 0;i < entrycount;i++)
            {
                int chunkid = BitConv.FromInt32(data,0x520+8*i);
                int entryid = BitConv.FromInt32(data,0x520+8*i+4);
                index[i] = new NSDLink(chunkid,entryid);
            }
            int spawncount = BitConv.FromInt32(data,0x520+8*entrycount);
            if (spawncount <= 0)
            {
                ErrorManager.SignalIgnorableError("NewNSD: Not enough spawn points");
            }
            if (data.Length < 2044 + entrycount * 8 + spawncount * 24)
            {
                ErrorManager.SignalError("NewNSD: Data is too short");
            }
            int blank = BitConv.FromInt32(data,0x524+8*entrycount);
            if (blank != 0)
            {
                ErrorManager.SignalIgnorableError("NewNSD: Blank value is not blank");
                blank = 0;
            }
            int id = BitConv.FromInt32(data,0x528+8*entrycount);
            int entitycount = BitConv.FromInt32(data,0x52C+8*entrycount);
            int[] goolmap = new int [128];
            for (int i = 0;i < 128;++i)
            {
                goolmap[i] = BitConv.FromInt32(data,0x530+8*entrycount+i*4);
            }
            int extralength = 0xCC;
            byte[] extradata = new byte [extralength];
            Array.Copy(data,0x730+8*entrycount,extradata,0,extralength);
            NSDSpawnPoint[] spawns = new NSDSpawnPoint[spawncount];
            for (int i = 0;i < spawncount;++i)
            {
                int zone = BitConv.FromInt32(data,0x7FC+8*entrycount+24*i);
                int camera = BitConv.FromInt32(data,0x7FC+8*entrycount+24*i+4);
                int unknown = BitConv.FromInt32(data,0x7FC+8*entrycount+24*i+8);
                int x = BitConv.FromInt32(data,0x7FC+8*entrycount+24*i+12);
                int y = BitConv.FromInt32(data,0x7FC+8*entrycount+24*i+16);
                int z = BitConv.FromInt32(data,0x7FC+8*entrycount+24*i+20);
                spawns[i] = new NSDSpawnPoint(zone,camera,unknown,x,y,z);
            }
            extralength = data.Length - (0x7FC+8*entrycount+24*spawncount);
            byte[] imagedata = new byte [extralength];
            Array.Copy(data,data.Length-extralength,imagedata,0,extralength);
            return new NewNSD(hashkeymap,chunkcount,leveldata,uncompressedchunksec,preludecount,compressedchunkinfo,index,blank,id,entitycount,goolmap,extradata,spawns,imagedata);
        }

        private List<NSDSpawnPoint> spawns;

        public NewNSD(int[] hashkeymap,int chunkcount,int[] leveldata,int uncompressedchunksec,int preludecount,int[] compressedchunkinfo,IEnumerable<NSDLink> index,int blank,int id,int entitycount,int[] goolmap,byte[] extradata,IEnumerable<NSDSpawnPoint> spawns,byte[] imagedata)
        {
            if (hashkeymap == null)
                throw new ArgumentNullException("firstentries");
            if (leveldata == null)
                throw new ArgumentNullException("leveldata");
            if (compressedchunkinfo == null)
                throw new ArgumentNullException("compressedchunkinfo");
            if (index == null)
                throw new ArgumentNullException("index");
            if (goolmap == null)
                throw new ArgumentNullException("goolmap");
            if (spawns == null)
                throw new ArgumentNullException("spawns");
            if (hashkeymap.Length != 256)
                throw new ArgumentException("Value must be 256 ints long.", "firstentries");
            if (leveldata.Length != 4)
                throw new ArgumentException("Value must be 4 ints long.", "leveldata");
            if (compressedchunkinfo.Length != 64)
                throw new ArgumentException("Value must be 64 ints long.", "compressedchunkinfo");
            if (goolmap.Length != 128)
                throw new ArgumentException("Value must be 128 ints long.", "goolmap");
            HashKeyMap = hashkeymap;
            ChunkCount = chunkcount;
            LevelData = leveldata;
            UncompressedChunkSec = uncompressedchunksec;
            PreludeCount = preludecount;
            CompressedChunkInfo = compressedchunkinfo;
            Index = new List<NSDLink>(index);
            Blank = blank;
            ID = id;
            EntityCount = entitycount;
            GOOLMap = goolmap;
            ExtraData = extradata ?? throw new ArgumentNullException("extradata");
            this.spawns = new List<NSDSpawnPoint>(spawns);
            ImageData = imagedata ?? throw new ArgumentNullException("image");
        }

        public int[] HashKeyMap { get; set; }
        public int ChunkCount { get; set; }
        public int[] LevelData { get; }
        public int UncompressedChunkSec { get; set; }
        public int PreludeCount { get; set; }
        public int[] CompressedChunkInfo { get; }
        public IList<NSDLink> Index { get; set; }
        public int Blank { get; set; }
        public int ID { get; set; }
        public int EntityCount { get; set; }
        public int[] GOOLMap { get; }
        public byte[] ExtraData { get; }
        public IList<NSDSpawnPoint> Spawns => spawns;
        public byte[] ImageData { get; }

        public byte[] Save()
        {
            int entrycount = Index.Count;
            int spawncount = spawns.Count;
            byte[] result = new byte [0x730+8*entrycount+24*spawncount + ExtraData.Length + ImageData.Length];
            for (int i = 0;i < 256;i++)
            {
                BitConv.ToInt32(result,i*4,HashKeyMap[i]);
            }
            BitConv.ToInt32(result,0x400,ChunkCount);
            BitConv.ToInt32(result,0x404,entrycount);
            for (int i = 0; i < LevelData.Length;i++)
            {
                BitConv.ToInt32(result,0x408+i*4,LevelData[i]);
            }
            // Keeps preludes
            //BitConv.ToInt32(result,0x418,UncompressedChunkSec);
            //BitConv.ToInt32(result,0x41C,PreludeCount);
            //for (int i = 0;i < 64;++i)
            //{
            //    BitConv.ToInt32(result,0x420+i*4,CompressedChunkInfo[i]);
            //}
            for (int i = 0;i < entrycount;++i)
            {
                BitConv.ToInt32(result,0x520+i*8,Index[i].ChunkID);
                BitConv.ToInt32(result,0x524+i*8,Index[i].EntryID);
            }
            BitConv.ToInt32(result,0x520+8*entrycount,spawncount);
            BitConv.ToInt32(result,0x524+8*entrycount,Blank);
            BitConv.ToInt32(result,0x528+8*entrycount,ID);
            BitConv.ToInt32(result,0x52C+8*entrycount,EntityCount);
            for (int i = 0; i < 128;++i)
            {
                BitConv.ToInt32(result,0x530+8*entrycount+i*4,GOOLMap[i]);
            }
            Array.Copy(ExtraData,0,result,0x730+entrycount*8,ExtraData.Length);
            for (int i = 0; i < spawncount;++i)
            {
                BitConv.ToInt32(result,0x730+8*entrycount+ExtraData.Length+i*24+0,spawns[i].ZoneEID);
                BitConv.ToInt32(result,0x730+8*entrycount+ExtraData.Length+i*24+4,spawns[i].Camera);
                BitConv.ToInt32(result,0x730+8*entrycount+ExtraData.Length+i*24+8,spawns[i].Unknown);
                BitConv.ToInt32(result,0x730+8*entrycount+ExtraData.Length+i*24+12,spawns[i].SpawnX);
                BitConv.ToInt32(result,0x730+8*entrycount+ExtraData.Length+i*24+16,spawns[i].SpawnY);
                BitConv.ToInt32(result,0x730+8*entrycount+ExtraData.Length+i*24+20,spawns[i].SpawnZ);
            }
            Array.Copy(ImageData,0,result,0x730+entrycount*8+ExtraData.Length+spawncount*24,ImageData.Length);
            return result;
        }
    }
}
