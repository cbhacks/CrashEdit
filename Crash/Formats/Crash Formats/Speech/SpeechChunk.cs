using System.Collections.Generic;

namespace Crash
{
    public sealed class SpeechChunk : EntryChunk
    {
        public SpeechChunk()
        {
        }

        public SpeechChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override short Type => 5;
        public override int Alignment => 16;
    }
}
