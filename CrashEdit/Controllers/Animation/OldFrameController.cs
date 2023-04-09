using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldFrameController : Controller
    {
        public OldFrameController(ProtoAnimationEntryController protoanimationentrycontroller, OldFrame oldframe)
        {
            ProtoAnimationEntryController = protoanimationentrycontroller;
            OldAnimationEntryController = null;
            OldFrame = oldframe;
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ_Game);
            AddMenu ("Export as OBJ (processed geometry)", Menu_Export_OBJ_Processed);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public OldFrameController(OldAnimationEntryController oldanimationentrycontroller, OldFrame oldframe)
        {
            ProtoAnimationEntryController = null;
            OldAnimationEntryController = oldanimationentrycontroller;
            OldFrame = oldframe;
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ_Game);
            AddMenu ("Export as OBJ (processed geometry)", Menu_Export_OBJ_Processed);
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

        private void Menu_Export_OBJ_Game()
        {
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?", "Export as OBJ", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(this.ToGameOBJ(), FileFilters.OBJ, FileFilters.Any);
        }
        
        private void Menu_Export_OBJ_Processed()
        {
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?", "Export as OBJ", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(this.ToProcessedOBJ(), FileFilters.OBJ, FileFilters.Any);
        }
        class ProcessedEntries
        {
            public float VX;
            public float VY;
            public float VZ;
            public float NX;
            public float NY;
            public float NZ;
            public OldSceneryColor color;
        }
        
        class GameEntries
        {
            public OldFrameVertex vertex;
            public OldSceneryColor color;
        }
        
        public byte[] ToProcessedOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    EntryController controller = OldAnimationEntryController != null ? OldAnimationEntryController : ProtoAnimationEntryController;
                    var model = controller.NSF.GetEntry<OldModelEntry>(OldFrame.ModelEID);

                    // build scale
                    float scaleX = model.ScaleX / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleY = model.ScaleY / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleZ = model.ScaleZ / (GameScales.ModelC1 * GameScales.AnimC1);

                    var list = new List <ProcessedEntries> ();

                    foreach (OldModelPolygon polygon in model.Polygons)
                    {
                        OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                        
                        // add the three polygons to the list
                        int [] vertices = new [] {polygon.VertexA / 6, polygon.VertexB / 6, polygon.VertexC / 6};

                        foreach (int vertex in vertices)
                        {
                            ProcessedEntries entry = new ProcessedEntries ();
                            
                            if (str is OldSceneryColor color)
                                entry.color = color;

                            entry.VX = (OldFrame.Vertices [vertex].X - OldFrame.XOffset) * scaleX;
                            entry.VY = (OldFrame.Vertices [vertex].Y - OldFrame.YOffset) * scaleY;
                            entry.VZ = (OldFrame.Vertices [vertex].Z - OldFrame.ZOffset) * scaleZ;
                            entry.NX = OldFrame.Vertices [vertex].NormalX / 127F;
                            entry.NY = OldFrame.Vertices [vertex].NormalY / 127F;
                            entry.NZ = OldFrame.Vertices [vertex].NormalZ / 127F;
                            
                            list.Add (entry);
                        }
                    }
                    
                    obj.WriteLine("# Vertices");
                    foreach (ProcessedEntries entry in list)
                    {
                        obj.WriteLine("v {0} {1} {2} {3} {4} {5}",
                            entry.VX.ToString(CultureInfo.InvariantCulture),
                            entry.VY.ToString(CultureInfo.InvariantCulture),
                            entry.VZ.ToString(CultureInfo.InvariantCulture),
                            (entry.color.R / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.G / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.B / 255F).ToString(CultureInfo.InvariantCulture)
                        );
                    }
                    
                    foreach (ProcessedEntries vertex in list)
                    {
                        obj.WriteLine("vn {0} {1} {2}",
                            vertex.NX.ToString(CultureInfo.InvariantCulture),
                            vertex.NY.ToString(CultureInfo.InvariantCulture),
                            vertex.NZ.ToString(CultureInfo.InvariantCulture)
                        );
                    }
                    
                    obj.WriteLine();
                    obj.WriteLine("# Triangles");
                    
                    for (int i = 0; i < list.Count; i += 3)
                    {
                        obj.WriteLine("f {0}//{0} {1}//{1} {2}//{2}", i + 1, i + 2, i + 3);
                    }
                }
                return stream.ToArray();
            }
        }
        /// <summary>
        /// Generates a OBJ file with the model's information as close as to what the original frame is
        ///
        /// Due to OBJ format we cannot include all the required colors for each face
        /// so it looks way out of place.
        ///
        /// TODO: MAYBE IMPLEMENT AN FBX EXPORT OR SOMETHING ELSE THAT IS A BIT MORE FLEXIBLE?
        ///
        /// This function resides here because access to GameScales is required, and the Frame object does not have access to it
        /// a good improvement might be to move this there
        /// </summary>
        /// <returns></returns>
        public byte[] ToGameOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    EntryController controller = OldAnimationEntryController != null ? OldAnimationEntryController : ProtoAnimationEntryController;
                    var model = controller.NSF.GetEntry<OldModelEntry>(OldFrame.ModelEID);
                    
                    // build scale
                    float scaleX = model.ScaleX / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleY = model.ScaleY / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleZ = model.ScaleZ / (GameScales.ModelC1 * GameScales.AnimC1);

                    var list = new List <GameEntries> ();
                    
                    foreach (OldFrameVertex vertex in OldFrame.Vertices)
                    {
                        list.Add (new GameEntries { vertex = vertex });
                    }
                    
                    // update color information
                    foreach (OldModelPolygon polygon in model.Polygons)
                    {
                        OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];

                        if (str is not OldSceneryColor color)
                            continue;

                        list [polygon.VertexA / 6].color = color;
                        list [polygon.VertexB / 6].color = color;
                        list [polygon.VertexC / 6].color = color;
                    }
                    
                    obj.WriteLine("# Vertices");
                    foreach (GameEntries entry in list)
                    {
                        obj.WriteLine("v {0} {1} {2} {3} {4} {5}",
                            ((entry.vertex.X - OldFrame.XOffset) * scaleX).ToString(CultureInfo.InvariantCulture),
                            ((entry.vertex.Y - OldFrame.YOffset) * scaleY).ToString(CultureInfo.InvariantCulture),
                            ((entry.vertex.Z - OldFrame.ZOffset) * scaleZ).ToString(CultureInfo.InvariantCulture),
                            (entry.color.R / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.G / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.B / 255F).ToString(CultureInfo.InvariantCulture)
                        );
                    }
                    
                    foreach (OldFrameVertex vertex in OldFrame.Vertices)
                    {
                        obj.WriteLine("vn {0} {1} {2}",
                            (vertex.NormalX/127F).ToString(CultureInfo.InvariantCulture),
                            (vertex.NormalY/127F).ToString(CultureInfo.InvariantCulture),
                            (vertex.NormalZ/127F).ToString(CultureInfo.InvariantCulture)
                        );
                    }
                    obj.WriteLine();
                    obj.WriteLine("# Triangles");
                    foreach (OldModelPolygon polygon in model.Polygons)
                    {
                        obj.WriteLine("f {0}//{0} {1}//{1} {2}//{2}", polygon.VertexA / 6 + 1,polygon.VertexB / 6 + 1,polygon.VertexC / 6 + 1);
                    }
                }
                return stream.ToArray();
            }
        }
    }
}
