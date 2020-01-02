using Crash;

namespace CrashEdit
{
    public sealed class T15EntryController : MysteryUniItemEntryController
    {
        public T15EntryController(EntryChunkController entrychunkcontroller,T15Entry t15entry) : base(entrychunkcontroller,t15entry)
        {
            T15Entry = t15entry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T15 ({0})",T15Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T15Entry T15Entry { get; }
    }
}
