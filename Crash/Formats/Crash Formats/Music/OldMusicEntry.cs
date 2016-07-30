using System;

namespace Crash
{
    public sealed class OldMusicEntry : Entry
    {
        private int vb0eid;
        private int vb1eid;
        private int vb2eid;
        private int vb3eid;
        private VH vh;
        private SEP sep;

        public OldMusicEntry(int vb0eid,int vb1eid,int vb2eid,int vb3eid,VH vh,SEP sep,int eid, int size) : base(eid, size)
        {
            if (vh == null)
                throw new ArgumentNullException("vh");
            if (sep == null)
                throw new ArgumentNullException("sep");
            this.vb0eid = vb0eid;
            this.vb1eid = vb1eid;
            this.vb2eid = vb2eid;
            this.vb3eid = vb3eid;
            this.vh = vh;
            this.sep = sep;
        }

        public override int Type
        {
            get { return 13; }
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

        public VH VH
        {
            get { return vh; }
            set
            {
                if (vh == null)
                    throw new ArgumentNullException("vh");
                vh = value;
            }
        }

        public SEP SEP
        {
            get { return sep; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [3][];
            items[0] = new byte [20];
            BitConv.ToInt32(items[0],0,SEP.SEQs.Count);
            BitConv.ToInt32(items[0],4,vb0eid);
            BitConv.ToInt32(items[0],8,vb1eid);
            BitConv.ToInt32(items[0],12,vb2eid);
            BitConv.ToInt32(items[0],16,vb3eid);
            items[1] = vh.Save();
            items[2] = sep.Save();
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
