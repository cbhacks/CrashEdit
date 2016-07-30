using System;

namespace Crash
{
    public sealed class SoundEntry : Entry
    {
        private SampleSet samples;

        public SoundEntry(SampleSet samples,int eid,int size) : base(eid,size)
        {
            if (samples == null)
                throw new ArgumentNullException("samples");
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

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [1][];
            items[0] = samples.Save();
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
