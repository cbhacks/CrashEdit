namespace CrashEdit.Crash
{
    public sealed class NormalChunk : EntryChunk
    {
        public NormalChunk()
        {
        }

        public NormalChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override string Title => $"Chunk {ChunkId}";
        public override string ImageKey => "JournalOrange";

        public override short Type => 0;
        public override int Alignment => 4;
    }
}
