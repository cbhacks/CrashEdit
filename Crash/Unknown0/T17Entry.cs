using System;
using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T17Entry : MysteryMultiItemEntry
    {
        public T17Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 17; }
        }
    }
}
