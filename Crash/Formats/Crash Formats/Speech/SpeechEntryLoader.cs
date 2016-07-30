using System;

namespace Crash
{
    [EntryType(20,GameVersion.Crash2)]
    [EntryType(20,GameVersion.Crash3)]
    public sealed class SpeechEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 1)
            {
                ErrorManager.SignalError("SpeechEntry: Wrong number of items");
            }
            return new SpeechEntry(SampleSet.Load(items[0]),eid,size);
        }
    }
}
