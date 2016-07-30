namespace Crash
{
    public sealed class DemoEntry : MysteryUniItemEntry
    {
        public DemoEntry(byte[] data,int eid,int size) : base(data,eid,size)
        {
        }

        public override int Type
        {
            get { return 19; }
        }
    }
}
