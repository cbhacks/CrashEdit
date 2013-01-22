using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    [ChunkType(3)]
    public sealed class SoundChunkLoader : EntryChunkLoader
    {
        public override Chunk Load(Entry[] entries,int unknown1,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("Entries cannot be null.");
            List<SoundEntry> soundentries = new List<SoundEntry>();
            foreach (Entry entry in entries)
            {
                if (entry is SoundEntry)
                {
                    soundentries.Add((SoundEntry)entry);
                }
                else
                {
                    throw new Exception();
                }
            }
            return new SoundChunk(soundentries,unknown1,unknown2);
        }
    }
}
