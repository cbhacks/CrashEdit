using System.Collections.Generic;

namespace Crash
{
    public sealed class T2Entry : MysteryMultiItemEntry
    {
        public T2Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type => 2;
    }
}
