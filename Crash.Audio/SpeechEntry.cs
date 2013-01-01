namespace Crash.Audio
{
    public sealed class SpeechEntry : Entry
    {
        private SampleSet samples;

        public SpeechEntry(SampleSet samples,int unknown) : base(unknown)
        {
            if (samples == null)
                throw new System.ArgumentNullException("Samples cannot be null.");
            this.samples = samples;
        }

        public override int Type
        {
            get { return 20; }
        }

        public SampleSet Samples
        {
            get { return samples; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [1][];
            items[0] = samples.Save();;
            return Save(items);
        }
    }
}
