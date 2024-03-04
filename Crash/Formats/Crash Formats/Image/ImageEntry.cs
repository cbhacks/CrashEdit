namespace CrashEdit.Crash
{
    public sealed class ImageEntry : MysteryMultiItemEntry
    {
        public ImageEntry(IEnumerable<byte[]> items, int eid) : base(items, eid)
        {
        }

        public override string Title => $"Image ({EName})";
        public override string ImageKey => "Painting";

        public override int Type => 15;
    }
}
