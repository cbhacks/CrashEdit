using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldEntityEntry : MysteryMultiItemEntry
    {
        public OldEntityEntry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 7; }
        }
    }
}
