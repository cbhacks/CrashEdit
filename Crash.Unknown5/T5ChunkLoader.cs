using System.Collections.Generic;

namespace Crash.Unknown5
{
    [ChunkType(5)]
    public sealed class T5ChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            List<T20Entry> t20entries = new List<T20Entry>();
            foreach (Entry entry in entries)
            {
                if (entry is T20Entry)
                {
                    t20entries.Add((T20Entry)entry);
                }
                else
                {
                    throw new System.Exception();
                }
            }
            return new T5Chunk(t20entries,unknown1,unknown2);
        }
    }
}
