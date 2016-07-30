using System;

namespace Crash
{
    //[EntryType(2, GameVersion.Crash2)]
    [EntryType(2, GameVersion.Crash3)]
    public sealed class T2EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("ModelEntry: Wrong number of items");
            }
            if (items[0].Length != 80)
            {
                ErrorManager.SignalError("ModelEntry: First item length is wrong");
            }
            return new T2Entry(items,eid,size);
        }
    }
}
