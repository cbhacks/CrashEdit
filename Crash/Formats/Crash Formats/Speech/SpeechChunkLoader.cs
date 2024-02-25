namespace CrashEdit.Crash
{
    [ChunkType(5)]
    public sealed class SpeechChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new SpeechChunk(entries);
        }
    }
}
