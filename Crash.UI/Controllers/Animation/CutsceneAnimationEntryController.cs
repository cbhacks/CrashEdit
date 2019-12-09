namespace Crash.UI
{
    public sealed class CutsceneAnimationEntryController : EntryController
    {
        public CutsceneAnimationEntryController(EntryChunkController up,CutsceneAnimationEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new CutsceneAnimationEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.CutsceneAnimationEntryController_Text,Entry.EName);
    }
}
