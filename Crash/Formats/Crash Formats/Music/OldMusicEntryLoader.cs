using System;

namespace Crash
{
    [EntryType(13,GameVersion.Crash1BetaMAR08)]
    public sealed class OldMusicEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3)
            {
                ErrorManager.SignalError("OldMusicEntry: Wrong number of items");
            }
            if (items[0].Length != 20)
            {
                ErrorManager.SignalError("OldMusicEntry: First item length is wrong");
            }
            int seqcount = BitConv.FromInt32(items[0],0);
            int vb0eid = BitConv.FromInt32(items[0],4);
            int vb1eid = BitConv.FromInt32(items[0],8);
            int vb2eid = BitConv.FromInt32(items[0],12);
            int vb3eid = BitConv.FromInt32(items[0],16);
            VH vh = VH.Load(items[1]);
            SEP sep = SEP.Load(items[2],seqcount);
            return new OldMusicEntry(vb0eid,vb1eid,vb2eid,vb3eid,vh,sep,eid,size);
        }
    }
}
