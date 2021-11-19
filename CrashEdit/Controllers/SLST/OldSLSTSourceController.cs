using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldSLSTSource))]
    public sealed class OldSLSTSourceController : LegacyController
    {
        public OldSLSTSourceController(OldSLSTSource oldslstsource, SubcontrollerGroup parentGroup) : base(parentGroup, oldslstsource)
        {
            OldSLSTSource = oldslstsource;
            InvalidateNode();
        }

        public void InvalidateNode()
        {
            NodeText = "Source";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldSLSTSourceBox(OldSLSTSource);
        }

        public OldSLSTSource OldSLSTSource { get; }
    }
}
