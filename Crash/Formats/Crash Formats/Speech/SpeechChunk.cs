namespace CrashEdit.Crash
{
    public sealed class SpeechChunk : EntryChunk
    {
        public SpeechChunk()
        {
        }

        public SpeechChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override string Title => $"Speech Chunk {ChunkId}";
        public override string ImageKey => "JournalWhite";

        public override short Type => 5;
        public override int Alignment => 16;
    }
}
