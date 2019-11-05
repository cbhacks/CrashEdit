using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoAnimationEntry : Entry
    {
        private List<ProtoFrame> frames;

        public ProtoAnimationEntry(IEnumerable<ProtoFrame> frames,bool notproto,int eid, int size) : base(eid, size)
        {
            this.frames = new List<ProtoFrame>(frames);
            NotProto = notproto;
        }

        public override int Type => 1;
        public IList<ProtoFrame> Frames => frames;

        public bool NotProto { get; }

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
