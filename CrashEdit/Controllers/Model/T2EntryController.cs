using Crash;

namespace CrashEdit
{
    public sealed class T2EntryController : EntryController
    {
        public T2EntryController(EntryChunkController entrychunkcontroller,T2Entry t2entry) : base(entrychunkcontroller,t2entry)
        {
            T2Entry = t2entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Model ({0})",T2Entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T2Entry T2Entry { get; }
    }
}
