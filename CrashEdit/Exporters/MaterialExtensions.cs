using CrashEdit.CE;
using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.Exporters;

// TODO: ALL VERTEX/QUAD CLASSES/STRUCTS SHOULD HAVE A BASE INTERFACE WITH DATA IN COMMON
// TODO: THAT WOULD MAKE WORKING WITH THEM EASIER FOR THINGS LIKE THESE WHERE YOU ONLY NEED THE DATA
// TODO: THEY HAVE IN COMMON, BUT CHANGING THAT IS OUT OF THE SCOPE OF THESE COMMITS
// TODO: BUT THAT WOULD CUT DOWN THE METHODS HERE TO JUST ONE OR TWO
public static class MaterialExtensions
{
    private static Vector2 TexSize (int colorMode)
    {
        return colorMode switch
        {
            0 => new Vector2 (1024, 128),
            1 => new Vector2 (512, 128),
            _ => new Vector2 (256, 128)
        };
    }
    
    public static string AddTexture (this OBJExporter exporter, NSF nsf, ModelTransformedTriangle tri, ModelEntry model, ref Dictionary <int, int> textureEIDs, ref Dictionary <string, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3, out bool flip)
    {
        string material = null;
        uv1 = uv2 = uv3 = null;
        bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
        flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
        
        // parse the texture and add it to the exporter
        if (TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures, out ModelTexture value) && value is not null)
        {
            int textureEID = model.GetTPAG (value.Page);

            material = objTranslate.FirstOrDefault (x => 
                x.Value.Color == value.ColorMode &&
                x.Value.Blend == value.BlendMode &&
                x.Value.ClutX == value.ClutX &&
                x.Value.ClutY == value.ClutY &&
                x.Value.Page == value.Page
            ).Key;

            // ignore the texinfo if there's already a texture with the exact same settings stored
            if (material is null)
            {
                var texinfo = new VertexTexInfo (
                    color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                    page: value.Page
                );

                var tpag = nsf.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

                Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

                // the material name changes
                material = exporter.AddTexture ($"{textureEID}-{((int)texinfo).ToString("X8")}c{texinfo.Color}b{texinfo.Blend}", texture);
                
                // add it to the lookup table too
                objTranslate [material] = texinfo;
            }

            Vector2 texsize = TexSize (value.ColorMode);
            
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

        return material;
    }
    
    public static string AddTexture (this OBJExporter exporter, NSF nsf, SceneryTriangle tri, SceneryEntry scenery, ref Dictionary <int, int> textureEIDs, ref Dictionary <string, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3)
    {
        string material = null;
        uv1 = uv2 = uv3 = null;

        if (TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, scenery.Textures, scenery.AnimatedTextures, out ModelTexture value) && value is not null)
        {
            int textureEID = scenery.GetTPAG (value.Page);

            material = objTranslate.FirstOrDefault (x =>
                x.Value.Color == value.ColorMode &&
                x.Value.Blend == value.BlendMode &&
                x.Value.ClutX == value.ClutX &&
                x.Value.ClutY == value.ClutY &&
                x.Value.Page == value.Page
            ).Key;

            // ignore the texinfo if there's already a texture with the exact same settings stored
            if (material is null)
            {
                var texinfo = new VertexTexInfo (
                    color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                    page: value.Page
                );

                var tpag = nsf.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

                Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

                // the material name changes
                material = exporter.AddTexture ($"{textureEID}-{((int)texinfo).ToString("X8")}c{texinfo.Color}b{texinfo.Blend}", texture);

                // add it to the lookup table too
                objTranslate [material] = texinfo;
            }

            Vector2 texsize = TexSize (value.ColorMode);

            uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
            uv2 = new Vector2 (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
            uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
        }

        return material;
    }
    
    public static string AddTexture (this OBJExporter exporter, NSF nsf, SceneryQuad quad, SceneryEntry scenery,  ref Dictionary <int, int> textureEIDs, ref Dictionary <string, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3, out Vector2? uv4)
    {
        string material = null;
        uv1 = uv2 = uv3 = uv4 = null;

        if (TextureUtils.ProcessTextureInfoC2 (0, quad.Texture, quad.Animated, scenery.Textures, scenery.AnimatedTextures, out ModelTexture value) && value is not null)
        {
            int textureEID = scenery.GetTPAG (value.Page);

            material = objTranslate.FirstOrDefault (x =>
                x.Value.Color == value.ColorMode &&
                x.Value.Blend == value.BlendMode &&
                x.Value.ClutX == value.ClutX &&
                x.Value.ClutY == value.ClutY &&
                x.Value.Page == value.Page
            ).Key;

            // ignore the texinfo if there's already a texture with the exact same settings stored
            if (material is null)
            {
                var texinfo = new VertexTexInfo (
                    color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                    page: value.Page
                );

                var tpag = nsf.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

                Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

                // the material name changes
                material = exporter.AddTexture ($"{textureEID}-{((int)texinfo).ToString("X8")}c{texinfo.Color}b{texinfo.Blend}", texture);

                // add it to the lookup table too
                objTranslate [material] = texinfo;
            }

            Vector2 texsize = TexSize (value.ColorMode);

            uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
            uv2 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
            uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
            uv4 = new (value.X4 / texsize.X, (128 - value.Y4) / texsize.Y);
        }

        return material;
    }
    
    public static string AddTexture (this OBJExporter exporter, NSF nsf, OldSceneryTexture t, int textureEID, ref Dictionary <string, VertexTexInfo> objTranslate, out Vector3 color, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3)
    {
        color = new Vector3 (t.R, t.G, t.B) / 255F;
                
        // add the texture to the list too
        string material = objTranslate.FirstOrDefault (x => 
            x.Value.Color == t.ColorMode &&
            x.Value.Blend == t.BlendMode &&
            x.Value.ClutX == t.ClutX &&
            x.Value.ClutY == t.ClutY &&
            x.Value.Page == t.UVIndex
        ).Key;
                
        if (material is null)
        {
            var texinfo = new VertexTexInfo(
                color: t.ColorMode, blend: t.BlendMode,
                clutx: t.ClutX, cluty: t.ClutY,
                page: (short) t.UVIndex
            );

            var tpag = nsf.GetEntry <TextureChunk> (textureEID);
            Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);
                    
            // the material name changes
            material = exporter.AddTexture ($"{textureEID}-{((int)texinfo).ToString("X8")}c{texinfo.Color}b{texinfo.Blend}", texture);
                    
            // add it to the lookup table too
            objTranslate [material] = texinfo;
        }

        Vector2 texsize = TexSize (t.ColorMode);
                
        uv3 = new Vector2 (t.U1 / texsize.X, (128 - t.V1) / texsize.Y);
        uv2 = new Vector2 (t.U2 / texsize.X, (128 - t.V2) / texsize.Y);
        uv1 = new Vector2 (t.U3 / texsize.X, (128 - t.V3) / texsize.Y);

        return material;
    }
    
    public static string AddTexture (this OBJExporter exporter, NSF nsf, OldModelTexture t, ref Dictionary<string, VertexTexInfo> objTranslate, out Vector3 color, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3)
    {
        color = new Vector3 (t.R, t.G, t.B) / 255F;

        // add the texture to the list too
        string material = objTranslate.FirstOrDefault (x =>
            x.Value.Color == t.ColorMode &&
            x.Value.Blend == t.BlendMode &&
            x.Value.ClutX == t.ClutX &&
            x.Value.ClutY == t.ClutY &&
            x.Value.Page == t.UVIndex &&
            x.Value.Face == Convert.ToInt32 (t.N)
        ).Key;

        if (material is null)
        {
            var texinfo = new VertexTexInfo (
                color: t.ColorMode, blend: t.BlendMode,
                clutx: t.ClutX, cluty: t.ClutY,
                face: Convert.ToInt32 (t.N),
                page: (short)t.UVIndex
            );

            var tpag = nsf.GetEntry <TextureChunk> (t.EID);
            Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

            // the material name changes
            material = exporter.AddTexture ($"{t.EID}-{((int)texinfo).ToString("X8")}c{texinfo.Color}b{texinfo.Blend}", texture);

            // add it to the lookup table too
            objTranslate [material] = texinfo;
        }

        Vector2 texsize = TexSize (t.ColorMode);

        uv3 = new Vector2 (t.U3 / texsize.X, (128 - t.V3) / texsize.Y);
        uv2 = new Vector2 (t.U2 / texsize.X, (128 - t.V2) / texsize.Y);
        uv1 = new Vector2 (t.U1 / texsize.X, (128 - t.V1) / texsize.Y);

        return material;
    }
}