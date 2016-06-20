using System.Collections.Generic;

namespace Crash
{
    public sealed class NewModelEntry : MysteryMultiItemEntry
    {
        public NewModelEntry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 2; }
        }
    }
}
