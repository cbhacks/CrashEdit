using Crash;

namespace CrashEdit
{
    public sealed class T11EntryController : MysteryMultiItemEntryController
    {
        private T11Entry t11entry;

        public T11EntryController(EntryChunkController entrychunkcontroller,T11Entry t11entry) : base(entrychunkcontroller,t11entry)
        {
            this.t11entry = t11entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T11 Entry ({0})",t11entry.EName);
            Node.ImageKey = "t11entry";
            Node.SelectedImageKey = "t11entry";
        }

        public T11Entry T11Entry
        {
            get { return t11entry; }
        }
    }
}
