using Crash;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
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
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldModelEntryController_Text,OldModelEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "crimsonb";
            Node.SelectedImageKey = "crimsonb";
        }

        protected override Control CreateEditor()
        {
            return new Label { Text = string.Format("Polygon count: {0}", BitConv.FromInt32(OldModelEntry.Info, 0)), TextAlign = ContentAlignment.MiddleCenter };
        }

        public OldModelEntry OldModelEntry { get; }
    }
}
