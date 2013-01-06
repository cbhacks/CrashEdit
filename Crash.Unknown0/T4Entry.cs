using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T4Entry : Entry
    {
        private List<T4Item> t4items;

        public T4Entry(IEnumerable<T4Item> t4items,int unknown) : base(unknown)
        {
            if (t4items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            this.t4items = new List<T4Item>(t4items);
        }

        public override int Type
        {
            get { return 4; }
        }

        public IList<T4Item> T4Items
        {
            get { return t4items; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [t4items.Count][];
            for (int i = 0;i < t4items.Count;i++)
            {
                items[i] = t4items[i].Save();
            }
            return Save(items);
        }
    }
}
