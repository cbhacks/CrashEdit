using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class WeirdEntry : MysteryMultiItemEntry
    {
        private int type;
        private Exception exception;

        public WeirdEntry(IEnumerable<byte[]> items,int eid,int type,Exception exception) : base(items,eid)
        {
            this.type = type;
            this.exception = exception;
        }

        public override int Type
        {
            get { return type; }
        }

        public Exception Exception
        {
            get { return exception; }
        }
    }
}
