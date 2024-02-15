using System;

namespace Crash
{
    [ChunkType(2)]
    public sealed class OldSoundChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries, NSF nsf)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            return new OldSoundChunk(entries, nsf);
        }
    }
}
