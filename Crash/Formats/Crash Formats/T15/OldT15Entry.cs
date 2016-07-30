using System.Collections.Generic;

namespace Crash
{
    public sealed class OldT15Entry : MysteryMultiItemEntry
    {
        public OldT15Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 15; }
        }
    }
}
