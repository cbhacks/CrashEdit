using System.Collections.Generic;

namespace Crash
{
    public sealed class SoundChunk : EntryChunk
    {
        public SoundChunk()
        {
        }

        public SoundChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override short Type
        {
            get { return 3; }
        }

        protected override int Alignment
        {
            get { return 16; }
        }

        protected override int AlignmentOffset
        {
            get { return 8; }
        }
    }
}
