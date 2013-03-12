using System;
using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T21Entry : MysteryMultiItemEntry
    {
        public T21Entry(IEnumerable<byte[]> items,int unknown) : base(items,unknown)
        {
        }

        public override int Type
        {
            get { return 21; }
        }
    }
}
