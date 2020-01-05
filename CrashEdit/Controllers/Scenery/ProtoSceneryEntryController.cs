using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoSceneryEntryController : EntryController
    {
        public ProtoSceneryEntryController(EntryChunkController entrychunkcontroller,ProtoSceneryEntry protosceneryentry) : base(entrychunkcontroller,protosceneryentry)
        {
            ProtoSceneryEntry = protosceneryentry;
            AddMenuSeparator();
            AddMenu("Export as OBJ",Menu_Export_OBJ);
            AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            AddMenu("Export as Crash 1 WGEO", Menu_ExportAsC1);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.ProtoSceneryEntryController_Text,ProtoSceneryEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "blueb";
            Node.SelectedImageKey = "blueb";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new ProtoSceneryEntryViewer(ProtoSceneryEntry));
        }

        public ProtoSceneryEntry ProtoSceneryEntry { get; }

        private void Menu_Export_OBJ()
        {
            if (MessageBox.Show("Exporting to OBJ is experimental.\nTexture information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(ProtoSceneryEntry.ToOBJ(),FileFilters.OBJ,FileFilters.Any);
        }

        private void Menu_Export_COLLADA()
        {
            if (MessageBox.Show("Exporting to COLLADA is experimental.\nTexture information will not be exported.\n\nContinue anyway?","Export as COLLADA",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(ProtoSceneryEntry.ToCOLLADA(),FileFilters.COLLADA,FileFilters.Any);
        }

        private void Menu_ExportAsC1()
        {
            List<OldSceneryPolygon> polygons = new List<OldSceneryPolygon>();
            foreach (var protop in ProtoSceneryEntry.Polygons)
            {
                polygons.Add(new OldSceneryPolygon(protop.VertexA,protop.VertexB,protop.VertexC,protop.Texture,(byte)protop.Page,0,0));
            }
            List<OldSceneryVertex> vertices = new List<OldSceneryVertex>();
            foreach (var protov in ProtoSceneryEntry.Vertices)
            {
                vertices.Add(new OldSceneryVertex((short)((short)(protov.X/8.0)*8),(short)((short)(protov.Y/8.0)*8),(short)((short)(protov.Z/8.0)*8),0x7F,0x7F,0x7F,false));
            }
            OldSceneryEntry newworld = new OldSceneryEntry(ProtoSceneryEntry.Info,polygons,vertices,null,ProtoSceneryEntry.EID);
            FileUtil.SaveFile(newworld.Save(), FileFilters.NSEntry, FileFilters.Any);
        }
    }
}
