using System;

namespace Crash.Game
{
    [EntryType(19)]
    public sealed class DemoEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new LoadException();
            }
            return new DemoEntry(items[0],unknown);
        }
    }
}
