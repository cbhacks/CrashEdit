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
            if (data.Length < 1312)
            {
                throw new LoadException();
            }
            int[] unknown1 = new int [256];
            for (int i = 0;i < 256;i++)
            {
                unknown1[i] = BitConv.FromInt32(data,i * 4);
            }
            int chunkcount = BitConv.FromInt32(data,1024);
            int entrycount = BitConv.FromInt32(data,1028);
            int[] unknown2 = new int [70];
            for (int i = 0;i < 70;i++)
            {
                unknown2[i] = BitConv.FromInt32(data,1032 + i * 4);
            }
            if (chunkcount < 0)
            {
                throw new LoadException();
            }
            if (entrycount < 0)
            {
                throw new LoadException();
            }
            if (data.Length < 1312 + 8 * entrycount)
            {
                throw new LoadException();
            }
            NSDLink[] index = new NSDLink [entrycount];
            for (int i = 0;i < entrycount;i++)
            {
                int chunkid = BitConv.FromInt32(data,1312 + 8 * i);
                int entryid = BitConv.FromInt32(data,1316 + 8 * i);
                index[i] = new NSDLink(chunkid,entryid);
            }
            int extralength = data.Length - (1312 + 8 * entrycount);
            byte[] extradata = new byte [extralength];
            Array.Copy(data,1312 + 8 * entrycount,extradata,0,extralength);
            return new NSD(unknown1,chunkcount,unknown2,index,extradata);
        }

        private int[] unknown1;
        private int chunkcount;
        private int[] unknown2;
        private List<NSDLink> index;
        private byte[] extradata;

        public NSD(int[] unknown1,int chunkcount,int[] unknown2,IEnumerable<NSDLink> index,byte[] extradata)
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
            this.chunkcount = chunkcount;
            this.unknown2 = unknown2;
            this.index = new List<NSDLink>(index);
            this.extradata = extradata;
        }

        public int[] Unknown1
        {
            get { return unknown1; }
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

        public byte[] ExtraData
        {
            get { return extradata; }
        }

        public byte[] Save()
        {
            byte[] result = new byte [1312 + 8 * index.Count + extradata.Length];
            for (int i = 0;i < 256;i++)
            {
                BitConv.ToInt32(result,i * 4,unknown1[i]);
            }
            BitConv.ToInt32(result,1024,chunkcount);
            BitConv.ToInt32(result,1028,index.Count);
            for (int i = 0;i < 70;i++)
            {
                BitConv.ToInt32(result,1032 + i * 4,unknown2[i]);
            }
            for (int i = 0;i < index.Count;i++)
            {
                BitConv.ToInt32(result,1312 + i * 8,index[i].ChunkID);
                BitConv.ToInt32(result,1316 + i * 8,index[i].EntryID);
            }
            Array.Copy(extradata,0,result,1312 + index.Count * 8,extradata.Length);
            return result;
        }
    }
}
