using System;

namespace Crash
{
    public sealed class OldMusicEntry : Entry
    {
        private VH vh;

        public OldMusicEntry(int vb0eid,int vb1eid,int vb2eid,int vb3eid,VH vh,SEP sep,int eid) : base(eid)
        {
            this.vh = vh ?? throw new ArgumentNullException("vh");
            SEP = sep ?? throw new ArgumentNullException("sep");
            VB0EID = vb0eid;
            VB1EID = vb1eid;
            VB2EID = vb2eid;
            VB3EID = vb3eid;
        }

        public override int Type => 13;
        public int VB0EID { get; }
        public int VB1EID { get; }
        public int VB2EID { get; }
        public int VB3EID { get; }

        // FIXME? - resaving of unused instrument metadata causes mismatches in
        // various game versions
        public override bool IgnoreResaveErrors => true;

        public VH VH
        {
            get => vh;
            set
            {
                if (vh == null)
                    throw new ArgumentNullException("vh");
                vh = value;
            }
        }

        public SEP SEP { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [3][];
            items[0] = new byte [20];
            BitConv.ToInt32(items[0],0,SEP.SEQs.Count);
            BitConv.ToInt32(items[0],4,VB0EID);
            BitConv.ToInt32(items[0],8,VB1EID);
            BitConv.ToInt32(items[0],12,VB2EID);
            BitConv.ToInt32(items[0],16,VB3EID);
            items[1] = vh.Save();
            items[2] = SEP.Save();
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
