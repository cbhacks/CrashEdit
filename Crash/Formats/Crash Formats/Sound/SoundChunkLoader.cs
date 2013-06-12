using System;
using System.Collections.Generic;

namespace Crash
{
    [ChunkType(3)]
    public sealed class SoundChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new SoundChunk(entries);
        }
    }
}
