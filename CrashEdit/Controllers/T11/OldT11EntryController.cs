using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldT11EntryController : EntryController
    {
        public OldT11EntryController(EntryChunkController entrychunkcontroller,OldT11Entry oldt11entry) : base(entrychunkcontroller,oldt11entry)
        {
            OldT11Entry = oldt11entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("GOOL ({0})",OldT11Entry.EName);
            Node.ImageKey = "codeb";
            Node.SelectedImageKey = "codeb";
        }

        protected override Control CreateEditor()
        {
            return new OldGOOLBox(OldT11Entry);
        }

        public OldT11Entry OldT11Entry { get; }
    }
}
