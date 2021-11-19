using System.Collections.Generic;

namespace CrashEdit.Crash
{
    public sealed class WavebankChunk : EntryChunk
    {
        public WavebankChunk()
        {
        }

        public WavebankChunk(IEnumerable<Entry> entries) : base(entries)
        {
        }

        public override string Title => $"Wavebank Chunk {ChunkId}";
        public override string ImageKey => "MusicNoteRed";

        public override short Type => 4;
        public override int Alignment => 16;
    }
}
