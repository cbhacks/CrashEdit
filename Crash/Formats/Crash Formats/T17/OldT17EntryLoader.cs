using System;

namespace Crash
{
    [EntryType(17,GameVersion.Crash1BetaMAR08)]
    [EntryType(17,GameVersion.Crash1BetaMAY11)]
    [EntryType(17,GameVersion.Crash1)]
    public sealed class OldT17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new OldT17Entry(items,eid,size);
        }
    }
}
