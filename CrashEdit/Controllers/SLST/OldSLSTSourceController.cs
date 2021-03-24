using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldSLSTSourceController : LegacyController
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
            NodeText = "Source";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldSLSTSourceBox(OldSLSTSource);
        }

        public OldSLSTEntryController OldSLSTEntryController { get; }
        public OldSLSTSource OldSLSTSource { get; }
    }
}
