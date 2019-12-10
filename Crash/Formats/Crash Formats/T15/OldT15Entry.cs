using System.Collections.Generic;

namespace Crash
{
    public sealed class OldT15Entry : MysteryMultiItemEntry
    {
        public OldT15Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type => 15;
    }
}
