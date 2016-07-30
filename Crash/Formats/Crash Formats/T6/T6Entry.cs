namespace Crash
{
    public sealed class T6Entry : MysteryUniItemEntry
    {
        public T6Entry(byte[] data,int eid,int size) : base(data,eid,size)
        {
        }

        public override int Type
        {
            get { return 6; }
        }
    }
}
