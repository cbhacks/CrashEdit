using System.Collections.Generic;

namespace CrashEdit.Crash
{
    public sealed class OldSoundChunk : EntryChunk
    {
        public OldSoundChunk()
        {
        }

        public OldSoundChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override string Title => $"Old Sound Chunk {ChunkId}";
        public override string ImageKey => "JournalBlue";

        public override short Type => 2;
        public override int Alignment => 16;
    }
}
