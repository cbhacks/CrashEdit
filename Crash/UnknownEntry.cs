using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class UnknownEntry : MysteryMultiItemEntry
    {
        private int type;

        public UnknownEntry(IEnumerable<byte[]> items,int eid,int type) : base(items,eid)
        {
            this.type = type;
        }

        public override int Type
        {
            get { return type; }
        }
    }
}
