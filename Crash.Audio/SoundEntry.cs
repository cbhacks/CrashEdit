using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SoundEntry : Entry
    {
        private List<SampleLine> samplelines;

        public SoundEntry(IEnumerable<SampleLine> samplelines,int unknown) : base(unknown)
        {
            this.samplelines = new List<SampleLine>(samplelines);
        }

        public override int Type
        {
            get { return 12; }
        }

        public IList<SampleLine> SampleLines
        {
            get { return samplelines; }
        }

        public override byte[] Save()
        {
            byte[] data = new byte [(samplelines.Count + 1) * 16];
            for (int i = 0;i < samplelines.Count;i++)
            {
                samplelines[i].Save().CopyTo(data,(i + 1) * 16);
            }
            byte[][] items = new byte [1][];
            items[0] = data;
            return Save(items,8);
        }
    }
}
