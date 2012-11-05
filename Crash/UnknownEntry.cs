namespace Crash
{
    public sealed class UnknownEntry : Entry
    {
        private byte[][] items;
        private int type;

        public UnknownEntry(byte[][] items,int unknown,int type) : base(unknown)
        {
            this.items = items;
            this.type = type;
        }

        public override int Type
        {
            get { return type; }
        }

        public byte[][] Items
        {
            get { return items; }
        }

        public override byte[] Save()
        {
            return Save(items);
        }
    }
}
