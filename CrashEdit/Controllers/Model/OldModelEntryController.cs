using CrashEdit.Crash;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldModelEntryController : EntryController
    {
        public OldModelEntryController(EntryChunkController entrychunkcontroller,OldModelEntry oldmodelentry) : base(entrychunkcontroller,oldmodelentry)
        {
            OldModelEntry = oldmodelentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.OldModelEntryController_Text,OldModelEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "crimsonb";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new Label { Text = string.Format("Polygon count: {0}", BitConv.FromInt32(OldModelEntry.Info, 0)), TextAlign = ContentAlignment.MiddleCenter };
        }

        public OldModelEntry OldModelEntry { get; }
    }
}
