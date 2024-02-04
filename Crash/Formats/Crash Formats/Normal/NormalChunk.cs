using System.Collections.Generic;

namespace Crash
{
    public sealed class NormalChunk : EntryChunk
    {
        public NormalChunk(NSF nsf) : base(nsf)
        {
        }

        public NormalChunk(IEnumerable<Entry> entries, NSF nsf) : base(entries, nsf)
        {
        }

        public override short Type => 0;
        public override int Alignment => 4;
    }
}
