using Crash;

namespace CrashEdit
{
    public sealed class T1EntryController : MysteryMultiItemEntryController
    {
        private T1Entry t1entry;

        public T1EntryController(EntryChunkController entrychunkcontroller,T1Entry t1entry) : base(entrychunkcontroller,t1entry)
        {
            this.t1entry = t1entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T1 ({0})",t1entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T1Entry T1Entry
        {
            get { return t1entry; }
        }
    }
}
