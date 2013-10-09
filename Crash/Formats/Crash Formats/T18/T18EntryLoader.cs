using System;

namespace Crash
{
    [EntryType(18,GameVersion.Crash1BetaMAR08)]
    public sealed class T18EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T18Entry(items,eid);
        }
    }
}
