using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class NewSceneryEntryController : EntryController
    {
        public NewSceneryEntryController(EntryChunkController entrychunkcontroller,NewSceneryEntry sceneryentry) : base(entrychunkcontroller,sceneryentry)
        {
            SceneryEntry = sceneryentry;
            AddMenuSeparator();
            AddMenu("Export as Wavefront OBJ",Menu_Export_OBJ);
            AddMenu("Export as Stanford PLY",Menu_Export_PLY);
            //AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            AddMenu("Fix coords imported from Crash 2", Menu_Fix_WGEOv2);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Scenery ({0})",SceneryEntry.EName);
            Node.ImageKey = "blueb";
            Node.SelectedImageKey = "blueb";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new NewSceneryEntryViewer(SceneryEntry));
        }

        public NewSceneryEntry SceneryEntry { get; }

        private void Menu_Export_OBJ()
        {
            if (MessageBox.Show("Exporting to Wavefront OBJ (.obj) is experimental.\nTexture and color information will not be exported.\n\nContinue anyway?", "Export as OBJ", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(SceneryEntry.ToOBJ(), FileFilters.OBJ, FileFilters.Any);
        }

        private void Menu_Export_PLY()
        {
            if (MessageBox.Show("Exporting to Stanford PLY (.ply) is experimental.\nTexture information will not be exported.\n\nContinue anyway?", "Export as PLY", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(SceneryEntry.ToPLY(), FileFilters.PLY, FileFilters.Any);
        }

        /*private void Menu_Export_COLLADA()
        {
            if (MessageBox.Show("Exporting to COLLADA (.dae) is experimental.\nTexture and quad information will not be exported.\n\nContinue anyway?", "Export as OBJ", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(sceneryentry.ToCOLLADA(), FileFilters.COLLADA, FileFilters.Any);
        }*/

        private void Menu_Fix_WGEOv2()
        {
            for (int i = 0;i < SceneryEntry.Vertices.Count;i++)
            {
                NewSceneryVertex vtx = SceneryEntry.Vertices[i];
                SceneryEntry.Vertices[i] = new NewSceneryVertex(
                    (vtx.X + 0x800) & 0xFFF,
                    (vtx.Y + 0x800) & 0xFFF,
                    (vtx.Z + 0x800) & 0xFFF,
                    vtx.UnknownX,
                    vtx.UnknownY,
                    vtx.UnknownZ
                );
            }

            SceneryEntry.XOffset -= 0x8000;
            SceneryEntry.YOffset -= 0x8000;
            SceneryEntry.ZOffset -= 0x8000;
        }
    }
}
