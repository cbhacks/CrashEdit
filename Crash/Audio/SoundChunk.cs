using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SoundChunk : EntryChunk
    {
        public SoundChunk()
        {
        }

        public SoundChunk(IEnumerable<Entry> entries,int unknown2) : base(entries,unknown2)
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
