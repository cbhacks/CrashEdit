using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldSLSTSourceController : Controller
    {
        public OldSLSTSourceController(OldSLSTEntryController oldslstentrycontroller,OldSLSTSource oldslstsource) : base(oldslstentrycontroller, oldslstsource)
        {
            OldSLSTEntryController = oldslstentrycontroller;
            OldSLSTSource = oldslstsource;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Source";
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldSLSTSourceBox(OldSLSTSource);
        }

        public OldSLSTEntryController OldSLSTEntryController { get; }
        public OldSLSTSource OldSLSTSource { get; }
    }
}
