using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoAnimationEntry : Entry
    {
        private List<ProtoFrame> frames;

        public ProtoAnimationEntry(IEnumerable<ProtoFrame> frames,int eid, int size) : base(eid, size)
        {
            this.frames = new List<ProtoFrame>(frames);
        }

        public override int Type
        {
            get { return 1; }
        }

        public IList<ProtoFrame> Frames
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
