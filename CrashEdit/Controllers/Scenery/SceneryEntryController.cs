using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SceneryEntryController : EntryController
    {
        private SceneryEntry sceneryentry;

        public SceneryEntryController(EntryChunkController entrychunkcontroller,SceneryEntry sceneryentry) : base(entrychunkcontroller,sceneryentry)
        {
            this.sceneryentry = sceneryentry;
            Node.Text = string.Format("Scenery Entry ({0})",sceneryentry.EIDString);
            Node.ImageKey = "sceneryentry";
            Node.SelectedImageKey = "sceneryentry";

            AddMenuSeparator();
            AddMenu("Export as OBJ", Menu_Export_OBJ);
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new SceneryEntryViewer(sceneryentry));
        }

        public SceneryEntry SceneryEntry
        {
            get { return sceneryentry; }
        }

        private void Menu_Export_OBJ()
        {
            if(MessageBox.Show("Exporting OBJ is experimental.\nTexture and color information will not be exported.\n\nContinue anyway?", "Export as OBJ", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            FileUtil.SaveFile(sceneryentry.ToOBJ(), FileFilters.OBJ, FileFilters.Any);
        }
    }
}
