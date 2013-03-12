using System;
using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T1Entry : MysteryMultiItemEntry
    {
        public T1Entry(IEnumerable<byte[]> items,int unknown) : base(items,unknown)
        {
        }

        public override int Type
        {
            get { return 1; }
        }
    }
}
