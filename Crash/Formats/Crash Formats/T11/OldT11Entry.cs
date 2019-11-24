using System.Collections.Generic;

namespace Crash
{
    public sealed class OldT11Entry : MysteryMultiItemEntry
    {
        public OldT11Entry(IEnumerable<byte[]> items,int eid,int size) : base(items,eid,size)
        {
        }

        public override int Type => 11;

        public int Format => BitConv.FromInt32(Items[0],8);
    }
}
