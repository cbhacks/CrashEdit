using System;

namespace Crash.Unknown0
{
    [EntryType(1)]
    public sealed class T1EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new T1Entry(items,unknown);
        }
    }
}
