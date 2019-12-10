using System;

namespace Crash
{
    public abstract class MysteryUniItemEntry : Entry
    {
        public MysteryUniItemEntry(byte[] data,int eid) : base(eid)
        {
            Data = data ?? throw new ArgumentNullException("data");
        }

        public byte[] Data { get; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [1][];
            items[0] = Data;
            int size = Data.Length;
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
