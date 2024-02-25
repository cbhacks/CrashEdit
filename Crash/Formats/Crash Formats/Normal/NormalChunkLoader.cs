namespace CrashEdit.Crash
{
    [ChunkType(0)]
    public sealed class NormalChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new NormalChunk(entries);
        }
    }
}
