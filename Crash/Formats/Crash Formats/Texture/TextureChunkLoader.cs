namespace CrashEdit.Crash
{
    [ChunkType(1)]
    public sealed class TextureChunkLoader : ChunkLoader
    {
        public override Chunk Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            return new TextureChunk(data);
        }
    }
}
