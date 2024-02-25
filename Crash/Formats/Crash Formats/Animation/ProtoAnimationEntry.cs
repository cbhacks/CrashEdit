using System.Collections.Generic;

namespace CrashEdit.Crash
{
    public sealed class ProtoAnimationEntry : Entry
    {
        public ProtoAnimationEntry(IEnumerable<OldFrame> frames,bool notproto,int eid) : base(eid)
        {
            Frames.AddRange(frames);
            NotProto = notproto;
        }

        public override string Title =>
            NotProto ?
            $"Old Animation ({EName})" :
            $"Prototype Animation ({EName})";

        public override string ImageKey => "ThingLime";

        public override int Type => 1;
        public bool NotProto { get; }

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
