using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class T4Entry : Entry
    {
        //private T4Item0 t4itemfirst;
        private List<T4Item> t4items;
        //private T4Item0 t4itemlast;

        public T4Entry(IEnumerable<T4Item> t4items,int eid) : base(eid)
        {
            if (t4items == null)
                throw new ArgumentNullException("t4items");
            //this.t4itemfirst = t4itemfirst;
            this.t4items = new List<T4Item>(t4items);
            //this.t4itemlast = t4itemlast;
        }

        public override int Type
        {
            get { return 4; }
        }

        public IList<T4Item> T4Items
        {
            get { return t4items; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [t4items.Count][];
            //items[0] = t4itemfirst.Save();
            for (int i = 0;i < t4items.Count;i++)
            {
                items[i] = t4items[i].Save();
            }
            //items[t4items.Count + 1] = t4itemlast.Save();
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
