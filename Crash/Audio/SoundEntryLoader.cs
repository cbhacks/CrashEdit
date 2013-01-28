using System;

namespace Crash.Audio
{
    [EntryType(12)]
    public sealed class SoundEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 1)
            {
                throw new LoadException();
            }
            return new SoundEntry(SampleSet.Load(items[0]),unknown);
        }
    }
}
