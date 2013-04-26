using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class OldSoundChunk : EntryChunk
    {
        public OldSoundChunk()
        {
        }

        public OldSoundChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override short Type
        {
            get { return 2; }
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
