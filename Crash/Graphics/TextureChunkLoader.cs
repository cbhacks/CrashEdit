using System;

namespace Crash.Graphics
{
    [ChunkType(1)]
    public sealed class TextureChunkLoader : ChunkLoader
    {
        public override Chunk Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null.");
            return new TextureChunk(data);
        }
    }
}
