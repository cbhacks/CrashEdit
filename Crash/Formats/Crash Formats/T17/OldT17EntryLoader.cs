using System;

namespace Crash
{
    [EntryType(17,GameVersion.Crash1BetaMAR08)]
    public sealed class OldT17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new OldT17Entry(items,eid);
        }
    }
}
