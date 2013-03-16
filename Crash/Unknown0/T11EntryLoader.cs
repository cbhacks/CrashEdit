using System;

namespace Crash.Unknown0
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
                throw new LoadException();
            }
            if (items[0].Length != 24)
            {
                throw new LoadException();
            }
            return new T11Entry(items,eid);
        }
    }
}
