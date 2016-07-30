using System.Collections.Generic;

namespace Crash
{
    public sealed class T11Entry : MysteryMultiItemEntry
    {
        public T11Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type
        {
            get { return 11; }
        }
    }
}
