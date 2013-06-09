using System;
using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public abstract class Entry : IEntry
    {
        public const int Magic = 0x100FFFF;
        public const int NullEID = 0x6396347F;

        internal static Dictionary<int,EntryLoader> loaders;

        static Entry()
        {
            loaders = new Dictionary<int,EntryLoader>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (EntryTypeAttribute attribute in type.GetCustomAttributes(typeof(EntryTypeAttribute),false))
                {
                    EntryLoader loader = (EntryLoader)Activator.CreateInstance(type);
                    loaders.Add(attribute.Type,loader);
                }
            }
        }

        public static UnprocessedEntry Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 16)
            {
                throw new LoadException();
            }
            int magic = BitConv.FromInt32(data,0);
            int eid = BitConv.FromInt32(data,4);
            int type = BitConv.FromInt32(data,8);
            int itemcount = BitConv.FromInt32(data,12);
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
                int itemstart = BitConv.FromInt32(data,16 + i * 4);
                int itemend = BitConv.FromInt32(data,20 + i * 4);
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
            return new UnprocessedEntry(items,eid,type);
        }

        private int eid;

        public Entry(int eid)
        {
            this.eid = eid;
        }

        public abstract int Type
        {
            get;
        }

        public int EID
        {
            get { return eid; }
        }

        public abstract UnprocessedEntry Unprocess();

        public virtual byte[] Save()
        {
            return Unprocess().Save();
        }
    }
}
