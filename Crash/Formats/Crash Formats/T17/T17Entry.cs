namespace CrashEdit.Crash
{
    public sealed class T17Entry : MysteryMultiItemEntry
    {
        public T17Entry(IEnumerable<byte[]> items, int eid) : base(items, eid)
        {
        }

        public override string Title => $"T17 ({EName})";
        public override string ImageKey => "ThingOrange";

        public override int Type => 17;
    }
}
