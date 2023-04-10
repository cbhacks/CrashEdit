using System.IO;
using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(EntryChunkController entrychunkcontroller, AnimationEntry animationentry) : base(entrychunkcontroller, animationentry)
        {
            AnimationEntry = animationentry;
            foreach (Frame frame in animationentry.Frames)
            {
                AddNode(new FrameController(this, frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
            AddMenu ("Export as OBJ", Menu_Export_OBJ_Game);
        }

        private void Menu_Export_OBJ_Game ()
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
                if (node.Tag is not FrameController frame)
                    continue;

                frame.ToOBJ (path, filename + id.ToString());
                id++;
            }
        }
        
        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.AnimationEntryController_Text, AnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            if (AnimationEntry.IsNew)
                return new Crash3AnimationSelector(NSF, AnimationEntry);
            else
                return new UndockableControl(new AnimationEntryViewer(NSF, Entry.EID));
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
