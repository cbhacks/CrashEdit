using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash2)]
    [EntryType(3,GameVersion.Crash3)]
    public sealed class SceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 7)
            {
                ErrorManager.SignalError("SceneryEntry: Wrong number of items");
            }
            if (items[0].Length != 76)
            {
                ErrorManager.SignalError("SceneryEntry: First item length is wrong");
            }
            return new SceneryEntry(items,eid);
        }
    }
}
