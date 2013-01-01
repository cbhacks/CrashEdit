namespace Crash.Audio
{
    [ChunkType(4)]
    public sealed class WavebankChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            if (entries == null)
                throw new System.ArgumentNullException("Entries cannot be null.");
            if (entries.Length != 1)
            {
                throw new System.Exception();
            }
            if (!(entries[0] is WavebankEntry))
            {
                throw new System.Exception();
            }
            return new WavebankChunk((WavebankEntry)entries[0],unknown1,unknown2);
        }
    }
}
