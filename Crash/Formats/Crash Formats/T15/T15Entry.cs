namespace CrashEdit.Crash
{
    public sealed class T15Entry : MysteryUniItemEntry
    {
        public T15Entry(byte[] data, int eid) : base(data, eid)
        {
        }

        public override string Title => $"T15 ({EName})";
        public override string ImageKey => "ThingOrange";

        public override int Type => 15;
    }
}
