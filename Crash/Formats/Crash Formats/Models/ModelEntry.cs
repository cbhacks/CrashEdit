using System.Collections.Generic;

namespace Crash
{
    public sealed class
        ModelEntry : MysteryMultiItemEntry
    {
        public ModelEntry(IEnumerable<byte[]> items, int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 2; }
        }
    }
}
