using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class MysteryMultiItemEntry : Entry
    {
        private readonly List<byte[]> items;

        public MysteryMultiItemEntry(IEnumerable<byte[]> items, int eid) : base(eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            this.items = new List<byte[]>(items);
        }

        public IList<byte[]> Items => items;

        public override UnprocessedEntry Unprocess()
        {
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
