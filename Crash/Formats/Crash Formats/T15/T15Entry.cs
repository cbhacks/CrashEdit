namespace Crash
{
    public sealed class T15Entry : MysteryUniItemEntry
    {
        public T15Entry(byte[] data,int eid,int size) : base(data,eid,size)
        {
        }

        public override int Type
        {
            get { return 15; }
        }
    }
}
