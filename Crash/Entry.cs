using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class Entry
    {
        public const int Magic = 0x100FFFF;

        private static Dictionary<int,EntryLoader> loaders;

        static Entry()
        {
            loaders = new Dictionary<int,EntryLoader>();
        }

        public static Entry Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 16)
            {
                throw new LoadException();
            }
            int magic = BitConv.FromWord(data,0);
            int unknown = BitConv.FromWord(data,4);
            int type = BitConv.FromWord(data,8);
            int itemcount = BitConv.FromWord(data,12);
            if (magic != Magic)
            {
                throw new LoadException();
            }
            if (itemcount < 0)
            {
                throw new LoadException();
            }
            if (data.Length < 20 + itemcount * 4)
            {
                throw new LoadException();
            }
            byte[][] items = new byte [itemcount][];
            byte[] itemdata;
            for (int i = 0;i < itemcount;i++)
            {
                int itemstart = BitConv.FromWord(data,16 + i * 4);
                int itemend = BitConv.FromWord(data,20 + i * 4);
                if (itemstart < 0)
                {
                    throw new LoadException();
                }
                if (itemend < itemstart)
                {
                    throw new LoadException();
                }
                if (itemend > data.Length)
                {
                    throw new LoadException();
                }
                int itemsize = itemend - itemstart;
                itemdata = new byte [itemsize];
                Array.Copy(data,itemstart,itemdata,0,itemsize);
                items[i] = itemdata;
            }
            if (loaders.ContainsKey(type))
            {
                return loaders[type].Load(items,unknown);
            }
            else
            {
                return new UnknownEntry(items,unknown,type);
            }
        }

        public static void AddLoader(int type,EntryLoader loader)
        {
            if (loader == null)
                throw new ArgumentNullException("loader");
            loaders.Add(type,loader);
        }

        private int unknown;

        public Entry(int unknown)
        {
            this.unknown = unknown;
        }

        public abstract int Type
        {
            get;
        }

        public int Unknown
        {
            get { return unknown; }
        }

        public abstract byte[] Save();

        protected byte[] Save(IList<byte[]> items)
        {
            return Save(items,4);
        }

        protected byte[] Save(IList<byte[]> items,int align)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (align < 0)
                throw new ArgumentOutOfRangeException("Align cannot be negative.");
            if (align == 0)
                throw new ArgumentOutOfRangeException("Align cannot be zero.");
            int length = 20 + items.Count * 4;
            Aligner.Align(ref length,align);
            foreach (byte[] item in items)
            {
                length += item.Length;
                Aligner.Align(ref length,align);
            }
            byte[] data = new byte [length];
            BitConv.ToWord(data,0,Magic);
            BitConv.ToWord(data,4,Unknown);
            BitConv.ToWord(data,8,Type);
            BitConv.ToWord(data,12,items.Count);
            int offset = 20 + items.Count * 4;
            Aligner.Align(ref offset,align);
            BitConv.ToWord(data,16,offset);
            for (int i = 0;i < items.Count;i++)
            {
                items[i].CopyTo(data,offset);
                offset += items[i].Length;
                Aligner.Align(ref offset,align);
                BitConv.ToWord(data,20 + i * 4,offset);
            }
            return data;
        }
    }
}
