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
            if (oldsceneryentry.ExtraData != null)
            {
                AddNode(new ItemController(null,oldsceneryentry.ExtraData));
            }
            AddMenuSeparator();
            AddMenu("Export as OBJ",Menu_Export_OBJ);
            AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Scenery ({0})",oldsceneryentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
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

        private void Menu_Export_COLLADA()
        {
            if (MessageBox.Show("Exporting to COLLADA is experimental.\nTexture information will not be exported.\n\nContinue anyway?","Export as COLLADA",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(oldsceneryentry.ToCOLLADA(),FileFilters.COLLADA,FileFilters.Any);
        }
    }
}
