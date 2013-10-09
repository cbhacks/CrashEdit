using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class T20Entry : MysteryMultiItemEntry
    {
        public T20Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 20; }
        }
    }
}
