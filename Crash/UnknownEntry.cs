using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class UnknownEntry : Entry,IMysteryMultiItemEntry
    {
        private byte[][] items;
        private int type;

        public UnknownEntry(byte[][] items,int unknown,int type) : base(unknown)
        {
            if (items == null)
                throw new ArgumentNullException("Items cannot be null.");
            this.items = items;
            this.type = type;
        }

        public override int Type
        {
            get { return type; }
        }

        public byte[][] Items
        {
            get { return items; }
        }

        IList<byte[]> IMysteryMultiItemEntry.Items
        {
            get { return items; }
        }

        public override byte[] Save()
        {
            return Save(items);
        }
    }
}
