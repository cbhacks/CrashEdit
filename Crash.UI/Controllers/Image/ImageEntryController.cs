namespace Crash.UI
{
    public sealed class ImageEntryController : MysteryMultiItemEntryController
    {
        public ImageEntryController(EntryChunkController up,ImageEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new ImageEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.ImageEntryController_Text,Entry.EName);
    }
}
