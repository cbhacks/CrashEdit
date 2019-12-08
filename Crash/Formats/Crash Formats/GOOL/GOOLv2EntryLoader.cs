using System;

namespace Crash
{
    [EntryType(11,GameVersion.Crash2)]
    [EntryType(11,GameVersion.Crash3)]
    public sealed class GOOLv2EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("GOOLv2Entry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("GOOLv2Entry: Header length is wrong");
            }
            int[] data = new int [items[2].Length/4];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = BitConv.FromInt32(items[2],i*4);
            }
            short[] statemap = null;
            GOOLStateDescriptor[] statedesc = null;
            if (items.Length > 3)
            {
                statemap = new short[items[3].Length/2];
                for (int i = 0; i < statemap.Length; ++i)
                {
                    statemap[i] = BitConv.FromInt16(items[3],i*2);
                }
                if (items.Length > 4)
                {
                    statedesc = new GOOLStateDescriptor[items[4].Length/0x10];
                    for (int i = 0; i < statedesc.Length; ++i)
                    {
                        statedesc[i] = new GOOLStateDescriptor(
                            BitConv.FromInt32(items[4],i*0x10+0),
                            BitConv.FromInt32(items[4],i*0x10+4),
                            BitConv.FromInt16(items[4],i*0x10+8),
                            BitConv.FromInt16(items[4],i*0x10+10),
                            BitConv.FromInt16(items[4],i*0x10+12),
                            BitConv.FromInt16(items[4],i*0x10+14)
                            );
                    }
                }
            }
            return new GOOLEntry(GOOLVersion.Version2,items[0],items[1],data,
                statemap,
                statedesc,
                items.Length >= 6 ? items[5] : null,
                eid,size);
        }
    }
}
