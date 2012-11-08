namespace Crash.Unknown5
{
    public sealed class T20Entry : Entry,IMysteryUniItemEntry
    {
        private byte[] data;

        public T20Entry(byte[] data,int unknown) : base(unknown)
        {
            this.data = data;
        }

        public override int Type
        {
            get { return 20; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override byte[] Save()
        {
            byte[][] items = new byte [1][];
            items[0] = data;
            return Save(items);
        }
    }
}
