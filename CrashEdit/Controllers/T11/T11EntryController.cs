using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class T11EntryController : MysteryMultiItemEntryController
    {
        public T11EntryController(EntryChunkController entrychunkcontroller,T11Entry t11entry) : base(entrychunkcontroller,t11entry)
        {
            T11Entry = t11entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("GOOLv2 ({0})",T11Entry.EName);
            Node.ImageKey = "codeb";
            Node.SelectedImageKey = "codeb";
        }

        protected override Control CreateEditor()
        {
            return new GOOLBox(T11Entry);
        }

        public T11Entry T11Entry { get; }
    }
}
