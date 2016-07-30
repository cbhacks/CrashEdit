using System;

namespace Crash
{
    [EntryType(11, GameVersion.Crash1Beta1995)]
    [EntryType(11, GameVersion.Crash1BetaMAR08)]
    [EntryType(11, GameVersion.Crash1BetaMAY11)]
    [EntryType(11, GameVersion.Crash1)]
    [EntryType(11, GameVersion.Crash2)]
    [EntryType(11,GameVersion.Crash3)]
    public sealed class T11EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("T11Entry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("T11Entry: First item length is wrong");
            }
            return new T11Entry(items,eid,size);
        }
    }
}
