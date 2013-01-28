using System;

namespace Crash.Unknown0
{
    [EntryType(3)]
    public sealed class T3EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 7)
            {
                throw new LoadException();
            }
            if (items[0].Length != 76)
            {
                throw new LoadException();
            }
            return new T3Entry(items,unknown);
        }
    }
}
