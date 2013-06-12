using System;

namespace Crash
{
    [EntryType(21)]
    public sealed class T21EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T21Entry(items,eid);
        }
    }
}
