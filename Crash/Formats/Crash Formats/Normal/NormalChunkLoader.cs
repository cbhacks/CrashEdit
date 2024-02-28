namespace CrashEdit.Crash
{
    [ChunkType(0)]
    public sealed class NormalChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            ArgumentNullException.ThrowIfNull(entries);
            return new NormalChunk(entries);
        }
    }
}
