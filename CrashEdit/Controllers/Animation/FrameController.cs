using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using Crash;
using System.Windows.Forms;
using CrashEdit.Exporters;
using OpenTK;

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
            AddMenu ("Export as OBJ", Menu_Export_OBJ);
        }

        private void Menu_Export_OBJ ()
        {
            FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any);
            ToOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }
        
        /// <summary>
        /// Exports the model to the OBJ file format ready to be used with other software
        ///
        /// TODO: MAYBE IMPLEMENT AN FBX EXPORT OR SOMETHING ELSE THAT IS A BIT MORE FLEXIBLE?
        ///
        /// This function resides here because access to GameScales is required, and the Frame object does not have access to it
        /// a good improvement might be to move this there
        /// </summary>
        /// <returns></returns>
        public void ToOBJ (string path, string modelname)
        {
            var exporter = new OBJExporter ();
            var model = this.AnimationEntryController.NSF.GetEntry<ModelEntry>(Frame.ModelEID);
            var vertices = Frame.MakeVertices (this.AnimationEntryController.NSF);
            var offset = new Vector3 (Frame.XOffset, Frame.YOffset, Frame.ZOffset) / 4F;
            var scale = new Vector3 (model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
            
            // detect how many textures are used and their eids to prepare the image
            Dictionary <int, int> textureEIDs = new ();

            for (int i = 0; i < model.TPAGCount; i++)
            {
                int tpag_eid = model.GetTPAG (i);

                if (textureEIDs.ContainsKey (tpag_eid))
                    continue;

                textureEIDs.Add (tpag_eid, textureEIDs.Count);
            }
            
            // once we have the textureEIDs we know what has to be loaded where
            // these textures are 128 pixels of height
            // and the game loads every texture used and keeps a lookup table of the texture "index"
            
            // depending on where in the model it's going to be drawn, the texture is treated differently
            // (4 bits per pixel, 8 bits per pixel, 16 bits per pixel)
            // that information is inside each triangle of the frame
            // so the same texture can be treated differently
            // try to build an atlas of sorts with all the information
            Dictionary <string, TexInfoUnpacked> objTranslate = new Dictionary <string, TexInfoUnpacked> ();
            
            // iterate all the triangles, get the texture modes and build information about those
            foreach (var tri in model.Triangles)
            {
                var info = ProcessTextureInfoC2 (tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures);
                string material = null;
                Vector2? uv1 = null, uv2 = null, uv3 = null;
                bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
                
                // parse the texture and add it to the exporter
                if (info.Item1 && info.Item2 is not null)
                {
                    var value = info.Item2.Value;
                    
                    material = objTranslate.FirstOrDefault (x => 
                        x.Value.color == value.ColorMode &&
                        x.Value.blend == value.BlendMode &&
                        x.Value.clutx == value.ClutX &&
                        x.Value.cluty == value.ClutY &&
                        x.Value.page == textureEIDs [model.GetTPAG (value.Page)]
                    ).Key;

                    // ignore the texinfo if there's already a texture with the exact same settings stored
                    if (material is null)
                    {
                        var texinfo = new TexInfoUnpacked (
                            true, color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                            page: textureEIDs [model.GetTPAG (value.Page)]
                        );

                        var tpag = this.AnimationEntryController.NSF.GetEntry <TextureChunk> (textureEIDs.First (x => x.Value == texinfo.page).Key);

                        Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

                        // the material name changes
                        material = exporter.AddTexture (((int) texinfo).ToString ("X8"), texture);
                        
                        // add it to the lookup table too
                        objTranslate [material] = texinfo;
                    }

                    Vector2 texsize = value.ColorMode switch
                    {
                        0 => new Vector2 (1024, 128),
                        1 => new Vector2 (512, 128),
                        _ => new Vector2 (256, 128)
                    };
                    
                    uv2 = new Vector2 (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);

                    if ((tri.Type != 2 && !flip) || (tri.Type == 2 && tri.Subtype == 1))
                    {
                        uv1 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                        uv3 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                    }
                    else
                    {
                        uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                        uv1 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                    }
                }

                // add the face
                SceneryColor fc1 = model.Colors [tri.Color [!flip ? 0 : 2]];
                SceneryColor fc2 = model.Colors [tri.Color [!flip ? 1 : 1]];
                SceneryColor fc3 = model.Colors [tri.Color [!flip ? 2 : 0]];
                FrameVertex fv1 = vertices [tri.Vertex [!flip ? 0 : 2] + Frame.SpecialVertexCount];
                FrameVertex fv2 = vertices [tri.Vertex [!flip ? 1 : 1] + Frame.SpecialVertexCount];
                FrameVertex fv3 = vertices [tri.Vertex [!flip ? 2 : 0] + Frame.SpecialVertexCount];
                Vector3 v1 = new Vector3 (fv1.X, fv1.Z, fv1.Y);
                Vector3 v2 = new Vector3 (fv2.X, fv2.Z, fv2.Y);
                Vector3 v3 = new Vector3 (fv3.X, fv3.Z, fv3.Y);
                Vector3 c1 = new Vector3 (fc1.Red, fc1.Green, fc1.Blue) / 255f;
                Vector3 c2 = new Vector3 (fc2.Red, fc2.Green, fc2.Blue) / 255f;
                Vector3 c3 = new Vector3 (fc3.Red, fc3.Green, fc3.Blue) / 255f;
                
                exporter.AddFace (
                    (v1 + offset) * scale,
                    (v2 + offset) * scale,
                    (v3 + offset) * scale,
                    c1, c2, c3,
                    material,
                    uv1, uv2, uv3
                );
            }
            
            exporter.Export (path, modelname);
        }
        
        /// <summary>
        /// TODO: THIS IS A DUPLICATED OF GLViewer:899, MOVE IT SOMEWHERE ELSE WHERE BOTH HAVE ACCESS TO IT
        /// </summary>
        /// <param name="in_tex_id"></param>
        /// <param name="animated"></param>
        /// <param name="textures"></param>
        /// <param name="animated_textures"></param>
        /// <returns></returns>
        protected Tuple<bool, ModelTexture?> ProcessTextureInfoC2(int in_tex_id, bool animated, IList<ModelTexture> textures, IList<ModelExtendedTexture> animated_textures)
        {
            if (in_tex_id != 0 || animated)
            {
                ModelTexture? info_temp = null;
                int tex_id = in_tex_id - 1;
                if (animated)
                {
                    if (++tex_id >= animated_textures.Count)
                    {
                        return new(false, null);
                    }
                    var anim = animated_textures[tex_id];
                    // check if it's an untextured polygon
                    if (anim.Offset != 0)
                    {
                        tex_id = anim.Offset - 1;
                        if (anim.IsLOD)
                        {
                            tex_id += anim.LOD0; // we only render closest LOD for now
                        }
                        else
                        {
                            tex_id += (int)((0 / 2 / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                            if (anim.Leap)
                            {
                                anim = animated_textures[++tex_id];
                                tex_id = anim.Offset - 1 + anim.LOD0;
                            }
                        }
                        if (tex_id >= textures.Count)
                        {
                            return new(false, null);
                        }
                        info_temp = textures[tex_id];
                    }
                }
                else
                {
                    if (tex_id >= textures.Count)
                    {
                        return new(false, null);
                    }
                    info_temp = textures[tex_id];
                }
                return new(true, info_temp);
            }
            return new(true, null);
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
            if (AnimationEntryController.AnimationEntry.IsNew)
                return new Crash3AnimationSelector(AnimationEntryController.NSF, AnimationEntryController.AnimationEntry, Frame);
            else
                return new UndockableControl(new AnimationEntryViewer(AnimationEntryController.NSF, AnimationEntryController.AnimationEntry.EID, AnimationEntryController.AnimationEntry.Frames.IndexOf(Frame)));
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
