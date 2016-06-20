using Crash;

namespace CrashEdit
{
    public sealed class T4EntryController : EntryController
    {
        private T4Entry t4entry;

        public T4EntryController(EntryChunkController entrychunkcontroller,T4Entry t4entry) : base(entrychunkcontroller,t4entry)
        {
            this.t4entry = t4entry;
            //AddNode(new T4Item0Controller(null,t4entry.T4ItemFirst));
            foreach (T4Item t4item in t4entry.T4Items)
            {
                AddNode(new T4ItemController(this,t4item));
            }
            //AddNode(new T4Item0Controller(null,t4entry.T4ItemLast));
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T4 ({0})",t4entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T4Entry T4Entry
        {
            get { return t4entry; }
        }
    }
}
