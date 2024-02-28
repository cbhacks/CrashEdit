namespace CrashEdit.Crash
{
    [ChunkType(2)]
    public sealed class OldSoundChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            ArgumentNullException.ThrowIfNull(entries);
            return new OldSoundChunk(entries);
        }
    }
}
