using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SpeechChunk : EntryChunk
    {
        public SpeechChunk()
        {
        }

        public SpeechChunk(IEnumerable<Entry> entries,int unknown2) : base(entries,unknown2)
        {
        }

        public override short Type
        {
            get { return 5; }
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
