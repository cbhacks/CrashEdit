using System;

namespace Crash
{
    [ChunkType(5)]
    public sealed class SpeechChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries, NSF nsf)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new SpeechChunk(entries, nsf);
        }
    }
}
