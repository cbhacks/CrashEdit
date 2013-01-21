using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class WavebankEntry : Entry
    {
        private int id;
        private SampleSet samples;

        public WavebankEntry(int id,SampleSet samples,int unknown) : base(unknown)
        {
            if (samples == null)
                throw new ArgumentNullException("Samples cannot be null.");
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

        public override byte[] Save()
        {
            byte[] info = new byte [8];
            byte[] data = samples.Save();
            BitConv.ToWord(info,0,id);
            BitConv.ToWord(info,4,data.Length);
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = data;
            return Save(items);
        }
    }
}
