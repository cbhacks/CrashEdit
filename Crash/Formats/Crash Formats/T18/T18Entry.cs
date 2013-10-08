using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class T18Entry : MysteryMultiItemEntry
    {
        public T18Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 18; }
        }
    }
}
