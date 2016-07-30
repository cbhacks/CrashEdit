using System.Collections.Generic;

namespace Crash
{
    public sealed class OldAnimationEntry : Entry
    {
        private List<OldFrame> frames;

        public OldAnimationEntry(IEnumerable<OldFrame> frames,int eid, int size) : base(eid, size)
        {
            this.frames = new List<OldFrame>(frames);
        }

        public override int Type
        {
            get { return 1; }
        }

        public IList<OldFrame> Frames
        {
            get { return frames; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [frames.Count][];
            for (int i = 0;i < frames.Count;i++)
            {
                items[i] = frames[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
