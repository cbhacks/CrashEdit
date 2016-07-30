using System.Collections.Generic;

namespace Crash
{
    public sealed class T21Entry : MysteryMultiItemEntry
    {
        public T21Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 21; }
        }
    }
}
