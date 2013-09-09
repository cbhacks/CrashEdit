using System;

namespace Crash
{
    [EntryType(1,GameVersion.Crash1BetaMAR08)]
    [EntryType(1,GameVersion.Crash1BetaMAY11)]
    [EntryType(1,GameVersion.Crash1)]
    [EntryType(1,GameVersion.Crash2)]
    [EntryType(1,GameVersion.Crash3)]
    public sealed class T1EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T1Entry(items,eid);
        }
    }
}
