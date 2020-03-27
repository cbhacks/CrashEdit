using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoAnimationEntry : Entry
    {
        private List<OldFrame> frames;

        public ProtoAnimationEntry(IEnumerable<OldFrame> frames,bool notproto,int eid) : base(eid)
        {
            this.frames = new List<OldFrame>(frames);
            NotProto = notproto;
        }

        public override int Type => 1;
        public IList<OldFrame> Frames => frames;

        public bool NotProto { get; }

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
