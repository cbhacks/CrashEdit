using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class WeirdEntry : MysteryMultiItemEntry
    {
        private int type;

        public WeirdEntry(IEnumerable<byte[]> items,int eid,int type) : base(items,eid)
        {
            this.type = type;
        }

        public override int Type
        {
            get { return type; }
        }
    }
}
