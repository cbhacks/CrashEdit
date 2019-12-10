using System.Collections.Generic;

namespace Crash
{
    public sealed class T21Entry : MysteryMultiItemEntry
    {
        public T21Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type => 21;
    }
}
