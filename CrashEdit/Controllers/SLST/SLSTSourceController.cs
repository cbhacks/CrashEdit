using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class SLSTSourceController : LegacyController
    {
        public SLSTSourceController(SLSTEntryController slstentrycontroller,SLSTSource slstsource) : base(slstentrycontroller, slstsource)
        {
            SLSTSource = slstsource;
            InvalidateNode();
        }

        public void InvalidateNode()
        {
            NodeText = "Source";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SLSTSourceBox(SLSTSource);
        }

        public SLSTEntryController SLSTEntryController => (SLSTEntryController)Modern.Parent.Legacy;
        public SLSTSource SLSTSource { get; }
    }
}
