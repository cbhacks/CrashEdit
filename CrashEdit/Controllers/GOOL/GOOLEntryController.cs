using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class GOOLEntryController : EntryController
    {
        public GOOLEntryController(EntryChunkController entrychunkcontroller,GOOLEntry goolentry) : base(entrychunkcontroller,goolentry)
        {
            GOOLEntry = goolentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            switch (GOOLEntry.Version)
            {
                case GOOLVersion.Version0:
                    Node.Text = $"Prototype GOOL ({GOOLEntry.EName})";
                    break;
                case GOOLVersion.Version1:
                    Node.Text = $"GOOL ({GOOLEntry.EName})";
                    break;
                case GOOLVersion.Version2:
                    Node.Text = $"GOOLv2 ({GOOLEntry.EName})";
                    break;
            }
            Node.ImageKey = "codeb";
            Node.SelectedImageKey = "codeb";
        }

        protected override Control CreateEditor()
        {
            return new GOOLBox(GOOLEntry);
        }

        public GOOLEntry GOOLEntry { get; }
    }
}
