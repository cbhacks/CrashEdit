using System;

namespace Crash
{
    [EntryType(11)]
    public sealed class T11EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 3)
            {
                ErrorManager.SignalError("T11Entry: Wrong number of items");
            }
            if (items[0].Length != 24)
            {
                ErrorManager.SignalError("T11Entry: First item length is wrong");
            }
            return new T11Entry(items,eid);
        }
    }
}
