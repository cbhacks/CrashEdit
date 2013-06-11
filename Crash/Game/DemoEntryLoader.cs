using System;

namespace Crash.Game
{
    [EntryType(19)]
    public sealed class DemoEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items.Length != 1)
            {
                ErrorManager.SignalError("DemoEntry: Wrong number of items");
            }
            return new DemoEntry(items[0],eid);
        }
    }
}
