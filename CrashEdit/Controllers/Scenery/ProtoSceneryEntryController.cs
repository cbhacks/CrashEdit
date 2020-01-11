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
            TextureChunk[] texturechunks = new TextureChunk[BitConv.FromInt32(ProtoSceneryEntry.Info,0x18)];
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturechunks[i] = FindEID<TextureChunk>(BitConv.FromInt32(ProtoSceneryEntry.Info,0x20 + i * 4));
            }
            return new UndockableControl(new ProtoSceneryEntryViewer(ProtoSceneryEntry,texturechunks));
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
            List<OldSceneryVertex> vertices = new List<OldSceneryVertex>();
            foreach (var protop in ProtoSceneryEntry.Polygons)
            {
                byte r,g,b;
                OldModelStruct str = ProtoSceneryEntry.Structs[protop.Texture];
                if (str is OldSceneryTexture tex)
                {
                    r = tex.R;
                    g = tex.G;
                    b = tex.B;
                }
                else if (str is OldSceneryColor col)
                {
                    r = col.R;
                    g = col.G;
                    b = col.B;
                }
                else
                {
                    r = 0x7F;
                    g = 0x7F;
                    b = 0x7F;
                }
                polygons.Add(new OldSceneryPolygon(polygons.Count*3,polygons.Count*3+1,polygons.Count*3+2,protop.Texture,(byte)protop.Page,0,0));
                var va = ProtoSceneryEntry.Vertices[protop.VertexA];
                var vb = ProtoSceneryEntry.Vertices[protop.VertexB];
                var vc = ProtoSceneryEntry.Vertices[protop.VertexC];
                vertices.Add(new OldSceneryVertex((short)((short)(va.X/8.0)*8),(short)((short)(va.Y/8.0)*8),(short)((short)(va.Z/8.0)*8),r,g,b,false));
                vertices.Add(new OldSceneryVertex((short)((short)(vb.X/8.0)*8),(short)((short)(vb.Y/8.0)*8),(short)((short)(vb.Z/8.0)*8),r,g,b,false));
                vertices.Add(new OldSceneryVertex((short)((short)(vc.X/8.0)*8),(short)((short)(vc.Y/8.0)*8),(short)((short)(vc.Z/8.0)*8),r,g,b,false));
            }
            for (int i = 0; i < vertices.Count; ++i)
            {
                OldSceneryVertex basevertex = vertices[i];
                for (int j = vertices.Count-1; j > i; --j)
                {
                    OldSceneryVertex testvertex = vertices[j];
                    if (basevertex.Red == testvertex.Red &&
                        basevertex.Green == testvertex.Green &&
                        basevertex.Blue == testvertex.Blue &&
                        basevertex.X == testvertex.X &&
                        basevertex.Y == testvertex.Y &&
                        basevertex.Z == testvertex.Z)
                    {
                        for (int k = 0; k < polygons.Count; ++k)
                        {
                            var poly = polygons[k];
                            if (poly.VertexA == j)
                                poly.VertexA = i;
                            else if (poly.VertexA > j)
                                --poly.VertexA;
                            if (poly.VertexB == j)
                                poly.VertexB = i;
                            else if (poly.VertexB > j)
                                --poly.VertexB;
                            if (poly.VertexC == j)
                                poly.VertexC = i;
                            else if (poly.VertexC > j)
                                --poly.VertexC;
                            polygons[k] = poly;
                        }
                        vertices.RemoveAt(j);
                    }
                }
            }
            OldSceneryEntry newworld = new OldSceneryEntry(ProtoSceneryEntry.Info,polygons,vertices,ProtoSceneryEntry.Structs,null,ProtoSceneryEntry.EID);
            BitConv.ToInt32(newworld.Info,0x10,vertices.Count);
            FileUtil.SaveFile(newworld.Save(), FileFilters.NSEntry, FileFilters.Any);
        }
    }
}
