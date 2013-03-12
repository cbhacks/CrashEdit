using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class MysteryMultiItemEntry : Entry
    {
        private List<byte[]> items;

        public MysteryMultiItemEntry(IEnumerable<byte[]> items,int unknown) : base(unknown)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            this.items = new List<byte[]>(items);
        }

        public IList<byte[]> Items
        {
            get { return items; }
        }

        public sealed override byte[] Save()
        {
            return Save(items);
        }
    }
}
