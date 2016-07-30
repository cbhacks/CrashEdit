using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class MysteryMultiItemEntry : Entry
    {
        private List<byte[]> items;

        public MysteryMultiItemEntry(IEnumerable<byte[]> items,int eid, int size) : base(eid, size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            this.items = new List<byte[]>(items);
        }

        public IList<byte[]> Items
        {
            get { return items; }
        }

        public override UnprocessedEntry Unprocess()
        {
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
