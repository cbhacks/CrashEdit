using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash1BetaMAR08)]
    [EntryType(3,GameVersion.Crash1BetaMAY11)]
    [EntryType(3,GameVersion.Crash1)]
    [EntryType(3,GameVersion.Crash2)]
    [EntryType(3,GameVersion.Crash3)]
    public sealed class T3EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 7)
            {
                ErrorManager.SignalError("T3Entry: Wrong number of items");
            }
            if (items[0].Length != 76)
            {
                ErrorManager.SignalError("T3Entry: First item length is wrong");
            }
            return new T3Entry(items,eid);
        }
    }
}
