using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class FrameController : Controller
    {
        public FrameController(AnimationEntryController animationentrycontroller, Frame frame)
        {
            AnimationEntryController = animationentrycontroller;
            Frame = frame;
            InvalidateNode();
            InvalidateNodeImage();
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ_Game);
            AddMenu ("Export as OBJ (processed geometry)", Menu_Export_OBJ_Processed);
        }

        private void Menu_Export_OBJ_Processed()
        {
            FileUtil.SaveFile(this.ToProcessedOBJ(), FileFilters.OBJ, FileFilters.Any);
        }

        private void Menu_Export_OBJ_Game ()
        {
            FileUtil.SaveFile(this.ToGameOBJ(), FileFilters.OBJ, FileFilters.Any);
        }
        
        class ProcessedEntries
        {
            public float VX;
            public float VY;
            public float VZ;
            public SceneryColor color;
        }
        
        class GameEntries
        {
            public FrameVertex vertex;
            public SceneryColor color;
        }
        
        /// <summary>
        /// Generates a OBJ file with the model's information ready to be displayed in any other 3D software.
        /// It includes extra geometry to ensure vertex colors do look properly
        ///
        /// This function resides here because access to GameScales is required, and the Frame object does not have access to it
        /// a good improvement would be to move this there
        /// </summary>
        /// <returns></returns>
        public byte[] ToProcessedOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    var model = this.AnimationEntryController.NSF.GetEntry<ModelEntry>(Frame.ModelEID);

                    obj.WriteLine("# Vertices");

                    var vertices = Frame.MakeVertices (this.AnimationEntryController.NSF);
                    var list = new List <ProcessedEntries>();

                    // build scale
                    float scaleX = model.ScaleX / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleY = model.ScaleY / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleZ = model.ScaleZ / (GameScales.ModelC1 * GameScales.AnimC1);
                    
                    foreach (ModelTransformedTriangle tri in model.Triangles)
                    {
                        bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                        bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;

                        for (int i = 0; i < 3; ++i)
                        {
                            var v_n = !flip ? i : 2 - i;
                            FrameVertex vert = vertices [tri.Vertex [v_n] + Frame.SpecialVertexCount];
                            SceneryColor color = model.Colors [tri.Color [v_n]];
                            
                            // calculate positions
                            list.Add (new ProcessedEntries
                            {
                                VX = (vert.X + (Frame.XOffset / 4F)) * scaleX,
                                VY = (vert.Z + (Frame.YOffset / 4F)) * scaleY,
                                VZ = (vert.Y + (Frame.ZOffset / 4F)) * scaleZ,
                                color = color
                            });
                        }
                    }

                    foreach (ProcessedEntries entry in list)
                    {
                        obj.WriteLine(
                            "v {0} {1} {2} {3} {4} {5}",
                            entry.VX.ToString(CultureInfo.InvariantCulture),
                            entry.VY.ToString(CultureInfo.InvariantCulture),
                            entry.VZ.ToString(CultureInfo.InvariantCulture),
                            (entry.color.Red / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.Green / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.Blue / 255F).ToString(CultureInfo.InvariantCulture));
                    }

                    obj.WriteLine ("");
                    obj.WriteLine ("# Triangles");
                    
                    for (int i = 0; i < list.Count; i+=3)
                    {
                        obj.WriteLine ("f {0} {1} {2}", i + 1, i + 2, i + 3);
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
        /// a good improvement would be to move this there
        /// </summary>
        /// <returns></returns>
        public byte[] ToGameOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    var model = this.AnimationEntryController.NSF.GetEntry<ModelEntry>(Frame.ModelEID);

                    obj.WriteLine("# Vertices");

                    var vertices = Frame.MakeVertices (this.AnimationEntryController.NSF);
                    var list = new List <GameEntries>();

                    // build scale
                    float scaleX = model.ScaleX / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleY = model.ScaleY / (GameScales.ModelC1 * GameScales.AnimC1);
                    float scaleZ = model.ScaleZ / (GameScales.ModelC1 * GameScales.AnimC1);

                    foreach (FrameVertex vertex in vertices)
                    {
                        list.Add (new GameEntries { vertex = vertex });
                    }
                    
                    foreach (ModelTransformedTriangle tri in model.Triangles)
                    {
                        bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                        bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;

                        for (int i = 0; i < 3; ++i)
                        {
                            var v_n = !flip ? i : 2 - i;
                            list [tri.Vertex [v_n] + Frame.SpecialVertexCount].color = model.Colors [tri.Color [v_n]];
                        }
                    }

                    foreach (GameEntries entry in list)
                    {
                        obj.WriteLine(
                            "v {0} {1} {2} {3} {4} {5}",
                            ((entry.vertex.X + (Frame.XOffset / 4F)) * scaleX).ToString(CultureInfo.InvariantCulture),
                            ((entry.vertex.Z + (Frame.YOffset / 4F)) * scaleY).ToString(CultureInfo.InvariantCulture),
                            ((entry.vertex.Y + (Frame.ZOffset / 4F)) * scaleZ).ToString(CultureInfo.InvariantCulture),
                            (entry.color.Red / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.Green / 255F).ToString(CultureInfo.InvariantCulture),
                            (entry.color.Blue / 255F).ToString(CultureInfo.InvariantCulture));
                    }
                    
                    obj.WriteLine();
                    obj.WriteLine("# Triangles");
                    
                    foreach (ModelTransformedTriangle tri in model.Triangles)
                    {
                        bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                        bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
                        int [] verts = new int[3];

                        for (int i = 0; i < 3; ++i)
                        {
                            var v_n = !flip ? i : 2 - i;
                            verts [i] = tri.Vertex [v_n] + Frame.SpecialVertexCount;
                        }

                        obj.WriteLine("f {0} {1} {2}", verts[0] + 1, verts[1] + 1, verts[2] + 1);
                    }
                }

                return stream.ToArray();
            }
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
            return new AnimationEntryViewer(AnimationEntryController.NSF, AnimationEntryController.AnimationEntry.EID, AnimationEntryController.AnimationEntry.Frames.IndexOf(Frame));
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
