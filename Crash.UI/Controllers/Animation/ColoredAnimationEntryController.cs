namespace Crash.UI
{
    public sealed class CutsceneAnimationEntryController : EntryController
    {
        public CutsceneAnimationEntryController(EntryChunkController up,ColoredAnimationEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new ColoredAnimationEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.ColoredAnimationEntryController_Text,Entry.EName);
    }
}
