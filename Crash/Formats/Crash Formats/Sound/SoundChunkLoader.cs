namespace CrashEdit.Crash
{
    [ChunkType(3)]
    public sealed class SoundChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            ArgumentNullException.ThrowIfNull(entries);
            return new SoundChunk(entries);
        }
    }
}
