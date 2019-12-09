using System;
using System.Collections.Generic;

namespace Crash
{
    [EntryType(11,GameVersion.Crash1Beta1995)]
    public sealed class OldGOOLEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("OldGOOLEntry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("OldGOOLEntry: Header length is wrong");
            }
            int[] ins = new int [items[2].Length/4];
            for (int i = 0; i < ins.Length; ++i)
            {
                ins[i] = BitConv.FromInt32(items[2],i*4);
            }
            short[] statemap = null;
            GOOLStateDescriptor[] statedesc = null;
            byte[] anims = null;
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
                    if (items.Length > 5)
                    {
                        anims = items[5];
                    }
                }
            }
            return new GOOLEntry(GOOLVersion.Version0,items[0],items[1],ins,
                statemap,
                statedesc,
                anims,
                eid,size);
        }
    }
}
