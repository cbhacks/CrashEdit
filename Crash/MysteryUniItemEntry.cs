using System;

namespace Crash
{
    public abstract class MysteryUniItemEntry : Entry
    {
        private byte[] data;

        public MysteryUniItemEntry(byte[] data,int unknown) : base(unknown)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
        }

        public byte[] Data
        {
            get { return data; }
        }

        public sealed override byte[] Save()
        {
            byte[][] items = new byte [1][];
            items[0] = data;
            return Save(items);
        }
    }
}
