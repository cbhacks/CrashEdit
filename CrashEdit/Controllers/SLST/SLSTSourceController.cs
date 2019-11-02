using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTSourceController : Controller
    {
        public SLSTSourceController(SLSTEntryController slstentrycontroller,SLSTSource slstsource)
        {
            SLSTEntryController = slstentrycontroller;
            SLSTSource = slstsource;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Source";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new SLSTSourceBox(SLSTSource);
        }

        public SLSTEntryController SLSTEntryController { get; }
        public SLSTSource SLSTSource { get; }
    }
}
