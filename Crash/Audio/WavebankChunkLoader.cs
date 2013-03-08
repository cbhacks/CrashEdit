using System;

namespace Crash.Audio
{
    [ChunkType(4)]
    public sealed class WavebankChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new WavebankChunk(entries,unknown2);
        }
    }
}
