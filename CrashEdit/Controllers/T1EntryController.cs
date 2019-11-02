using Crash;

namespace CrashEdit
{
    public sealed class T1EntryController : MysteryMultiItemEntryController
    {
        public T1EntryController(EntryChunkController entrychunkcontroller,T1Entry t1entry) : base(entrychunkcontroller,t1entry)
        {
            T1Entry = t1entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T1 ({0})",T1Entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T1Entry T1Entry { get; }
    }
}
