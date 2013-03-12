namespace Crash.Game
{
    public sealed class DemoEntry : MysteryUniItemEntry
    {
        public DemoEntry(byte[] data,int unknown) : base(data,unknown)
        {
        }

        public override int Type
        {
            get { return 19; }
        }
    }
}
