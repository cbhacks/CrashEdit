using System.Collections.Generic;

namespace Crash
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
    }
}
