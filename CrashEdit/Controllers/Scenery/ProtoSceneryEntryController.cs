using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoSceneryEntryController : EntryController
    {
        private ProtoSceneryEntry protosceneryentry;

        public ProtoSceneryEntryController(EntryChunkController entrychunkcontroller,ProtoSceneryEntry protosceneryentry) : base(entrychunkcontroller,protosceneryentry)
        {
            this.protosceneryentry = protosceneryentry;
            if (protosceneryentry.ExtraData != null)
            {
                AddNode(new ItemController(null,protosceneryentry.ExtraData));
            }
            AddMenuSeparator();
            AddMenu("Export as OBJ",Menu_Export_OBJ);
            AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Prototype Scenery ({0})",protosceneryentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new ProtoSceneryEntryViewer(protosceneryentry));
        }

        public ProtoSceneryEntry ProtoSceneryEntry
        {
            get { return protosceneryentry; }
        }

        private void Menu_Export_OBJ()
        {
            if (MessageBox.Show("Exporting to OBJ is experimental.\nTexture information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(protosceneryentry.ToOBJ(),FileFilters.OBJ,FileFilters.Any);
        }

        private void Menu_Export_COLLADA()
        {
            if (MessageBox.Show("Exporting to COLLADA is experimental.\nTexture information will not be exported.\n\nContinue anyway?","Export as COLLADA",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(protosceneryentry.ToCOLLADA(),FileFilters.COLLADA,FileFilters.Any);
        }
    }
}
