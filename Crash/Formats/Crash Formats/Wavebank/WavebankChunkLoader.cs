namespace Crash
{
    [ChunkType(4)]
    public sealed class WavebankChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries, NSF nsf)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new WavebankChunk(entries, nsf);
        }
    }
}
