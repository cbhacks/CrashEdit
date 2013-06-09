using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class UnprocessedEntry : MysteryMultiItemEntry
    {
        private int type;

        public UnprocessedEntry(IEnumerable<byte[]> items,int eid,int type) : base(items,eid)
        {
            this.type = type;
        }

        public override int Type
        {
            get { return type; }
        }

        public Entry Process()
        {
            if (loaders.ContainsKey(type))
            {
                return loaders[type].Load(((List<byte[]>)Items).ToArray(),EID);
            }
            else
            {
                throw new LoadException();
            }
        }

        public override UnprocessedEntry Unprocess()
        {
            return this;
        }

        public override byte[] Save()
        {
            int length = 20 + Items.Count * 4;
            Aligner.Align(ref length,4);
            foreach (byte[] item in Items)
            {
                length += item.Length;
                Aligner.Align(ref length,4);
            }
            byte[] data = new byte [length];
            BitConv.ToInt32(data,0,Magic);
            BitConv.ToInt32(data,4,EID);
            BitConv.ToInt32(data,8,Type);
            BitConv.ToInt32(data,12,Items.Count);
            int offset = 20 + Items.Count * 4;
            Aligner.Align(ref offset,4);
            BitConv.ToInt32(data,16,offset);
            for (int i = 0;i < Items.Count;i++)
            {
                Items[i].CopyTo(data,offset);
                offset += Items[i].Length;
                Aligner.Align(ref offset,4);
                BitConv.ToInt32(data,20 + i * 4,offset);
            }
            return data;
        }
    }
}
