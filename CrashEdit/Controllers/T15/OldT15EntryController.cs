using Crash;

namespace CrashEdit
{
    public sealed class OldT15EntryController : MysteryMultiItemEntryController
    {
        public OldT15EntryController(EntryChunkController entrychunkcontroller,OldT15Entry oldt15entry) : base(entrychunkcontroller,oldt15entry)
        {
            OldT15Entry = oldt15entry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old T15 ({0})",OldT15Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "image";
            Node.SelectedImageKey = "image";
        }

        public OldT15Entry OldT15Entry { get; }
    }
}
