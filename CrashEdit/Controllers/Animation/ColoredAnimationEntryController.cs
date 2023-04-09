using System.IO;
using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ColoredAnimationEntryController : EntryController
    {
        public ColoredAnimationEntryController(EntryChunkController entrychunkcontroller, ColoredAnimationEntry coloredanimationentry)
            : base(entrychunkcontroller, coloredanimationentry)
        {
            ColoredAnimationEntry = coloredanimationentry;
            foreach (OldFrame frame in coloredanimationentry.Frames)
            {
                AddNode(new ColoredFrameController(this, frame));
            }
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ_Game);
            AddMenu ("Export as OBJ (processed geometry)", Menu_Export_OBJ_Processed);

            InvalidateNode();
            InvalidateNodeImage();
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
                if (node.Tag is not ColoredFrameController frame)
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
                if (node.Tag is not ColoredFrameController frame)
                    continue;

                string final = path + Path.DirectorySeparatorChar + filename + id.ToString () + ext;
                File.WriteAllBytes (final, frame.ToGameOBJ ());
                id++;
            }
        }
        
        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.ColoredAnimationEntryController_Text, ColoredAnimationEntry.EName);
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

        public ColoredAnimationEntry ColoredAnimationEntry { get; }
    }
}

