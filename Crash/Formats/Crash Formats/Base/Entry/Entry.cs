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
        public static readonly string[] Errors = {
            "EID is not 5 characters long.",
            "EID has invalid characters.",
            "EID cannot be \"NONE!\"",
            "EID already exists.",
            "EID final character mismatch."
        };

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
            for (int i = 0;i < itemcount;++i)
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
            return new UnprocessedEntry(items,eid,type);
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

        public static int ENameToEID(string str)
        {
            if (str.Length != 5)
                throw new ArgumentException("ENameToEID: Incorrect length for EName.");
            int eid = 1;
            for (int i = 0; i < 5; i++)
            {
                int c = ENameCharacterSet.IndexOf(str[i]);
                if (c == -1)
                    throw new ArgumentException(string.Format("ENameToEID: Invalid EName character: {0}.", str[i]));
                eid |= c << (25 - 6 * i);
            }
            return eid;
        }

        public Entry(int eid)
        {
            EID = eid;
        }

        public abstract int Type { get; }
        public int EID { get; set; }
        public string EName => EIDToEName(EID);
        public int HashKey => EID >> 15 & 0xFF;

        public virtual bool IgnoreResaveErrors => false;

        public abstract UnprocessedEntry Unprocess();

        public virtual byte[] Save()
        {
            return Unprocess().Save();
        }

        ///<summary> Verifies the integrity of an entry name and returns an error string if it is invalid, returns and empty string on success.</summary>
        ///<param name="ename">An entry name to verify.</param>
        ///<param name="allownull">Determines whether to error out if entry name is the null entry name.</param>
        ///<param name="nsf">An NSF in which to find duplicate entry names in. If null or unspecified, the pre-existing entry check is skipped.</param>
        public static string CheckEIDErrors(string ename, bool allownull, NSF nsf = null)
        {
            if (ename.Length != 5)
            {
                return Errors[0];
            }
            int eid = NullEID;
            try
            {
                eid = ENameToEID(ename);
            }
            catch (ArgumentException)
            {
                return Errors[1];
            }
            if (eid == NullEID && !allownull)
            {
                return Errors[2];
            }
            if (nsf != null)
            {
                IEntry existingentry = nsf.GetEntry<Entry>(eid);
                if (existingentry == null)
                {
                    existingentry = nsf.GetEntry<TextureChunk>(eid);
                }
                if (existingentry != null)
                {
                    return Errors[3];
                }
            }
            return string.Empty;
        }
    }
}
