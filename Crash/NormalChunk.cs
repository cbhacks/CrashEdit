using System.Collections.Generic;

namespace Crash
{
    public sealed class NormalChunk : EntryChunk
    {
        public NormalChunk(IEnumerable<Entry> entries,int unknown2) : base(entries,unknown2)
        {
        }

        public override short Type
        {
            get { return 0; }
        }

        protected override int Alignment
        {
            get { return 4; }
        }

        protected override int AlignmentOffset
        {
            get { return 0; }
        }
    }
}
