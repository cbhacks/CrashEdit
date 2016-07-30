using System;

namespace Crash
{
    public sealed class MusicEntry : Entry
    {
        private int vheid;
        private int vb0eid;
        private int vb1eid;
        private int vb2eid;
        private int vb3eid;
        private int vb4eid;
        private int vb5eid;
        private int vb6eid;
        private VH vh;
        private SEP sep;

        public MusicEntry(int vheid,int vb0eid,int vb1eid,int vb2eid,int vb3eid,int vb4eid,int vb5eid,int vb6eid,VH vh,SEP sep,int eid, int size) : base(eid, size)
        {
            if (sep == null)
                throw new ArgumentNullException("sep");
            this.vheid = vheid;
            this.vb0eid = vb0eid;
            this.vb1eid = vb1eid;
            this.vb2eid = vb2eid;
            this.vb3eid = vb3eid;
            this.vb4eid = vb4eid;
            this.vb5eid = vb5eid;
            this.vb6eid = vb6eid;
            this.vh = vh;
            this.sep = sep;
        }

        public override int Type
        {
            get { return 13; }
        }

        public int VHEID
        {
            get { return vheid; }
        }

        public int VB0EID
        {
            get { return vb0eid; }
        }

        public int VB1EID
        {
            get { return vb1eid; }
        }

        public int VB2EID
        {
            get { return vb2eid; }
        }

        public int VB3EID
        {
            get { return vb3eid; }
        }

        public int VB4EID
        {
            get { return vb4eid; }
        }

        public int VB5EID
        {
            get { return vb5eid; }
        }

        public int VB6EID
        {
            get { return vb6eid; }
        }

        public VH VH
        {
            get { return vh; }
            set { vh = value; }
        }

        public SEP SEP
        {
            get { return sep; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [3][];
            items[0] = new byte [36];
            BitConv.ToInt32(items[0],0,SEP.SEQs.Count);
            BitConv.ToInt32(items[0],4,vheid);
            BitConv.ToInt32(items[0],8,vb0eid);
            BitConv.ToInt32(items[0],12,vb1eid);
            BitConv.ToInt32(items[0],16,vb2eid);
            BitConv.ToInt32(items[0],20,vb3eid);
            BitConv.ToInt32(items[0],24,vb4eid);
            BitConv.ToInt32(items[0],28,vb5eid);
            BitConv.ToInt32(items[0],32,vb6eid);
            if (vh != null)
            {
                items[1] = vh.Save();
            }
            else
            {
                items[1] = new byte [0];
            }
            items[2] = sep.Save();
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
