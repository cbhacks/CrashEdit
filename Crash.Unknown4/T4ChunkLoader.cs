namespace Crash.Unknown4
{
    [ChunkType(4)]
    public sealed class T4ChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            if (entries.Length != 1)
            {
                throw new System.Exception();
            }
            if (!(entries[0] is T14Entry))
            {
                throw new System.Exception();
            }
            return new T4Chunk((T14Entry)entries[0],unknown1,unknown2);
        }
    }
}
