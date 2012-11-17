using System.Collections.Generic;

namespace Crash.Audio
{
    [ChunkType(5)]
    public sealed class SpeechChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            List<SpeechEntry> speechentries = new List<SpeechEntry>();
            foreach (Entry entry in entries)
            {
                if (entry is SpeechEntry)
                {
                    speechentries.Add((SpeechEntry)entry);
                }
                else
                {
                    throw new System.Exception();
                }
            }
            return new SpeechChunk(speechentries,unknown1,unknown2);
        }
    }
}
