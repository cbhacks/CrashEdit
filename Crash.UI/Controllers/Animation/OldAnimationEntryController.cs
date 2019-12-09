namespace Crash.UI
{
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(EntryChunkController up,OldAnimationEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldAnimationEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldAnimationEntryController_Text,Entry.EName);
    }
}
