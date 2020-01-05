using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SceneryEntryController : EntryController
    {
        public SceneryEntryController(EntryChunkController entrychunkcontroller,SceneryEntry sceneryentry) : base(entrychunkcontroller,sceneryentry)
        {
            SceneryEntry = sceneryentry;
            AddMenuSeparator();
            AddMenu("Export as Wavefront OBJ",Menu_Export_OBJ);
            AddMenu("Export as Stanford PLY",Menu_Export_PLY);
            //AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            AddMenu("Fix coords imported from Crash 3", Menu_Fix_WGEOv3);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.SceneryEntryController_Text,SceneryEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "blueb";
            Node.SelectedImageKey = "blueb";
        }

        protected override Control CreateEditor()
        {
            TextureChunk[] texturechunks = new TextureChunk[BitConv.FromInt32(SceneryEntry.Info,0x28)];
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturechunks[i] = FindEID<TextureChunk>(BitConv.FromInt32(SceneryEntry.Info,0x2C+i*4));
            }
            return new UndockableControl(new SceneryEntryViewer(SceneryEntry,texturechunks));
        }

        public SceneryEntry SceneryEntry { get; }

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

        private void Menu_Fix_WGEOv3()
        {
            for (int i = 0;i < SceneryEntry.Vertices.Count;i++)
            {
                SceneryVertex vtx = SceneryEntry.Vertices[i];
                SceneryEntry.Vertices[i] = new SceneryVertex(
                    (vtx.X & 0xFFF) - 0x800,
                    (vtx.Y & 0xFFF) - 0x800,
                    (vtx.Z & 0xFFF) - 0x800,
                    vtx.UnknownX,
                    vtx.UnknownY,
                    vtx.UnknownZ
                );
            }

            SceneryEntry.XOffset += 0x8000;
            SceneryEntry.YOffset += 0x8000;
            SceneryEntry.ZOffset += 0x8000;
        }
    }
}
