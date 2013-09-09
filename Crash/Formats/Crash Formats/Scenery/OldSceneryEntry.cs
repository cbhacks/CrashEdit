using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldSceneryEntry : MysteryMultiItemEntry
    {
        public OldSceneryEntry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 3; }
        }
    }
}
