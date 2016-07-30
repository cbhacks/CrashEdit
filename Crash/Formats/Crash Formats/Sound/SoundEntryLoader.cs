using System;

namespace Crash
{
    [EntryType(12,GameVersion.Crash1Beta1995)]
    [EntryType(12,GameVersion.Crash1BetaMAR08)]
    [EntryType(12,GameVersion.Crash1BetaMAY11)]
    [EntryType(12,GameVersion.Crash1)]
    [EntryType(12,GameVersion.Crash2)]
    [EntryType(12,GameVersion.Crash3)]
    public sealed class SoundEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 1)
            {
                ErrorManager.SignalError("SoundEntry: Wrong number of items");
            }
            return new SoundEntry(SampleSet.Load(items[0]),eid,size);
        }
    }
}
