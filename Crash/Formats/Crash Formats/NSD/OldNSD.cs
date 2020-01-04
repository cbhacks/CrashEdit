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
            if (data.Length < 1672)
            {
                ErrorManager.SignalError("OldNSD: Data is too short");
            }
            int chunkcount = BitConv.FromInt32(data,0x400);
            if (chunkcount < 0)
            {
                ErrorManager.SignalError("OldNSD: Chunk count is negative");
            }
            int entrycount = BitConv.FromInt32(data,0x404);
            if (entrycount < 0)
            {
                ErrorManager.SignalError("OldNSD: Entry count is negative");
            }
            if (data.Length < 1672 + entrycount * 8)
            {
                ErrorManager.SignalError("OldNSD: Data is too short");
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
            int magic = BitConv.FromInt32(data,0x520+8*entrycount);
            if (magic != 1)
            {
                ErrorManager.SignalIgnorableError("OldNSD: Magic number is wrong");
                magic = 1;
            }
            int id = BitConv.FromInt32(data,0x524+8*entrycount);
            int startzone = BitConv.FromInt32(data,0x528+8*entrycount);
            int camera = BitConv.FromInt32(data,0x52C+8*entrycount);
            int unknown = BitConv.FromInt32(data,0x530+8*entrycount);
            int[] goolmap = new int [64];
            for (int i = 0;i < 64;++i)
            {
                goolmap[i] = BitConv.FromInt32(data,0x534+8*entrycount+i*4);
            }
            int extralength = data.Length - (0x634+8*entrycount);
            byte[] extradata = new byte [extralength];
            Array.Copy(data,data.Length-extralength,extradata,0,extralength);
            return new OldNSD(hashkeymap,chunkcount,leveldata,uncompressedchunksec,preludecount,compressedchunkinfo,index,magic,id,startzone,camera,unknown,goolmap,extradata);
        }

        public OldNSD(int[] hashkeymap,int chunkcount,int[] leveldata,int uncompressedchunksec,int preludecount,int[] compressedchunkinfo,IEnumerable<NSDLink> index,int magic,int id,int startzone,int camera,int unknown,int[] goolmap,byte[] extradata)
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
            if (hashkeymap.Length != 256)
                throw new ArgumentException("Value must be 256 ints long.", "firstentries");
            if (leveldata.Length != 4)
                throw new ArgumentException("Value must be 4 ints long.", "leveldata");
            if (compressedchunkinfo.Length != 64)
                throw new ArgumentException("Value must be 64 ints long.", "compressedchunkinfo");
            if (goolmap.Length != 64)
                throw new ArgumentException("Value must be 64 ints long.", "goolmap");
            HashKeyMap = hashkeymap;
            ChunkCount = chunkcount;
            LevelData = leveldata;
            UncompressedChunkSec = uncompressedchunksec;
            PreludeCount = preludecount;
            CompressedChunkInfo = compressedchunkinfo;
            Index = new List<NSDLink>(index);
            Magic = magic;
            ID = id;
            StartZone = startzone;
            Camera = camera;
            Unknown = unknown;
            GOOLMap = goolmap;
            ExtraData = extradata ?? throw new ArgumentNullException("extradata");
        }

        public int[] HashKeyMap { get; set; }
        public int ChunkCount { get; set; }
        public int[] LevelData { get; }
        public int UncompressedChunkSec { get; set; }
        public int PreludeCount { get; set; }
        public int[] CompressedChunkInfo { get; }
        public IList<NSDLink> Index { get; set; }
        public int Magic { get; set; }
        public int ID { get; set; }
        public int StartZone { get; set; }
        public int Camera { get; set; }
        public int Unknown { get; set; }
        public int[] GOOLMap { get; }
        public byte[] ExtraData { get; }

        public byte[] Save()
        {
            int entrycount = Index.Count;
            byte[] result = new byte [0x634+8*entrycount + ExtraData.Length];
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
            BitConv.ToInt32(result,0x520+8*entrycount, Magic);
            BitConv.ToInt32(result,0x524+8*entrycount,ID);
            BitConv.ToInt32(result,0x528+8*entrycount,StartZone);
            BitConv.ToInt32(result,0x52C+8*entrycount,Camera);
            BitConv.ToInt32(result,0x530+8*entrycount,Unknown);
            for (int i = 0; i < 64;++i)
            {
                BitConv.ToInt32(result,0x534+8*entrycount+i*4,GOOLMap[i]);
            }
            Array.Copy(ExtraData,0,result,0x634+entrycount*8,ExtraData.Length);
            return result;
        }
    }
}
