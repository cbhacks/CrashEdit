using System;

namespace Crash
{
    [EntryType(11,GameVersion.Crash2)]
    [EntryType(11,GameVersion.Crash3)]
    public sealed class GOOLv2EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 5 && items.Length != 6)
            {
                ErrorManager.SignalError("GOOLv2Entry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("GOOLv2Entry: Header length is wrong");
            }
            return new GOOLEntry(GOOLVersion.Version2,items[0],items[1],items[2],
                items.Length >= 4 ? items[3] : null,
                items.Length >= 5 ? items[4] : null,
                items.Length >= 6 ? items[5] : null,
                eid,size);
        }
    }
}
