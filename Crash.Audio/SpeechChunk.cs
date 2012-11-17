using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SpeechChunk : EntryChunk
    {
        private List<SpeechEntry> entries;
        private int unknown1;
        private int unknown2;

        public SpeechChunk(IEnumerable<SpeechEntry> entries,int unknown1,int unknown2)
        {
            this.entries = new List<SpeechEntry>(entries);
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 5; }
        }

        public IList<SpeechEntry> Entries
        {
            get { return entries; }
        }

        public override byte[] Save()
        {
            Entry[] entries = new Entry [this.entries.Count];
            for (int i = 0;i < this.entries.Count;i++)
            {
                entries[i] = this.entries[i];
            }
            return Save(entries,unknown1,unknown2);
        }
    }
}
