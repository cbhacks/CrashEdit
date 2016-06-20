using Crash;

namespace CrashEdit
{
    public sealed class T17EntryController : MysteryMultiItemEntryController
    {
        private T17Entry t17entry;

        public T17EntryController(EntryChunkController entrychunkcontroller,T17Entry t17entry) : base(entrychunkcontroller,t17entry)
        {
            this.t17entry = t17entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T17 ({0})",t17entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T17Entry T17Entry
        {
            get { return t17entry; }
        }
    }
}
