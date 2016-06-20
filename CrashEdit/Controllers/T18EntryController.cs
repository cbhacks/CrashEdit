using Crash;

namespace CrashEdit
{
    public sealed class T18EntryController : MysteryMultiItemEntryController
    {
        private T18Entry t18entry;

        public T18EntryController(EntryChunkController entrychunkcontroller,T18Entry t18entry) : base(entrychunkcontroller,t18entry)
        {
            this.t18entry = t18entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T18 ({0})",t18entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T18Entry T18Entry
        {
            get { return t18entry; }
        }
    }
}
