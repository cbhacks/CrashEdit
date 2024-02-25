namespace Crash
{
    [ChunkType(0)]
    public sealed class NormalChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries, NSF nsf)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new NormalChunk(entries, nsf);
        }
    }
}
