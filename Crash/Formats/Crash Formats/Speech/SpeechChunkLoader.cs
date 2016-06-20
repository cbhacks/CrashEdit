using System;

namespace Crash
{
    [ChunkType(5)]
    public sealed class SpeechChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new SpeechChunk(entries);
        }
    }
}
