using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldSceneryEntryController : EntryController
    {
        private OldSceneryEntry oldsceneryentry;

        public OldSceneryEntryController(EntryChunkController entrychunkcontroller,OldSceneryEntry oldsceneryentry) : base(entrychunkcontroller,oldsceneryentry)
        {
            this.oldsceneryentry = oldsceneryentry;
            Node.Text = string.Format("Old Scenery Entry ({0})",oldsceneryentry.EIDString);
            Node.ImageKey = "oldsceneryentry";
            Node.SelectedImageKey = "oldsceneryentry";
            AddMenuSeparator();
            AddMenu("Export as OBJ",Menu_Export_OBJ);
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new OldSceneryEntryViewer(oldsceneryentry));
        }

        public OldSceneryEntry OldSceneryEntry
        {
            get { return oldsceneryentry; }
        }

        private void Menu_Export_OBJ()
        {
            if (MessageBox.Show("Exporting to OBJ is experimental.\nTexture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(oldsceneryentry.ToOBJ(),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
