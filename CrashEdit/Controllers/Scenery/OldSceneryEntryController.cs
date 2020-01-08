using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldSceneryEntryController : EntryController
    {
        public OldSceneryEntryController(EntryChunkController entrychunkcontroller,OldSceneryEntry oldsceneryentry) : base(entrychunkcontroller,oldsceneryentry)
        {
            OldSceneryEntry = oldsceneryentry;
            if (oldsceneryentry.ExtraData != null)
            {
                AddNode(new ItemController(null,oldsceneryentry.ExtraData));
            }
            AddMenuSeparator();
            AddMenu("Export as OBJ",Menu_Export_OBJ);
            AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldSceneryEntryController_Text,OldSceneryEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "blueb";
            Node.SelectedImageKey = "blueb";
        }

        protected override Control CreateEditor()
        {
            TextureChunk[] texturechunks = new TextureChunk[BitConv.FromInt32(OldSceneryEntry.Info,0x18)];
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturechunks[i] = FindEID<TextureChunk>(BitConv.FromInt32(OldSceneryEntry.Info,0x20 + i * 4));
            }
            return new UndockableControl(new OldSceneryEntryViewer(OldSceneryEntry,texturechunks));
        }

        public OldSceneryEntry OldSceneryEntry { get; }

        private void Menu_Export_OBJ()
        {
            if (MessageBox.Show("Exporting to OBJ is experimental.\nTexture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(OldSceneryEntry.ToOBJ(),FileFilters.OBJ,FileFilters.Any);
        }

        private void Menu_Export_COLLADA()
        {
            if (MessageBox.Show("Exporting to COLLADA is experimental.\nTexture information will not be exported.\n\nContinue anyway?","Export as COLLADA",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(OldSceneryEntry.ToCOLLADA(),FileFilters.COLLADA,FileFilters.Any);
        }
    }
}
