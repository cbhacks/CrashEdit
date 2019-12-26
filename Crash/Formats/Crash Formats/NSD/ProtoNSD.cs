using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoNSD
    {
        public static ProtoNSD Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 0x408)
            {
                ErrorManager.SignalError("ProtoNSD: Data is too short");
            }
            int chunkcount = BitConv.FromInt32(data,0x400);
            if (chunkcount < 0)
            {
                ErrorManager.SignalError("ProtoNSD: Chunk count is negative");
            }
            int entrycount = BitConv.FromInt32(data,0x404);
            if (entrycount < 0)
            {
                ErrorManager.SignalError("ProtoNSD: Entry count is negative");
            }
            if (data.Length < 0x408 + entrycount * 8)
            {
                ErrorManager.SignalError("ProtoNSD: Data is too short");
            }
            int[] firstentries = new int [256];
            for (int i = 0;i < 256;i++)
            {
                firstentries[i] = BitConv.FromInt32(data,i*4);
            }
            NSDLink[] index = new NSDLink [entrycount];
            for (int i = 0;i < entrycount;i++)
            {
                int chunkid = BitConv.FromInt32(data,0x520+8*i);
                int entryid = BitConv.FromInt32(data,0x520+8*i+4);
                index[i] = new NSDLink(chunkid,entryid);
            }
            return new ProtoNSD(firstentries,chunkcount,index);
        }

        private List<NSDLink> index;

        public ProtoNSD(int[] firstentries,int chunkcount,IEnumerable<NSDLink> index)
        {
            if (firstentries == null)
                throw new ArgumentNullException("firstentries");
            if (index == null)
                throw new ArgumentNullException("index");
            FirstEntries = firstentries;
            ChunkCount = chunkcount;
            this.index = new List<NSDLink>(index);
        }

        public int[] FirstEntries { get; }
        public int ChunkCount { get; set; }
        public IList<NSDLink> Index => index;

        public byte[] Save()
        {
            int entrycount = Index.Count;
            byte[] result = new byte [0x408+8*entrycount];
            for (int i = 0;i < 256;i++)
            {
                BitConv.ToInt32(result,i*4,FirstEntries[i]);
            }
            BitConv.ToInt32(result,0x400,ChunkCount);
            BitConv.ToInt32(result,0x404,entrycount);
            for (int i = 0;i < entrycount;++i)
            {
                BitConv.ToInt32(result,0x408+i*8,index[i].ChunkID);
                BitConv.ToInt32(result,0x408+i*8,index[i].EntryID);
            }
            return result;
        }
    }
}
