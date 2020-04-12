using System.Collections.Generic;

namespace Crash
{
    public sealed class WavebankChunk : EntryChunk
    {
        public WavebankChunk()
        {
        }

        public WavebankChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override short Type => 4;
        public override int Alignment => 16;
    }
}
