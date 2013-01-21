using System;

namespace Crash
{
    [ChunkType(0)]
    public sealed class NormalChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("Entries cannot be null.");
            return new NormalChunk(entries,unknown1,unknown2);
        }
    }
}
