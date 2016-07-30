using System.Collections.Generic;

namespace Crash
{
    public sealed class T17Entry : MysteryMultiItemEntry
    {
        public T17Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 17; }
        }
    }
}
