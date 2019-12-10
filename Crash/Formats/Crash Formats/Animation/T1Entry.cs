using System.Collections.Generic;

namespace Crash
{
    public sealed class T1Entry : MysteryMultiItemEntry
    {
        public T1Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type => 1;
    }
}
