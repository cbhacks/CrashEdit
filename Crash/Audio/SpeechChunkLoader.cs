using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    [ChunkType(5)]
    public sealed class SpeechChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new SpeechChunk(entries,unknown2);
        }
    }
}
