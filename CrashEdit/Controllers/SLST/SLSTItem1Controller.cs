using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTItem1Controller : Controller
    {
        private SLSTEntryController slstentrycontroller;
        private SLSTItem1 slstitem;

        public SLSTItem1Controller(SLSTEntryController slstentrycontroller,SLSTItem1 slstitem)
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
            return new SLSTItem1Box(slstitem);
        }

        public SLSTEntryController SLSTEntryController
        {
            get { return slstentrycontroller; }
        }

        public SLSTItem1 SLSTItem
        {
            get { return slstitem; }
        }
    }
}
