using System.Collections.Generic;

namespace Crash
{
    public sealed class T2Entry : MysteryMultiItemEntry
    {
        public T2Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 2; }
        }
    }
}
