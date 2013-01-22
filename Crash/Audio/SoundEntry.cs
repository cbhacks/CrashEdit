using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SoundEntry : Entry
    {
        private SampleSet samples;

        public SoundEntry(SampleSet samples,int unknown) : base(unknown)
        {
            if (samples == null)
                throw new ArgumentNullException("Samples cannot be null.");
            this.samples = samples;
        }

        public override int Type
        {
            get { return 12; }
        }

        public SampleSet Samples
        {
            get { return samples; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [1][];
            items[0] = samples.Save();
            return Save(items,8);
        }
    }
}
