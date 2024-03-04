namespace CrashEdit.Crash
{
    [ChunkType(5)]
    public sealed class SpeechChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            ArgumentNullException.ThrowIfNull(entries);
            return new SpeechChunk(entries);
        }
    }
}
