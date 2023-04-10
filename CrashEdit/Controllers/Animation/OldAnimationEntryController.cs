using System.IO;
using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(EntryChunkController entrychunkcontroller, OldAnimationEntry oldanimationentry) : base(entrychunkcontroller, oldanimationentry)
        {
            OldAnimationEntry = oldanimationentry;
            foreach (OldFrame frame in oldanimationentry.Frames)
            {
                AddNode(new OldFrameController(this, frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
            AddMenu ("Export as OBJ", Menu_Export_OBJ);
        }

        private void Menu_Export_OBJ ()
        {
            if (!FileUtil.SelectSaveFile (out string output, FileFilters.OBJ, FileFilters.Any))
                return;
            
            // modify the path to add a number before the extension
            string ext = Path.GetExtension (output);
            string filename = Path.GetFileNameWithoutExtension (output);
            string path = Path.GetDirectoryName (output);

            int id = 0;

            foreach (TreeNode node in Node.Nodes)
            {
                if (node.Tag is not OldFrameController frame)
                    continue;

                frame.ToOBJ (path, filename + id.ToString());
                id++;
            }
        }
        
        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldAnimationEntryController_Text, OldAnimationEntry.EName);
            Node.Text = string.Format(OldAnimationEntry.Proto ? Crash.UI.Properties.Resources.ProtoAnimationEntryController_Text
                                                              : Crash.UI.Properties.Resources.OldAnimationEntryController_Text, OldAnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new OldAnimationEntryViewer(NSF, Entry.EID));
        }

        public OldAnimationEntry OldAnimationEntry { get; }
    }
}
