using System;

namespace Crash.Unknown0
{
    [EntryType(2)]
    public sealed class T2EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("Items cannot be null.");
            if (items.Length < 5)
            {
                throw new Exception();
            }
            if (items[0].Length != 80)
            {
                throw new Exception();
            }
            return new T2Entry(items,unknown);
        }
    }
}
