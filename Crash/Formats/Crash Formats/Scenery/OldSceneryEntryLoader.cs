using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash1BetaMAR08)]
    [EntryType(3,GameVersion.Crash1BetaMAY11)]
    [EntryType(3,GameVersion.Crash1)]
    public sealed class OldSceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3)
            {
                ErrorManager.SignalError("OldSceneryEntry: Wrong number of items");
            }
            return new OldSceneryEntry(items,eid);
        }
    }
}
