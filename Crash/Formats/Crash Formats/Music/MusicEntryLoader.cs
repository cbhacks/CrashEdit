using System;

namespace Crash
{
    [EntryType(13,GameVersion.Crash1BetaMAY11)]
    [EntryType(13,GameVersion.Crash1)]
    [EntryType(13,GameVersion.Crash2)]
    [EntryType(13,GameVersion.Crash3)]
    public sealed class MusicEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3)
            {
                ErrorManager.SignalError("MusicEntry: Wrong number of items");
            }
            if (items[0].Length != 36)
            {
                ErrorManager.SignalError("MusicEntry: First item length is wrong");
            }
            int seqcount = BitConv.FromInt32(items[0],0);
            int vheid = BitConv.FromInt32(items[0],4);
            int vb0eid = BitConv.FromInt32(items[0],8);
            int vb1eid = BitConv.FromInt32(items[0],12);
            int vb2eid = BitConv.FromInt32(items[0],16);
            int vb3eid = BitConv.FromInt32(items[0],20);
            int vb4eid = BitConv.FromInt32(items[0],24);
            int vb5eid = BitConv.FromInt32(items[0],28);
            int vb6eid = BitConv.FromInt32(items[0],32);
            VH vh;
            if (items[1].Length != 0)
            {
                vh = VH.Load(items[1]);
            }
            else
            {
                vh = null;
            }
            SEP sep = SEP.Load(items[2],seqcount);
            return new MusicEntry(vheid,vb0eid,vb1eid,vb2eid,vb3eid,vb4eid,vb5eid,vb6eid,vh,sep,eid,size);
        }
    }
}
