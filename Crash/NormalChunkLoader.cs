using System;

namespace Crash
{
    [ChunkType(0)]
    public sealed class NormalChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new NormalChunk(entries,unknown2);
        }
    }
}
