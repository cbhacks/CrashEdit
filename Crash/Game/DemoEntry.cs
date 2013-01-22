namespace Crash.Game
{
    public sealed class DemoEntry : Entry,IMysteryUniItemEntry
    {
        private byte[] data;

        public DemoEntry(byte[] data,int unknown) : base(unknown)
        {
            this.data = data;
        }

        public override int Type
        {
            get { return 19; }
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
