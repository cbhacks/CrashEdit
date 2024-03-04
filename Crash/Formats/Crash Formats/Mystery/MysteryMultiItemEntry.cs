namespace CrashEdit.Crash
{
    public abstract class MysteryMultiItemEntry : Entry
    {
        private List<byte[]> items;

        public MysteryMultiItemEntry(IEnumerable<byte[]> items, int eid) : base(eid)
        {
            ArgumentNullException.ThrowIfNull(items);
            this.items = new List<byte[]>(items);
        }

        [SubresourceList]
        public IList<byte[]> Items => items;

        public override UnprocessedEntry Unprocess()
        {
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
