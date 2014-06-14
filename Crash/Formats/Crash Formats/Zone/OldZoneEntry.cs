using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldZoneEntry : MysteryMultiItemEntry
    {
        public OldZoneEntry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 7; }
        }
    }
}
