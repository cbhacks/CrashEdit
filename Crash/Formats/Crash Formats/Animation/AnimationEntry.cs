namespace CrashEdit.Crash
{
    public sealed class AnimationEntry : Entry
    {
        public AnimationEntry(IEnumerable<Frame> frames, bool isnew, int eid) : base(eid)
        {
            Frames.AddRange(frames);
            IsNew = isnew;
        }

        public override string Title => $"Animation ({EName})";
        public override string ImageKey => "ThingLime";

        public override int Type => 1;

        [SubresourceList]
        public List<Frame> Frames { get; } = new List<Frame>();

        public bool IsNew { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[Frames.Count][];
            for (int i = 0; i < Frames.Count; i++)
            {
                items[i] = Frames[i].Save();
            }
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
