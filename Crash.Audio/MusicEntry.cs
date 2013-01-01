namespace Crash.Audio
{
    public sealed class MusicEntry : Entry
    {
        private byte[] unknown1;
        private byte[] vh;
        private SEP sep;

        public MusicEntry(byte[] unknown1,byte[] vh,SEP sep,int unknown) : base(unknown)
        {
            if (unknown1 == null)
                throw new System.ArgumentNullException("Unknown1 cannot be null.");
            if (vh == null)
                throw new System.ArgumentNullException("VH cannot be null.");
            if (sep == null)
                throw new System.ArgumentNullException("SEP cannot be null.");
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

        public byte[] VH
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
            items[1] = vh;
            items[2] = sep.Save();
            return Save(items);
        }
    }
}
