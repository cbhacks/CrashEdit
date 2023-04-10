using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crash;
using OpenTK;

namespace CrashEdit.Exporters;

public static class FrameExtensions
{
    public static void AddFrame (this OBJExporter exporter, NSF nsf, OldFrame frame, ref Dictionary <int, int> textureEIDs, ref Dictionary <string, TexInfoUnpacked> objTranslate)
    {
        var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
        var offset = new Vector3 (frame.XOffset, frame.YOffset, frame.ZOffset);
        var scale = new Vector3 (model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);

        foreach (OldModelStruct str in model.Structs)
        {
            if (str is not OldModelTexture tex)
                continue;

            if (textureEIDs.ContainsKey (tex.EID))
                continue;

            textureEIDs [tex.EID] = textureEIDs.Count;
        }

        foreach (OldModelPolygon polygon in model.Polygons)
        {
            string material = null;
            Vector2? uv1 = null, uv2 = null, uv3 = null;
            OldModelStruct str = model.Structs [polygon.Unknown & 0x7FFF];
            OldFrameVertex ov1 = frame.Vertices [polygon.VertexA / 6];
            OldFrameVertex ov2 = frame.Vertices [polygon.VertexB / 6];
            OldFrameVertex ov3 = frame.Vertices [polygon.VertexC / 6];
            Vector3 v1 = new Vector3 (ov1.X, ov1.Y, ov1.Z);
            Vector3 v2 = new Vector3 (ov2.X, ov2.Y, ov2.Z);
            Vector3 v3 = new Vector3 (ov3.X, ov3.Y, ov3.Z);
            Vector3 color = Vector3.Zero;

            if (str is OldModelTexture t)
            {
                int page = textureEIDs [t.EID];
                color = new Vector3 (t.R, t.G, t.B) / 255F;

                // add the texture to the list too
                material = objTranslate.FirstOrDefault (x =>
                    x.Value.color == t.ColorMode &&
                    x.Value.blend == t.BlendMode &&
                    x.Value.clutx == t.ClutX &&
                    x.Value.cluty == t.ClutY &&
                    x.Value.page == page
                ).Key;

                if (material is null)
                {
                    var texinfo = new TexInfoUnpacked (
                        true, color: t.ColorMode, blend: t.BlendMode,
                        clutx: t.ClutX, cluty: t.ClutY,
                        face: Convert.ToInt32 (t.N),
                        page: textureEIDs [t.EID]
                    );

                    var tpag = nsf.GetEntry <TextureChunk> (t.EID);
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
            else if (str is OldSceneryColor c)
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
    }

    public static void AddFrame (this OBJExporter exporter, NSF nsf, Frame frame, ref Dictionary <int, int> textureEIDs, ref Dictionary <string, TexInfoUnpacked> objTranslate)
    {
        var model = nsf.GetEntry<ModelEntry>(frame.ModelEID);
        var vertices = frame.MakeVertices (model);
        var offset = new Vector3 (frame.XOffset, frame.YOffset, frame.ZOffset) / 4F;
        var scale = new Vector3 (model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
        
        // detect how many textures are used and their eids to prepare the image

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
        
        // iterate all the triangles, get the texture modes and build information about those
        foreach (var tri in model.Triangles)
        {
            var info = TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures);
            string material = null;
            Vector2? uv1 = null, uv2 = null, uv3 = null;
            bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
            bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
            
            // parse the texture and add it to the exporter
            if (info.Item1 && info.Item2 is not null)
            {
                var value = info.Item2.Value;
                int textureEID = model.GetTPAG (value.Page);
                int page = textureEIDs [textureEID];

                material = objTranslate.FirstOrDefault (x => 
                    x.Value.color == value.ColorMode &&
                    x.Value.blend == value.BlendMode &&
                    x.Value.clutx == value.ClutX &&
                    x.Value.cluty == value.ClutY &&
                    x.Value.page == page
                ).Key;

                // ignore the texinfo if there's already a texture with the exact same settings stored
                if (material is null)
                {
                    var texinfo = new TexInfoUnpacked (
                        true, color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                        page: textureEIDs [textureEID]
                    );

                    var tpag = nsf.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

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
            Position fv1 = vertices [tri.Vertex [!flip ? 0 : 2] + frame.SpecialVertexCount];
            Position fv2 = vertices [tri.Vertex [!flip ? 1 : 1] + frame.SpecialVertexCount];
            Position fv3 = vertices [tri.Vertex [!flip ? 2 : 0] + frame.SpecialVertexCount];
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
    }
}