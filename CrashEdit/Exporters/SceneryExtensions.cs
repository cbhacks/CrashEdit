using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crash;
using OpenTK;

namespace CrashEdit.Exporters;

public static class SceneryExtensions
{
    public static void AddScenery (this OBJExporter exporter, NSF nsf, OldSceneryEntry scenery, ref Dictionary<int, int> textureEIDs, ref Dictionary <string, TexInfoUnpacked> objTranslate)
    {
        var offset = new Vector3 (scenery.XOffset, scenery.YOffset, scenery.ZOffset);
        var scale = new Vector3 (1 / GameScales.WorldC1);
        
        for (int i = 0; i < scenery.TPAGCount; i++)
        {
            int tpag_eid = scenery.GetTPAG (i);

            if (textureEIDs.ContainsKey (tpag_eid))
                continue;

            textureEIDs.Add (tpag_eid, textureEIDs.Count);
        }
        
        foreach (var polygon in scenery.Polygons)
        {
            string material = null;
            Vector2? uv1 = null, uv2 = null, uv3 = null;
            OldModelStruct str = scenery.Structs[polygon.ModelStruct];
            OldSceneryVertex ov1 = scenery.Vertices [polygon.VertexA];
            OldSceneryVertex ov2 = scenery.Vertices [polygon.VertexB];
            OldSceneryVertex ov3 = scenery.Vertices [polygon.VertexC];
            Vector3 v1 = new Vector3 (ov1.X, ov1.Y, ov1.Z);
            Vector3 v2 = new Vector3 (ov2.X, ov2.Y, ov2.Z);
            Vector3 v3 = new Vector3 (ov3.X, ov3.Y, ov3.Z);
            Vector3 color = Vector3.Zero;

            if (str is OldSceneryTexture t)
            {
                int page = textureEIDs [scenery.GetTPAG (polygon.Page)];
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
                    var texinfo = new TexInfoUnpacked(
                        true, color: t.ColorMode, blend: t.BlendMode,
                        clutx: t.ClutX, cluty: t.ClutY,
                        page: textureEIDs[scenery.GetTPAG (polygon.Page)]
                    );

                    var tpag = nsf.GetEntry <TextureChunk> (scenery.GetTPAG (polygon.Page));
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
                
                uv3 = new Vector2 (t.U1 / texsize.X, (128 - t.V1) / texsize.Y);
                uv2 = new Vector2 (t.U2 / texsize.X, (128 - t.V2) / texsize.Y);
                uv1 = new Vector2 (t.U3 / texsize.X, (128 - t.V3) / texsize.Y);
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
    }
    
    public static void AddScenery (this OBJExporter exporter, NSF nsf, SceneryEntry scenery, ref Dictionary <int, int> textureEIDs, ref Dictionary <string, TexInfoUnpacked> objTranslate)
    {
        var offset = new Vector3 (scenery.XOffset, scenery.YOffset, scenery.ZOffset);
        var scale = new Vector3 (1 / GameScales.WorldC1);
        
        for (int i = 0; i < scenery.TPAGCount; i++)
        {
            int tpag_eid = scenery.GetTPAG (i);

            if (textureEIDs.ContainsKey (tpag_eid))
                continue;

            textureEIDs.Add (tpag_eid, textureEIDs.Count);
        }
        
        foreach (var tri in scenery.Triangles)
        {
            if (tri.VertexA > scenery.Vertices.Count ||
                tri.VertexB > scenery.Vertices.Count ||
                tri.VertexC > scenery.Vertices.Count)
                continue;
            
            var info = TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, scenery.Textures, scenery.AnimatedTextures);
            string material = null;
            Vector2? uv1 = null, uv2 = null, uv3 = null;

            if (info.Item1 && info.Item2 is not null)
            {
                var value = info.Item2.Value;
                int textureEID = scenery.GetTPAG (value.Page);
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

                uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
                uv2 = new Vector2 (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
            }

            // add the face
            SceneryVertex fv1 = scenery.Vertices [tri.VertexA];
            SceneryVertex fv2 = scenery.Vertices [tri.VertexB];
            SceneryVertex fv3 = scenery.Vertices [tri.VertexC];
            SceneryColor fc1 = scenery.Colors [fv1.Color];
            SceneryColor fc2 = scenery.Colors [fv2.Color];
            SceneryColor fc3 = scenery.Colors [fv3.Color];
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

        foreach (var quad in scenery.Quads)
        {
            // ignore quads that are out of limits
            if (quad.VertexA > scenery.Vertices.Count ||
                quad.VertexB > scenery.Vertices.Count ||
                quad.VertexC > scenery.Vertices.Count ||
                quad.VertexD > scenery.Vertices.Count)
                continue;
            
            var info = TextureUtils.ProcessTextureInfoC2 (0, quad.Texture, quad.Animated, scenery.Textures, scenery.AnimatedTextures);
            string material = null;
            Vector2? uv1 = null, uv2 = null, uv3 = null, uv4 = null;

            if (info.Item1 && info.Item2 is not null)
            {
                var value = info.Item2.Value;
                int textureEID = scenery.GetTPAG (value.Page);
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

                uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
                uv2 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                uv4 = new (value.X4 / texsize.X, (128 - value.Y4) / texsize.Y);
            }

            // add the face
            SceneryVertex fv1 = scenery.Vertices [quad.VertexA];
            SceneryVertex fv2 = scenery.Vertices [quad.VertexB];
            SceneryVertex fv3 = scenery.Vertices [quad.VertexC];
            SceneryVertex fv4 = scenery.Vertices [quad.VertexD];
            SceneryColor fc1 = scenery.Colors [fv1.Color];
            SceneryColor fc2 = scenery.Colors [fv2.Color];
            SceneryColor fc3 = scenery.Colors [fv3.Color];
            SceneryColor fc4 = scenery.Colors [fv4.Color];
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
    }
}