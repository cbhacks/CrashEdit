using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldT17Entry : MysteryMultiItemEntry
    {
        public OldT17Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 17; }
        }
    }
}
