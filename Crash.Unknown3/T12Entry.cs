namespace Crash.Unknown3
{
    public sealed class T12Entry : Entry
    {
        private byte[] data;

        public T12Entry(byte[] data,int unknown) : base(unknown)
        {
            this.data = data;
        }

        public override int Type
        {
            get { return 12; }
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
