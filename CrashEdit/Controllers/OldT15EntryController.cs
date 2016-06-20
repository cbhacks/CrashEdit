using Crash;

namespace CrashEdit
{
    public sealed class OldT15EntryController : MysteryMultiItemEntryController
    {
        private OldT15Entry oldt15entry;

        public OldT15EntryController(EntryChunkController entrychunkcontroller,OldT15Entry oldt15entry) : base(entrychunkcontroller,oldt15entry)
        {
            this.oldt15entry = oldt15entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old T15 ({0})",oldt15entry.EName);
            Node.ImageKey = "image";
            Node.SelectedImageKey = "image";
        }

        public OldT15Entry OldT15Entry
        {
            get { return oldt15entry; }
        }
    }
}
