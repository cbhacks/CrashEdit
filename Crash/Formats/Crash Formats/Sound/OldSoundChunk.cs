using System.Collections.Generic;

namespace Crash
{
    public sealed class OldSoundChunk : EntryChunk
    {
        public OldSoundChunk(NSF nsf) : base(nsf)
        {
        }

        public OldSoundChunk(IEnumerable<Entry> entries, NSF nsf) : base(entries, nsf)
        {
        }

        public override short Type => 2;
        public override int Alignment => 16;
    }
}
