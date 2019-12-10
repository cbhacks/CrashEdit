using System;

namespace Crash
{
    public sealed class SpeechEntry : Entry
    {
        public SpeechEntry(SampleSet samples,int eid) : base(eid)
        {
            Samples = samples ?? throw new ArgumentNullException("samples");
        }

        public override int Type => 20;
        public SampleSet Samples { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [1][];
            items[0] = Samples.Save();
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
