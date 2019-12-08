using System;

namespace Crash
{
    [EntryType(11,GameVersion.Crash1BetaMAR08)]
    [EntryType(11,GameVersion.Crash1BetaMAY11)]
    [EntryType(11,GameVersion.Crash1)]
    public sealed class GOOLEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("GOOLEntry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("GOOLEntry: Header length is wrong");
            }
            short[] statemap = null;
            if (items.Length > 3)
            {
                statemap = new short[items[3].Length/2];
                for (int i = 0; i < statemap.Length; ++i)
                {
                    statemap[i] = BitConv.FromInt16(items[3],i*2);
                }
            }
            return new GOOLEntry(GOOLVersion.Version1,items[0],items[1],items[2],
                statemap,
                items.Length >= 5 ? items[4] : null,
                items.Length >= 6 ? items[5] : null,
                eid,size);
        }
    }
}
