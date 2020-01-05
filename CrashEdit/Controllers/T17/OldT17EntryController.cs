using Crash;

namespace CrashEdit
{
    public sealed class OldT17EntryController : MysteryMultiItemEntryController
    {
        public OldT17EntryController(EntryChunkController entrychunkcontroller,OldT17Entry oldt17entry) : base(entrychunkcontroller,oldt17entry)
        {
            OldT17Entry = oldt17entry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldT17EntryController_Text,OldT17Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public OldT17Entry OldT17Entry { get; }
    }
}
