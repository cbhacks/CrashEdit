namespace CrashEdit.Crash
{
    [ChunkType(4)]
    public sealed class WavebankChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new WavebankChunk(entries);
        }
    }
}
