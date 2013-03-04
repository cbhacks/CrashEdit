using System;

namespace Crash.Audio
{
    public sealed class MusicEntry : Entry
    {
        private byte[] unknown1;
        private VH vh;
        private SEP sep;

        public MusicEntry(byte[] unknown1,VH vh,SEP sep,int unknown) : base(unknown)
        {
            if (unknown1 == null)
                throw new ArgumentNullException("unknown1");
            if (sep == null)
                throw new ArgumentNullException("sep");
            this.unknown1 = unknown1;
            this.vh = vh;
            this.sep = sep;
        }

        public override int Type
        {
            get { return 13; }
        }

        public byte[] Unknown1
        {
            get { return unknown1; }
        }

        public VH VH
        {
            get { return vh; }
        }

        public SEP SEP
        {
            get { return sep; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [3][];
            items[0] = unknown1;
            if (vh != null)
            {
                items[1] = vh.Save();
            }
            else
            {
                items[1] = new byte [0];
            }
            items[2] = sep.Save();
            return Save(items);
        }
    }
}
