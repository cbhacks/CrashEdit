using System;

namespace Crash.Audio
{
    [EntryType(20)]
    public sealed class SpeechEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 1)
            {
                ErrorManager.SignalError("SpeechEntry: Wrong number of items");
            }
            return new SpeechEntry(SampleSet.Load(items[0]),eid);
        }
    }
}
