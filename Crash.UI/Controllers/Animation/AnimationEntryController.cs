namespace Crash.UI
{
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(EntryChunkController up,AnimationEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new AnimationEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.AnimationEntryController_Text,Entry.EName);
    }
}
