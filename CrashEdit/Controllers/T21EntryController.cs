using Crash;

namespace CrashEdit
{
    public sealed class T21EntryController : MysteryMultiItemEntryController
    {
        private T21Entry t21entry;

        public T21EntryController(EntryChunkController entrychunkcontroller,T21Entry t21entry) : base(entrychunkcontroller,t21entry)
        {
            this.t21entry = t21entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T21 Entry ({0})",t21entry.EName);
            Node.ImageKey = "t21entry";
            Node.SelectedImageKey = "t21entry";
        }

        public T21Entry T21Entry
        {
            get { return t21entry; }
        }
    }
}
