namespace CrashEdit.Crash
{
    public sealed class T21Entry : MysteryMultiItemEntry
    {
        public T21Entry(IEnumerable<byte[]> items, int eid) : base(items, eid)
        {
        }

        public override string Title => $"T21 ({EName})";
        public override string ImageKey => "Painting";

        public override int Type => 21;
    }
}
