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

        public override short Type
        {
            get { return 4; }
        }

        protected override int Alignment
        {
            get { return 16; }
        }

        protected override int AlignmentOffset
        {
            get { return 4; }
        }
    }
}
