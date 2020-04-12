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

        public override short Type => 0;
        public override int Alignment => 4;
    }
}
