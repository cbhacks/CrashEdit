using System;

namespace Crash.Audio
{
    public sealed class WavebankChunk : EntryChunk
    {
        private WavebankEntry entry;
        private int unknown2;

        public WavebankChunk(WavebankEntry entry,int unknown2)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");
            this.entry = entry;
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 4; }
        }

        public int Unknown2
        {
            get { return unknown2; }
        }

        public WavebankEntry Entry
        {
            get { return entry; }
        }

        public override byte[] Save(int chunkid)
        {
            Entry[] entries = new Entry [1];
            entries[0] = entry;
            return Save(chunkid,entries,unknown2,16,4);
        }
    }
}
