namespace Crash.UI
{
    public sealed class CutsceneAnimationEntryController : EntryController
    {
        private CutsceneAnimationEntry entry;

        public CutsceneAnimationEntryController(EntryChunkController up,CutsceneAnimationEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new CutsceneAnimationEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.CutsceneAnimationEntryController_Text,entry.EName);
        }
    }
}
