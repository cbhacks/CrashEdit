using Crash;

namespace CrashEdit
{
    public sealed class T20EntryController : MysteryMultiItemEntryController
    {
        private T20Entry t20entry;

        public T20EntryController(EntryChunkController entrychunkcontroller,T20Entry t20entry) : base(entrychunkcontroller,t20entry)
        {
            this.t20entry = t20entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T20 Entry ({0})",t20entry.EName);
            Node.ImageKey = "t20entry";
            Node.SelectedImageKey = "t20entry";
        }

        public T20Entry T20Entry
        {
            get { return t20entry; }
        }
    }
}
