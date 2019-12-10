using System;

namespace Crash
{
    public sealed class MusicEntry : Entry
    {
        public MusicEntry(int vheid,int vb0eid,int vb1eid,int vb2eid,int vb3eid,int vb4eid,int vb5eid,int vb6eid,VH vh,SEP sep,int eid) : base(eid)
        {
            SEP = sep ?? throw new ArgumentNullException("sep");
            VHEID = vheid;
            VB0EID = vb0eid;
            VB1EID = vb1eid;
            VB2EID = vb2eid;
            VB3EID = vb3eid;
            VB4EID = vb4eid;
            VB5EID = vb5eid;
            VB6EID = vb6eid;
            VH = vh;
        }

        public override int Type => 13;
        public int VHEID { get; }
        public int VB0EID { get; }
        public int VB1EID { get; }
        public int VB2EID { get; }
        public int VB3EID { get; }
        public int VB4EID { get; }
        public int VB5EID { get; }
        public int VB6EID { get; }
        public VH VH { get; set; }
        public SEP SEP { get; }

        // FIXME? - resaving of unused instrument metadata causes mismatches in
        // various game versions
        public override bool IgnoreResaveErrors => true;

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [3][];
            items[0] = new byte [36];
            BitConv.ToInt32(items[0],0,SEP.SEQs.Count);
            BitConv.ToInt32(items[0],4,VHEID);
            BitConv.ToInt32(items[0],8,VB0EID);
            BitConv.ToInt32(items[0],12,VB1EID);
            BitConv.ToInt32(items[0],16,VB2EID);
            BitConv.ToInt32(items[0],20,VB3EID);
            BitConv.ToInt32(items[0],24,VB4EID);
            BitConv.ToInt32(items[0],28,VB5EID);
            BitConv.ToInt32(items[0],32,VB6EID);
            if (VH != null)
            {
                items[1] = VH.Save();
            }
            else
            {
                items[1] = new byte [0];
            }
            items[2] = SEP.Save();
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
