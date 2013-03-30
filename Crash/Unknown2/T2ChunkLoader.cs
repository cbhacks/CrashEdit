using System;

namespace Crash.Unknown2
{
    [ChunkType(2)]
    public sealed class T2ChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new T2Chunk(entries);
        }
    }
}
