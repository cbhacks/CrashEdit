namespace Crash.Graphics
{
    [ChunkType(1)]
    public sealed class TextureChunkLoader : ChunkLoader
    {
        public override Chunk Load(byte[] data)
        {
            return new TextureChunk(data);
        }
    }
}
