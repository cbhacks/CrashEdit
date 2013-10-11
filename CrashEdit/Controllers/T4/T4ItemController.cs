using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class T4ItemController : Controller
    {
        private T4EntryController t4entrycontroller;
        private T4Item t4item;

        public T4ItemController(T4EntryController t4entrycontroller,T4Item t4item)
        {
            this.t4entrycontroller = t4entrycontroller;
            this.t4item = t4item;
            Node.Text = "Item";
            Node.ImageKey = "t4item";
            Node.SelectedImageKey = "t4item";
        }

        protected override Control CreateEditor()
        {
            return new T4ItemBox(t4item);
        }

        public T4EntryController T4EntryController
        {
            get { return t4entrycontroller; }
        }

        public T4Item T4Item
        {
            get { return t4item; }
        }
    }
}
