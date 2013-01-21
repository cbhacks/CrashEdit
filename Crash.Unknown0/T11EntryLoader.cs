using System;

namespace Crash.Unknown0
{
    [EntryType(11)]
    public sealed class T11EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("Items cannot be null.");
            if (items.Length < 3)
            {
                throw new Exception();
            }
            if (items[0].Length != 24)
            {
                throw new Exception();
            }
            return new T11Entry(items,unknown);
        }
    }
}
