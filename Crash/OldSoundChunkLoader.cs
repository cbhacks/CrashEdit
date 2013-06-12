using System;

namespace Crash
{
    [ChunkType(2)]
    public sealed class OldSoundChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new OldSoundChunk(entries);
        }
    }
}
