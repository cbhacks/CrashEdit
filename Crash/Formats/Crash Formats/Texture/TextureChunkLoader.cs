using System;

namespace Crash
{
    [ChunkType(1)]
    public sealed class TextureChunkLoader : ChunkLoader
    {
        public override Chunk Load(int chunkid, byte[] data, NSF nsf)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            return new TextureChunk(data, nsf);
        }
    }
}
