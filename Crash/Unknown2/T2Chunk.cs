using System.Collections.Generic;

namespace Crash.Unknown2
{
    public sealed class T2Chunk : EntryChunk
    {
        public T2Chunk()
        {
        }

        public T2Chunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override short Type
        {
            get { return 2; }
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
