namespace CrashEdit.Crash
{
    [ChunkType(4)]
    public sealed class WavebankChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            ArgumentNullException.ThrowIfNull(entries);
            return new WavebankChunk(entries);
        }
    }
}
