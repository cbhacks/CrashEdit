using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class T4Item0Controller : Controller
    {
        private T4EntryController t4entrycontroller;
        private T4Item0 t4item;

        public T4Item0Controller(T4EntryController t4entrycontroller,T4Item0 t4item)
        {
            this.t4entrycontroller = t4entrycontroller;
            this.t4item = t4item;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Item";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new T4Item0Box(t4item);
        }

        public T4EntryController T4EntryController
        {
            get { return t4entrycontroller; }
        }

        public T4Item0 T4Item
        {
            get { return t4item; }
        }
    }
}
