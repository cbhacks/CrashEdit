namespace Crash
{
    [ChunkType(0)]
    public sealed class NormalChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            return new NormalChunk(entries,unknown1,unknown2);
        }
    }
}
