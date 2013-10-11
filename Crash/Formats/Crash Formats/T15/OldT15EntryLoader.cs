using System;

namespace Crash
{
    [EntryType(15,GameVersion.Crash1BetaMAR08)]
    [EntryType(15,GameVersion.Crash1BetaMAY11)]
    [EntryType(15,GameVersion.Crash1)]
    public sealed class OldT15EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new OldT15Entry(items,eid);
        }
    }
}
