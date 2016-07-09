using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTItemController : Controller
    {
        private SLSTEntryController slstentrycontroller;
        private SLSTItem slstitem;

        public SLSTItemController(SLSTEntryController slstentrycontroller,SLSTItem slstitem)
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
            return new SLSTItemBox(slstitem);
        }

        public SLSTEntryController SLSTEntryController
        {
            get { return slstentrycontroller; }
        }

        public SLSTItem SLSTItem
        {
            get { return slstitem; }
        }
    }
}
