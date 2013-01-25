using System;

namespace Crash.Audio
{
    [EntryType(14)]
    public sealed class WavebankEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("Items cannot be null.");
            if (items.Length != 2)
            {
                throw new LoadException();
            }
            if (items[0].Length != 8)
            {
                throw new LoadException();
            }
            int id = BitConv.FromWord(items[0],0);
            int length = BitConv.FromWord(items[0],4);
            if (id < 0 || id > 3)
            {
                throw new LoadException();
            }
            if (length != items[1].Length)
            {
                throw new LoadException();
            }
            return new WavebankEntry(id,SampleSet.Load(items[1]),unknown);
        }
    }
}
