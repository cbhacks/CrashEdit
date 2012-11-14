using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SoundChunk : Chunk
    {
        private List<SoundEntry> entries;
        private int unknown1;
        private int unknown2;

        public SoundChunk(IEnumerable<SoundEntry> entries,int unknown1,int unknown2)
        {
            this.entries = new List<SoundEntry>(entries);
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 3; }
        }

        public IList<SoundEntry> Entries
        {
            get { return entries; }
        }

        public override byte[] Save()
        {
            byte[] data = new byte [Length];
            BitConv.ToHalf(data,0,Magic);
            BitConv.ToHalf(data,2,Type);
            BitConv.ToWord(data,4,unknown1);
            BitConv.ToWord(data,8,entries.Count);
            BitConv.ToWord(data,12,unknown2);
            int offset = 20 + entries.Count * 4;
            offset += offset % 8; // Sounds must be 8-byte aligned
            BitConv.ToWord(data,16,offset);
            for (int i = 0;i < entries.Count;i++)
            {
                byte[] entrydata = entries[i].Save();
                entrydata.CopyTo(data,offset);
                offset += entrydata.Length;
                BitConv.ToWord(data,20 + i * 4,offset);
            }
            return data;
        }
    }
}
