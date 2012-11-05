using System.Collections.Generic;

namespace Crash.Unknown3
{
    [ChunkType(3)]
    public sealed class T3ChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            List<T12Entry> t12entries = new List<T12Entry>();
            foreach (Entry entry in entries)
            {
                if (entry is T12Entry)
                {
                    t12entries.Add((T12Entry)entry);
                }
                else
                {
                    throw new System.Exception();
                }
            }
            return new T3Chunk(t12entries,unknown1,unknown2);
        }
    }
}
