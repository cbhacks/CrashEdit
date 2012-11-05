namespace Crash.Unknown4
{
    public sealed class T14Entry : Entry
    {
        private int id;
        private byte[] data;

        public T14Entry(int id,byte[] data,int unknown) : base(unknown)
        {
            this.id = id;
            this.data = data;
        }

        public override int Type
        {
            get { return 14; }
        }

        public int ID
        {
            get { return id; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override byte[] Save()
        {
            byte[] info = new byte [8];
            BitConv.ToWord(info,0,id);
            BitConv.ToWord(info,4,data.Length);
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = data;
            return Save(items);
        }
    }
}
