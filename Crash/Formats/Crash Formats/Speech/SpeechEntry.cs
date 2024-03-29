namespace CrashEdit.Crash
{
    public sealed class SpeechEntry : Entry
    {
        public SpeechEntry(SampleSet samples, int eid) : base(eid)
        {
            Samples = samples ?? throw new ArgumentNullException(nameof(samples));
        }

        public override string Title => $"Speech ({EName})";
        public override string ImageKey => "SpeakerWhite";

        public override int Type => 20;
        public SampleSet Samples { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[1][];
            items[0] = Samples.Save();
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
