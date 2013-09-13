using System;

namespace Crash
{
    [EntryType(7,GameVersion.Crash1BetaMAR08)]
    [EntryType(7,GameVersion.Crash1BetaMAY11)]
    [EntryType(7,GameVersion.Crash1)]
    public sealed class OldEntityEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new OldEntityEntry(items,eid);
        }
    }
}
