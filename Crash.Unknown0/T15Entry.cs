namespace Crash.Unknown0
{
    public sealed class T15Entry : Entry
    {
        private byte[] data;

        public T15Entry(byte[] data,int unknown) : base(unknown)
        {
            this.data = data;
        }

        public override int Type
        {
            get { return 15; }
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
