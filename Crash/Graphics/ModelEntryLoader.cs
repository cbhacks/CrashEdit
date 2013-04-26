using System;

namespace Crash.Graphics
{
    [EntryType(2)]
    public sealed class ModelEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 5 && items.Length != 6)
            {
                throw new LoadException();
            }
            if (items[0].Length != 80)
            {
                throw new LoadException();
            }
            return new ModelEntry(items,eid);
        }
    }
}
