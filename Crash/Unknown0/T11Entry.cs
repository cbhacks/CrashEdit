using System;
using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T11Entry : MysteryMultiItemEntry
    {
        public T11Entry(IEnumerable<byte[]> items,int eid) : base(items,eid)
        {
        }

        public override int Type
        {
            get { return 11; }
        }
    }
}
