using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTItem0Controller : Controller
    {
        private SLSTEntryController slstentrycontroller;
        private SLSTItem0 slstitem;

        public SLSTItem0Controller(SLSTEntryController slstentrycontroller,SLSTItem0 slstitem)
        {
            this.slstentrycontroller = slstentrycontroller;
            this.slstitem = slstitem;
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
            return new SLSTItem0Box(slstitem);
        }

        public SLSTEntryController SLSTEntryController
        {
            get { return slstentrycontroller; }
        }

        public SLSTItem0 SLSTItem
        {
            get { return slstitem; }
        }
    }
}
