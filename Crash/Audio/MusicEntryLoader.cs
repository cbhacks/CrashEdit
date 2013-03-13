using System;

namespace Crash.Audio
{
    [EntryType(13)]
    public sealed class MusicEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3)
            {
                throw new LoadException();
            }
            byte[] unknown1 = items[0];
            int seqcount = BitConv.FromInt32(unknown1,0);
            if (BitConv.FromInt32(unknown1,24) != 0x6396347F ||
                BitConv.FromInt32(unknown1,28) != 0x6396347F ||
                BitConv.FromInt32(unknown1,32) != 0x6396347F)
            {
                throw new LoadException();
            }
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
            return new MusicEntry(unknown1,vh,sep,unknown);
        }
    }
}
