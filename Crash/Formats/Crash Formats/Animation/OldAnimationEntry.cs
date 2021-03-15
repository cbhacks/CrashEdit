using System.Collections.Generic;

namespace CrashEdit.Crash
{
    public sealed class OldAnimationEntry : Entry
    {
        private List<OldFrame> frames;

        public OldAnimationEntry(IEnumerable<OldFrame> frames,int eid) : base(eid)
        {
            this.frames = new List<OldFrame>(frames);
        }

        public override int Type => 1;
        public IList<OldFrame> Frames => frames;

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
