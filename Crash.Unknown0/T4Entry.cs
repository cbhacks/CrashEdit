using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T4Entry : Entry,IMysteryMultiItemEntry
    {
        private List<byte[]> items;

        public T4Entry(IEnumerable<byte[]> items,int unknown) : base(unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            this.items = new List<byte[]>(items);
        }

        public override int Type
        {
            get { return 4; }
        }

        public IList<byte[]> Items
        {
            get { return items; }
        }

        public override byte[] Save()
        {
            return Save(items);
        }
    }
}
