using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NormalChunk : EntryChunk
    {
        private List<Entry> entries;
        private int unknown1;
        private int unknown2;

        public NormalChunk(IEnumerable<Entry> entries,int unknown1,int unknown2)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            this.entries = new List<Entry>(entries);
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 0; }
        }

        public int Unknown1
        {
            get { return unknown1; }
        }

        public int Unknown2
        {
            get { return unknown2; }
        }

        public IList<Entry> Entries
        {
            get { return entries; }
        }

        public override byte[] Save()
        {
            return Save(entries,unknown1,unknown2);
        }
    }
}
