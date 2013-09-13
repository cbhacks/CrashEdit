using System;

namespace Crash
{
    [EntryType(2,GameVersion.Crash1BetaMAR08)]
    [EntryType(2,GameVersion.Crash1BetaMAY11)]
    [EntryType(2,GameVersion.Crash1)]
    public sealed class OldModelEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 2)
            {
                ErrorManager.SignalError("OldModelEntry: Wrong number of items");
            }
            return new OldModelEntry(items,eid);
        }
    }
}
