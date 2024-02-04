using System.Collections.Generic;

namespace Crash
{
    public sealed class WavebankChunk : EntryChunk
    {
        public WavebankChunk(NSF nsf) : base(nsf)
        {
        }

        public WavebankChunk(IEnumerable<Entry> entries, NSF nsf) : base(entries, nsf)
        {
        }

        public override short Type => 4;
        public override int Alignment => 16;
    }
}
