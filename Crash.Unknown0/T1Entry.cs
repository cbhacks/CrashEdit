using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T1Entry : Entry,IMysteryMultiItemEntry
    {
        private List<byte[]> items;

        public T1Entry(IEnumerable<byte[]> items,int unknown) : base(unknown)
        {
            this.items = new List<byte[]>(items);
        }

        public override int Type
        {
            get { return 1; }
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
