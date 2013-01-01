namespace Crash.Audio
{
    public sealed class MusicEntry : Entry
    {
        private byte[] unknown1;
        private byte[] vab;
        private SEP sep;

        public MusicEntry(byte[] unknown1,byte[] vab,SEP sep,int unknown) : base(unknown)
        {
            if (unknown1 == null)
                throw new System.ArgumentNullException("Unknown1 cannot be null.");
            if (vab == null)
                throw new System.ArgumentNullException("VAB cannot be null.");
            if (sep == null)
                throw new System.ArgumentNullException("SEP cannot be null.");
            this.unknown1 = unknown1;
            this.vab = vab;
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

        public byte[] VAB
        {
            get { return vab; }
        }

        public SEP SEP
        {
            get { return sep; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [3][];
            items[0] = unknown1;
            items[1] = vab;
            items[2] = sep.Save();
            return Save(items);
        }
    }
}
