using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Crash;
using System.Windows.Forms;
using CrashEdit.Exporters;
using OpenTK;

namespace CrashEdit
{
    public sealed class SceneryEntryController : EntryController
    {
        public SceneryEntryController(EntryChunkController entrychunkcontroller, SceneryEntry sceneryentry) : base(entrychunkcontroller, sceneryentry)
        {
            SceneryEntry = sceneryentry;
            AddMenuSeparator();
            AddMenu("Export as Wavefront OBJ", Menu_Export_OBJ);
            AddMenu("Export as Stanford PLY", Menu_Export_PLY);
            //AddMenu("Export as COLLADA",Menu_Export_COLLADA);
            AddMenu("Fix coords imported from Crash 3", Menu_Fix_WGEOv3);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.SceneryEntryController_Text, SceneryEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "blueb";
            Node.SelectedImageKey = "blueb";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new SceneryEntryViewer(NSF, SceneryEntry.EID));
        }

        public SceneryEntry SceneryEntry { get; }

        private void Menu_Export_OBJ ()
        {
            FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any);
            ToOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }

        private void ToOBJ (string path, string modelname)
        {
            var exporter = new OBJExporter ();
            var offset = new Vector3 (SceneryEntry.XOffset, SceneryEntry.YOffset, SceneryEntry.ZOffset);
            var scale = new Vector3 (1 / GameScales.WorldC1);
            
            // detect how many textures are used and their eids to prepare the image
            Dictionary <int, int> textureEIDs = new ();

            for (int i = 0; i < SceneryEntry.TPAGCount; i++)
            {
                int tpag_eid = SceneryEntry.GetTPAG (i);

                if (textureEIDs.ContainsKey (tpag_eid))
                    continue;

                textureEIDs.Add (tpag_eid, textureEIDs.Count);
            }
            
            Dictionary <string, TexInfoUnpacked> objTranslate = new Dictionary <string, TexInfoUnpacked> ();

            foreach (var tri in SceneryEntry.Triangles)
            {
                var info = ProcessTextureInfoC2(tri.Texture, tri.Animated, SceneryEntry.Textures, SceneryEntry.AnimatedTextures);
                string material = null;
                Vector2? uv1 = null, uv2 = null, uv3 = null;

                if (info.Item1 && info.Item2 is not null)
                {
                    var value = info.Item2.Value;
                    
                    material = objTranslate.FirstOrDefault (x => 
                        x.Value.color == value.ColorMode &&
                        x.Value.blend == value.BlendMode &&
                        x.Value.clutx == value.ClutX &&
                        x.Value.cluty == value.ClutY &&
                        x.Value.page == textureEIDs [SceneryEntry.GetTPAG (value.Page)]
                    ).Key;

                    // ignore the texinfo if there's already a texture with the exact same settings stored
                    if (material is null)
                    {
                        var texinfo = new TexInfoUnpacked (
                            true, color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                            page: textureEIDs [SceneryEntry.GetTPAG (value.Page)]
                        );

                        var tpag = this.NSF.GetEntry <TextureChunk> (textureEIDs.First (x => x.Value == texinfo.page).Key);

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
                    
                    uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
                    uv2 = new Vector2 (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                    uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                }
                
                // add the face
                SceneryVertex fv1 = SceneryEntry.Vertices[tri.VertexA];
                SceneryVertex fv2 = SceneryEntry.Vertices[tri.VertexB];
                SceneryVertex fv3 = SceneryEntry.Vertices[tri.VertexC];
                SceneryColor fc1 = SceneryEntry.Colors [fv1.Color];
                SceneryColor fc2 = SceneryEntry.Colors [fv2.Color];
                SceneryColor fc3 = SceneryEntry.Colors [fv3.Color];
                Vector3 v1 = new Vector3 (fv1.X, fv1.Y, fv1.Z);
                Vector3 v2 = new Vector3 (fv2.X, fv2.Y, fv2.Z);
                Vector3 v3 = new Vector3 (fv3.X, fv3.Y, fv3.Z);
                Vector3 c1 = new Vector3 (fc1.Red, fc1.Green, fc1.Blue) / 255f;
                Vector3 c2 = new Vector3 (fc2.Red, fc2.Green, fc2.Blue) / 255f;
                Vector3 c3 = new Vector3 (fc3.Red, fc3.Green, fc3.Blue) / 255f;
                
                exporter.AddFace (
                    (v1 + offset / 16) * scale,
                    (v2 + offset / 16) * scale,
                    (v3 + offset / 16) * scale,
                    c1, c2, c3,
                    material,
                    uv1, uv2, uv3
                );
            }
            
            foreach (var quad in SceneryEntry.Quads)
            {
                var info = ProcessTextureInfoC2(quad.Texture, quad.Animated, SceneryEntry.Textures, SceneryEntry.AnimatedTextures);
                string material = null;
                Vector2? uv1 = null, uv2 = null, uv3 = null, uv4 = null;

                if (info.Item1 && info.Item2 is not null)
                {
                    var value = info.Item2.Value;
                    
                    material = objTranslate.FirstOrDefault (x => 
                        x.Value.color == value.ColorMode &&
                        x.Value.blend == value.BlendMode &&
                        x.Value.clutx == value.ClutX &&
                        x.Value.cluty == value.ClutY &&
                        x.Value.page == textureEIDs [SceneryEntry.GetTPAG (value.Page)]
                    ).Key;

                    // ignore the texinfo if there's already a texture with the exact same settings stored
                    if (material is null)
                    {
                        var texinfo = new TexInfoUnpacked (
                            true, color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                            page: textureEIDs [SceneryEntry.GetTPAG (value.Page)]
                        );

                        var tpag = this.NSF.GetEntry <TextureChunk> (textureEIDs.First (x => x.Value == texinfo.page).Key);

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
                    
                    uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
                    uv2 = new Vector2 (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                    uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                    uv4 = new (value.X4 / texsize.X, (128 - value.Y4) / texsize.Y);
                }
                
                // add the face
                SceneryVertex fv1 = SceneryEntry.Vertices[quad.VertexA];
                SceneryVertex fv2 = SceneryEntry.Vertices[quad.VertexB];
                SceneryVertex fv3 = SceneryEntry.Vertices[quad.VertexC];
                SceneryVertex fv4 = SceneryEntry.Vertices[quad.VertexD];
                SceneryColor fc1 = SceneryEntry.Colors [fv1.Color];
                SceneryColor fc2 = SceneryEntry.Colors [fv2.Color];
                SceneryColor fc3 = SceneryEntry.Colors [fv3.Color];
                SceneryColor fc4 = SceneryEntry.Colors [fv4.Color];
                Vector3 v1 = new Vector3 (fv1.X, fv1.Y, fv1.Z);
                Vector3 v2 = new Vector3 (fv2.X, fv2.Y, fv2.Z);
                Vector3 v3 = new Vector3 (fv3.X, fv3.Y, fv3.Z);
                Vector3 v4 = new Vector3 (fv4.X, fv4.Y, fv4.Z);
                Vector3 c1 = new Vector3 (fc1.Red, fc1.Green, fc1.Blue) / 255f;
                Vector3 c2 = new Vector3 (fc2.Red, fc2.Green, fc2.Blue) / 255f;
                Vector3 c3 = new Vector3 (fc3.Red, fc3.Green, fc3.Blue) / 255f;
                Vector3 c4 = new Vector3 (fc4.Red, fc4.Green, fc4.Blue) / 255f;
                
                exporter.AddFace (
                    (v1 + offset / 16) * scale,
                    (v2 + offset / 16) * scale,
                    (v3 + offset / 16) * scale,
                    (v4 + offset / 16) * scale,
                    c1, c2, c3, c4,
                    material,
                    uv1, uv2, uv3, uv4
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
            for (int i = 0; i < SceneryEntry.Vertices.Count; i++)
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
