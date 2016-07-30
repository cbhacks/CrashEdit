using System;
using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public abstract class Entry : IEntry
    {
        public const int Magic = 0x100FFFF;
        public const int NullEID = 0x6396347F;
        public const string NullEName = "NONE!";
        public const string ENameCharacterSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_!";

        internal static Dictionary<GameVersion,Dictionary<int,EntryLoader>> loadersets;

        static Entry()
        {
            loadersets = new Dictionary<GameVersion,Dictionary<int,EntryLoader>>();
        }

        internal static Dictionary<int,EntryLoader> GetLoaders(GameVersion gameversion)
        {
            if (!loadersets.ContainsKey(gameversion))
            {
                Dictionary<int,EntryLoader> loaders = new Dictionary<int,EntryLoader>();
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    foreach (EntryTypeAttribute attribute in type.GetCustomAttributes(typeof(EntryTypeAttribute),false))
                    {
                        if (attribute.GameVersion == gameversion)
                        {
                            EntryLoader loader = (EntryLoader)Activator.CreateInstance(type);
                            loaders.Add(attribute.Type,loader);
                        }
                    }
                }
                loadersets.Add(gameversion,loaders);
            }
            return loadersets[gameversion];
        }

        public static UnprocessedEntry Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 16)
            {
                ErrorManager.SignalError("Entry: Data is too short");
            }
            int magic = BitConv.FromInt32(data,0);
            int eid = BitConv.FromInt32(data,4);
            int type = BitConv.FromInt32(data,8);
            int itemcount = BitConv.FromInt32(data,12);
            if (magic != Magic)
            {
                ErrorManager.SignalIgnorableError("Entry: Magic number is wrong");
            }
            if (itemcount < 0)
            {
                ErrorManager.SignalError("Entry: Item count is negative");
            }
            if (data.Length < 20 + itemcount * 4)
            {
                ErrorManager.SignalError("Entry: Data is too short");
            }
            byte[][] items = new byte [itemcount][];
            byte[] itemdata;
            for (int i = 0;i < itemcount;i++)
            {
                int itemstart = BitConv.FromInt32(data,16 + i * 4);
                int itemend = BitConv.FromInt32(data,20 + i * 4);
                if (itemstart < 0)
                {
                    ErrorManager.SignalError("Entry: Item begins out of bounds");
                }
                if (itemend < itemstart)
                {
                    ErrorManager.SignalError("Entry: Item ends before it begins");
                }
                if (itemend > data.Length)
                {
                    ErrorManager.SignalError("Entry: Item ends out of bounds");
                }
                int itemsize = itemend - itemstart;
                itemdata = new byte [itemsize];
                Array.Copy(data,itemstart,itemdata,0,itemsize);
                items[i] = itemdata;
            }
            int size = data.Length;
            return new UnprocessedEntry(items,eid,type,size);
        }

        public static string EIDToEName(int eid)
        {
            char[] str = new char [5];
            eid >>= 1;
            for (int i = 0;i < 5;i++)
            {
                str[4 - i] = ENameCharacterSet[eid & 0x3F];
                eid >>= 6;
            }
            return new string(str);
        }

        public static string EIDToEName(int? eid)
        {
            char[] str = new char[5];
            eid >>= 1;
            for (int i = 0; i < 5; i++)
            {
                str[4 - i] = ENameCharacterSet[(int)eid & 0x3F];
                eid >>= 6;
            }
            return new string(str);
        }

        // Special thanks to NeoKesha for this
        public static int? Str2EID(string str)
        {
            int EID = 1;
            for (byte i = 0; i < 5; i++)
            {
                byte chr_id = SeekCharId(str[i]);
                EID = EID | (chr_id << (1 + 6 * i));
            }
            return EID;
        }

        // And this
        public static byte SeekCharId(char chr)
        {
            byte i = 0;
            while (i < 64 && !(chr == ENameCharacterSet[i]))
            {
                i++;
            }
            return i;
        }

        private int eid;
        private int size;

        public Entry(int eid,int size)
        {
            this.eid = eid;
            this.size = size;
        }

        public abstract int Type
        {
            get;
        }

        public int EID
        {
            get { return eid; }
        }

        public string EName
        {
            get { return EIDToEName(eid); }
        }

        public int Size
        {
            get { return size; }
        }

        public abstract UnprocessedEntry Unprocess();

        public virtual byte[] Save()
        {
            return Unprocess().Save();
        }
    }
}
