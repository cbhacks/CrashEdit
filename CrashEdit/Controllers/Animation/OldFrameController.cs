using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Crash;
using System.Windows.Forms;
using CrashEdit.Exporters;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class OldFrameController : Controller
    {
        public OldFrameController(ProtoAnimationEntryController protoanimationentrycontroller, OldFrame oldframe)
        {
            ProtoAnimationEntryController = protoanimationentrycontroller;
            OldAnimationEntryController = null;
            OldFrame = oldframe;
            AddMenu ("Export as OBJ", Menu_Export_OBJ);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public OldFrameController(OldAnimationEntryController oldanimationentrycontroller, OldFrame oldframe)
        {
            ProtoAnimationEntryController = null;
            OldAnimationEntryController = oldanimationentrycontroller;
            OldFrame = oldframe;
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.FrameController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            TabControl tbcTabs = new TabControl() { Dock = DockStyle.Fill };

            OldFrameBox framebox = new OldFrameBox(this);
            framebox.Dock = DockStyle.Fill;
            EntryController controller = OldAnimationEntryController != null ? OldAnimationEntryController : ProtoAnimationEntryController;
            Entry entry = controller.Entry;
            OldAnimationEntryViewer viewerbox = new OldAnimationEntryViewer(controller.NSF, entry.EID,
                                                                            entry is ProtoAnimationEntry pentry ? pentry.Frames.IndexOf(OldFrame)
                                                                                                                : (entry as OldAnimationEntry).Frames.IndexOf(OldFrame))
            { Dock = DockStyle.Fill };

            TabPage edittab = new TabPage("Editor");
            edittab.Controls.Add(framebox);
            TabPage viewertab = new TabPage("Viewer");
            viewertab.Controls.Add(new UndockableControl(viewerbox));

            tbcTabs.TabPages.Add(viewertab);
            tbcTabs.TabPages.Add(edittab);
            tbcTabs.SelectedTab = viewertab;

            return tbcTabs;
        }

        public ProtoAnimationEntryController ProtoAnimationEntryController { get; }
        public OldAnimationEntryController OldAnimationEntryController { get; }
        public OldFrame OldFrame { get; }

        private void Menu_Export_OBJ()
        {
            FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any);
            ToOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }

        public void ToOBJ(string path, string modelname)
        {
            var exporter = new OBJExporter ();
            EntryController controller = OldAnimationEntryController != null ? OldAnimationEntryController : ProtoAnimationEntryController;
            var model = controller.NSF.GetEntry<OldModelEntry>(OldFrame.ModelEID);
            var offset = new Vector3 (OldFrame.XOffset, OldFrame.YOffset, OldFrame.ZOffset);
            var scale = new Vector3 (model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);

            // detect how many textures are used an ther eids to prepare the image
            Dictionary <int, int> textureEIDs = new ();

            foreach (OldModelStruct str in model.Structs)
            {
                if (str is not OldModelTexture tex)
                    continue;

                if (textureEIDs.ContainsKey (tex.EID))
                    continue;

                textureEIDs [tex.EID] = textureEIDs.Count;
            }

            Dictionary <string, TexInfoUnpacked> objTranslate = new Dictionary <string, TexInfoUnpacked> ();
            
            foreach (OldModelPolygon polygon in model.Polygons)
            {
                string material = null;
                Vector2? uv1 = null, uv2 = null, uv3 = null;
                OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                OldFrameVertex ov1 = OldFrame.Vertices [polygon.VertexA / 6];
                OldFrameVertex ov2 = OldFrame.Vertices [polygon.VertexB / 6];
                OldFrameVertex ov3 = OldFrame.Vertices [polygon.VertexC / 6];

                Vector3 v1 = new Vector3 (ov1.X, ov1.Y, ov1.Z);
                Vector3 v2 = new Vector3 (ov2.X, ov2.Y, ov2.Z);
                Vector3 v3 = new Vector3 (ov3.X, ov3.Y, ov3.Z);
                Vector3 color = Vector3.Zero;

                if (str is OldModelTexture t)
                {
                    color = new Vector3 (t.R, t.G, t.B) / 255F;
                    
                    // add the texture to the list too
                    material = objTranslate.FirstOrDefault (x => 
                        x.Value.color == t.ColorMode &&
                        x.Value.blend == t.BlendMode &&
                        x.Value.clutx == t.ClutX &&
                        x.Value.cluty == t.ClutY &&
                        x.Value.page == textureEIDs [t.EID]
                    ).Key;
                    
                    if (material is null)
                    {
                        var texinfo = new TexInfoUnpacked(
                            true, color: t.ColorMode, blend: t.BlendMode,
                            clutx: t.ClutX, cluty: t.ClutY,
                            face: Convert.ToInt32(t.N),
                            page: textureEIDs[t.EID]
                        );

                        var tpag = controller.NSF.GetEntry <TextureChunk> (t.EID);
                        Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);
                        
                        // the material name changes
                        material = exporter.AddTexture (((int) texinfo).ToString ("X8"), texture);
                        
                        // add it to the lookup table too
                        objTranslate [material] = texinfo;
                    }

                    Vector2 texsize = t.ColorMode switch
                    {
                        0 => new Vector2 (1024, 128),
                        1 => new Vector2 (512, 128),
                        _ => new Vector2 (256, 128)
                    };
                    
                    uv3 = new Vector2 (t.U3 / texsize.X, (128 - t.V3) / texsize.Y);
                    uv2 = new Vector2 (t.U2 / texsize.X, (128 - t.V2) / texsize.Y);
                    uv1 = new Vector2 (t.U1 / texsize.X, (128 - t.V1) / texsize.Y);
                }
                else if(str is OldSceneryColor c)
                {
                    color = new Vector3 (c.R, c.G, c.B) / 255F;
                }

                exporter.AddFace (
                    (v1 + offset) * scale,
                    (v2 + offset) * scale,
                    (v3 + offset) * scale,
                    color, color, color,
                    material,
                    uv1, uv2, uv3
                );
            }

            exporter.Export (path, modelname);
        }
    }
}
