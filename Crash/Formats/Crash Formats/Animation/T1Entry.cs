using System.Collections.Generic;

namespace Crash
{
    public sealed class T1Entry : MysteryMultiItemEntry
    {
        public T1Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 1; }
        }
    }
}
