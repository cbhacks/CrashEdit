using System.Collections.Generic;

namespace Crash
{
    public sealed class T17Entry : MysteryMultiItemEntry
    {
        public T17Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type => 17;
    }
}
