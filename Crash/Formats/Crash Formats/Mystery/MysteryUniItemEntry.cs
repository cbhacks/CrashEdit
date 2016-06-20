using System;

namespace Crash
{
    public abstract class MysteryUniItemEntry : Entry
    {
        private byte[] data;

        public MysteryUniItemEntry(byte[] data,int eid) : base(eid)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [1][];
            items[0] = data;
            int size = data.Length;
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
