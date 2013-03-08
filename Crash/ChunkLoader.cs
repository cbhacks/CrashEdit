namespace Crash
{
    public abstract class ChunkLoader
    {
        public abstract Chunk Load(int chunkid,byte[] data);
    }
}
