using Crash;

namespace CrashEdit
{
    public sealed class T2EntryController : EntryController
    {
        private T2Entry t2entry;

        public T2EntryController(EntryChunkController entrychunkcontroller,T2Entry t2entry) : base(entrychunkcontroller,t2entry)
        {
            this.t2entry = t2entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Model ({0})",t2entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T2Entry T2Entry
        {
            get { return t2entry; }
        }
    }
}
