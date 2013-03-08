using System;
using System.Collections.Generic;

namespace Crash.Audio
{
    public sealed class SpeechChunk : EntryChunk
    {
        private List<SpeechEntry> entries;
        private int unknown2;

        public SpeechChunk(IEnumerable<SpeechEntry> entries,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            this.entries = new List<SpeechEntry>(entries);
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 5; }
        }

        public int Unknown2
        {
            get { return unknown2; }
        }

        public IList<SpeechEntry> Entries
        {
            get { return entries; }
        }

        public override byte[] Save(int chunkid)
        {
            Entry[] entries = new Entry [this.entries.Count];
            for (int i = 0;i < this.entries.Count;i++)
            {
                entries[i] = this.entries[i];
            }
            return Save(chunkid,entries,unknown2,16,8);
        }
    }
}
