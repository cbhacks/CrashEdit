using System;

namespace Crash.Audio
{
    public sealed class WavebankChunk : EntryChunk
    {
        private WavebankEntry entry;
        private int unknown1;
        private int unknown2;

        public WavebankChunk(WavebankEntry entry,int unknown1,int unknown2)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");
            this.entry = entry;
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
        }

        public override short Type
        {
            get { return 4; }
        }

        public int Unknown1
        {
            get { return unknown1; }
        }

        public int Unknown2
        {
            get { return unknown2; }
        }

        public WavebankEntry Entry
        {
            get { return entry; }
        }

        public override byte[] Save()
        {
            Entry[] entries = new Entry [1];
            entries[0] = entry;
            return Save(entries,unknown1,unknown2,16,4);
        }
    }
}
