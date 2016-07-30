using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SLSTEntry : Entry
    {
        //private SLSTItem0 slstitemfirst;
        private List<SLSTItem> slstitems;
        //private SLSTItem0 slstitemlast;

        public SLSTEntry(IEnumerable<SLSTItem> slstitems,int eid,int size) : base(eid,size)
        {
            if (slstitems == null)
                throw new ArgumentNullException("slstitems");
            //this.slstitemfirst = slstitemfirst;
            this.slstitems = new List<SLSTItem>(slstitems);
            //this.slstitemlast = slstitemlast;
        }

        public override int Type
        {
            get { return 4; }
        }

        public IList<SLSTItem> SLSTItems
        {
            get { return slstitems; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [slstitems.Count][];
            //items[0] = slstitemfirst.Save();
            for (int i = 0;i < slstitems.Count;i++)
            {
                items[i] = slstitems[i].Save();
            }
            //items[slstitems.Count + 1] = slstitemlast.Save();
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
