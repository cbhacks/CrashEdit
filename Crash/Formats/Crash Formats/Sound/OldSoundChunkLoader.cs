using System;

namespace CrashEdit.Crash
{
    [ChunkType(2)]
    public sealed class OldSoundChunkLoader : EntryChunkLoader
    {
        public override EntryChunk Load(Entry[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            return new OldSoundChunk(entries);
        }
    }
}
