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
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ_Game);
            AddMenu ("Export as OBJ (processed geometry)", Menu_Export_OBJ_Processed);
        }

        private void Menu_Export_OBJ_Processed()
        {
            FileUtil.SelectSaveFile (out string output, FileFilters.OBJ, FileFilters.Any);
            
            // modify the path to add a number before the extension
            string ext = Path.GetExtension (output);
            string filename = Path.GetFileNameWithoutExtension (output);
            string path = Path.GetDirectoryName (output);

            int id = 0;

            foreach (TreeNode node in Node.Nodes)
            {
                if (node.Tag is not FrameController frame)
                    continue;

                string final = path + Path.DirectorySeparatorChar + filename + id.ToString () + ext;
                File.WriteAllBytes (final, frame.ToProcessedOBJ ());
                id++;
            }
        }

        private void Menu_Export_OBJ_Game ()
        {
            FileUtil.SelectSaveFile (out string output, FileFilters.OBJ, FileFilters.Any);
            
            // modify the path to add a number before the extension
            string ext = Path.GetExtension (output);
            string filename = Path.GetFileNameWithoutExtension (output);
            string path = Path.GetDirectoryName (output);

            int id = 0;

            foreach (TreeNode node in Node.Nodes)
            {
                if (node.Tag is not FrameController frame)
                    continue;

                string final = path + Path.DirectorySeparatorChar + filename + id.ToString () + ext;
                File.WriteAllBytes (final, frame.ToGameOBJ ());
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
            return new UndockableControl(new AnimationEntryViewer(NSF, Entry.EID));
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
