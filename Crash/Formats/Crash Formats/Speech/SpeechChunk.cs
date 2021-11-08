using System.Collections.Generic;

namespace Crash
{
    public sealed class SpeechChunk : EntryChunk
    {
        public SpeechChunk(NSF nsf) : base(nsf)
        {
        }

        public SpeechChunk(IEnumerable<Entry> entries, NSF nsf) : base(entries, nsf)
        {
        }

        public override short Type => 5;
        public override int Alignment => 16;
    }
}
