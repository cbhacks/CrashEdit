using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldModelEntry : MysteryMultiItemEntry
    {
        public OldModelEntry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 2; }
        }
    }
}
