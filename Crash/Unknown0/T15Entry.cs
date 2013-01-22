using System;

namespace Crash.Unknown0
{
    public sealed class T15Entry : Entry,IMysteryUniItemEntry
    {
        private byte[] data;

        public T15Entry(byte[] data,int unknown) : base(unknown)
        {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null.");
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
