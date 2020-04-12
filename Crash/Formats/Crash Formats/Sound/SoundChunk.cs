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

        public override short Type => 3;
        public override int Alignment => 16;
    }
}
