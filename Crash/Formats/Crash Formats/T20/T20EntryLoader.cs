using System;

namespace Crash
{
    [EntryType(20,GameVersion.Crash1BetaMAR08)]
    [EntryType(20,GameVersion.Crash1BetaMAY11)]
    public sealed class T20EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T20Entry(items,eid);
        }
    }
}
