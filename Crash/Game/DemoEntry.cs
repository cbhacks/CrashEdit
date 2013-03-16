namespace Crash.Game
{
    public sealed class DemoEntry : MysteryUniItemEntry
    {
        public DemoEntry(byte[] data,int eid) : base(data,eid)
        {
        }

        public override int Type
        {
            get { return 19; }
        }
    }
}
