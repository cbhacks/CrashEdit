namespace Crash
{
    public sealed class SoundChunk : EntryChunk
    {
        public SoundChunk(NSF nsf) : base(nsf)
        {
        }

        public SoundChunk(IEnumerable<Entry> entries, NSF nsf) : base(entries, nsf)
        {
        }

        public override short Type => 3;
        public override int Alignment => 16;
    }
}
