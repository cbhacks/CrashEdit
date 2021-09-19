namespace CrashEdit.Crash
{
    public sealed class DemoEntry : MysteryUniItemEntry
    {
        public DemoEntry(byte[] data,int eid) : base(data,eid)
        {
        }

        public override string Title => $"Demo ({EName})";
        public override string ImageKey => "ThingOrange";

        public override int Type => 19;
    }
}
