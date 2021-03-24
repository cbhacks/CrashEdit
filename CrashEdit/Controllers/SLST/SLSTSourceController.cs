using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class SLSTSourceController : LegacyController
    {
        public SLSTSourceController(SLSTEntryController slstentrycontroller,SLSTSource slstsource) : base(slstentrycontroller, slstsource)
        {
            SLSTEntryController = slstentrycontroller;
            SLSTSource = slstsource;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = "Source";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SLSTSourceBox(SLSTSource);
        }

        public SLSTEntryController SLSTEntryController { get; }
        public SLSTSource SLSTSource { get; }
    }
}
