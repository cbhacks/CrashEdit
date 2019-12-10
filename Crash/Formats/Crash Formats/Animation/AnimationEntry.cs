using System.Collections.Generic;

namespace Crash
{
    public sealed class AnimationEntry : Entry
    {
        private List<Frame> frames;

        public AnimationEntry(IEnumerable<Frame> frames,int eid) : base(eid)
        {
            this.frames = new List<Frame>(frames);
        }

        public override int Type => 1;
        public IList<Frame> Frames => frames;

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [frames.Count][];
            for (int i = 0;i < frames.Count;i++)
            {
                items[i] = frames[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
