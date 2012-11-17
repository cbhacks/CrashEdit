using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class WavebankEntry : Entry
    {
        private int id;
        private List<SampleLine> samplelines;

        public WavebankEntry(int id,IEnumerable<SampleLine> samplelines,int unknown) : base(unknown)
        {
            this.id = id;
            this.samplelines = new List<SampleLine>(samplelines);
        }

        public override int Type
        {
            get { return 14; }
        }

        public int ID
        {
            get { return id; }
        }

        public IList<SampleLine> SampleLines
        {
            get { return samplelines; }
        }

        public override byte[] Save()
        {
            byte[] info = new byte [8];
            byte[] data = new byte [(samplelines.Count + 1) * 16];
            BitConv.ToWord(info,0,id);
            BitConv.ToWord(info,4,data.Length);
            for (int i = 0;i < samplelines.Count;i++)
            {
                samplelines[i].Save().CopyTo(data,(i + 1) * 16);
            }
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = data;
            return Save(items);
        }
    }
}
