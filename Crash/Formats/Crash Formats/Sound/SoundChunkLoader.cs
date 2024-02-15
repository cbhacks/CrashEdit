using System;

namespace Crash
{
    [ChunkType(3)]
    public sealed class SoundChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries, NSF nsf)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new SoundChunk(entries, nsf);
        }
    }
}
