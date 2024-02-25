namespace CrashEdit.Crash
{
    [ChunkType(3)]
    public sealed class SoundChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new SoundChunk(entries);
        }
    }
}
