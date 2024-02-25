using System.Collections.Generic;

namespace CrashEdit.Crash
{
    public sealed class OldAnimationEntry : Entry
    {
        public OldAnimationEntry(IEnumerable<OldFrame> frames,int eid) : base(eid)
        {
            Frames.AddRange(frames);
        }

        public override string Title => $"Old Animation ({EName})";
        public override string ImageKey => "ThingLime";

        public override int Type => 1;

        [SubresourceList]
        public List<OldFrame> Frames { get; } = new List<OldFrame>();

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [Frames.Count][];
            for (int i = 0;i < Frames.Count;i++)
            {
                items[i] = Frames[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
