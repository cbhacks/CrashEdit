using System;

namespace Crash
{
    [EntryType(12)]
    public sealed class SoundEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 1)
            {
                ErrorManager.SignalError("SoundEntry: Wrong number of items");
            }
            return new SoundEntry(SampleSet.Load(items[0]),eid);
        }
    }
}
