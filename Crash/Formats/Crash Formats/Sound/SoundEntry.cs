using System;

namespace Crash
{
    public sealed class SoundEntry : Entry
    {
        public SoundEntry(SampleSet samples,int eid) : base(eid)
        {
            Samples = samples ?? throw new ArgumentNullException("samples");
        }

        public override int Type => 12;
        public SampleSet Samples { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [1][];
            items[0] = Samples.Save();
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
