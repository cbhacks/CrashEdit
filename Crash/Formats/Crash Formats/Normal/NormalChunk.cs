using System.Collections.Generic;

namespace Crash
{
    public sealed class NormalChunk : EntryChunk
    {
        public NormalChunk()
        {
        }

        public NormalChunk(IEnumerable<Entry> entries) : base(entries)
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
    }
}
