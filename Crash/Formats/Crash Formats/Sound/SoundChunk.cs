namespace CrashEdit.Crash
{
    public sealed class SoundChunk : EntryChunk
    {
        public SoundChunk()
        {
        }

        public SoundChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override string Title => $"Sound Chunk {ChunkId}";
        public override string ImageKey => "JournalBlue";

        public override short Type => 3;
        public override int Alignment => 16;
    }
}
