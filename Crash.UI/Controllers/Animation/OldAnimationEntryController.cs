namespace Crash.UI
{
    public sealed class OldAnimationEntryController : EntryController
    {
        private OldAnimationEntry entry;

        public OldAnimationEntryController(EntryChunkController up,OldAnimationEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldAnimationEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldAnimationEntryController_Text,entry.EName);
        }
    }
}
