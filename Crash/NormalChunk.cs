using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NormalChunk : EntryChunk
    {
        private List<Entry> entries;
        private int unknown2;

        public NormalChunk(IEnumerable<Entry> entries,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            this.entries = new List<Entry>(entries);
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 0; }
        }

        public int Unknown2
        {
            get { return unknown2; }
        }

        public IList<Entry> Entries
        {
            get { return entries; }
        }

        public override byte[] Save(int chunkid)
        {
            return Save(chunkid,entries,unknown2);
        }
    }
}
