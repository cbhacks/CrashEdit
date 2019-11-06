using System;

namespace Crash
{
    public sealed class SoundEntry : Entry
    {
        public SoundEntry(SampleSet samples,int eid,int size) : base(eid,size)
        {
            Samples = samples ?? throw new ArgumentNullException("samples");
        }

        public override int Type => 12;
        public SampleSet Samples { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [1][];
            items[0] = Samples.Save();
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
