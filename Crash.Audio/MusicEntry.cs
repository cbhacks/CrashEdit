namespace Crash.Audio
{
    public sealed class MusicEntry : Entry
    {
        private byte[] unknown1;
        private byte[] vab;
        private byte[] seq;

        public MusicEntry(byte[] unknown1,byte[] vab,byte[] seq,int unknown) : base(unknown)
        {
            this.unknown1 = unknown1;
            this.vab = vab;
            this.seq = seq;
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

        public byte[] SEQ
        {
            get { return seq; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [3][];
            items[0] = unknown1;
            items[1] = vab;
            items[2] = seq;
            return Save(items);
        }
    }
}
