using System;

namespace Crash
{
    public sealed class WavebankEntry : Entry
    {
        private int id;
        private SampleSet samples;

        public WavebankEntry(int id,SampleSet samples,int eid,int size) : base(eid,size)
        {
            if (samples == null)
                throw new ArgumentNullException("samples");
            this.id = id;
            this.samples = samples;
        }

        public override int Type
        {
            get { return 14; }
        }

        public int ID
        {
            get { return id; }
        }

        public SampleSet Samples
        {
            get { return samples; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[] info = new byte [8];
            byte[] data = samples.Save();
            BitConv.ToInt32(info,0,id);
            BitConv.ToInt32(info,4,data.Length);
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = data;
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
