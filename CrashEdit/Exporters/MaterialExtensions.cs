using System.Runtime.CompilerServices;
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
    private static VertexTexInfo? FindOrAddTexture
        (this OBJExporter exporter, NSF nsf, int colorMode, int blendMode, int clutx, int cluty, short page, int face, int textureEID, ref Dictionary <int, int> textureEIDs, ref Dictionary <VertexTexInfo, VertexTexInfo> objTranslate)
    {
        VertexTexInfo? material = null;
        
        try
        {
            // ignore the texinfo if there's already a texture with the exact same settings stored
            material = objTranslate.First (x => 
                x.Key.Color == colorMode &&
                x.Key.Blend == blendMode &&
                x.Key.ClutX == clutx &&
                x.Key.ClutY == cluty &&
                x.Key.Page == page &&
                x.Key.Face == face
            ).Key;
        }
        catch (Exception e)
        {
            // first throw an exception if there's no match, thus here we create it
            material = new VertexTexInfo (
                color: colorMode, blend: blendMode, clutx: clutx, cluty: cluty, page: page, face: face
            );

            var tpag = nsf.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

            Bitmap texture = TextureExporter.CreateTexture (tpag.Data, material.Value);

            // the material name changes
            exporter.AddTexture (material.Value, texture);
                
            // add it to the lookup table too
            objTranslate.Add (material.Value, material.Value);
        }

        return material;
    }
    
    public static VertexTexInfo? AddTexture (this OBJExporter exporter, NSF nsf, ModelTransformedTriangle tri, ModelEntry model, ref Dictionary <int, int> textureEIDs, ref Dictionary <VertexTexInfo, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3, out bool flip)
    {
        VertexTexInfo? material = null;
        uv1 = uv2 = uv3 = null;
        bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
        flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
        
        // parse the texture and add it to the exporter
        if (TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures, out ModelTexture value) && value is not null)
        {
            int textureEID = model.GetTPAG (value.Page);

            material = exporter.FindOrAddTexture (nsf, value.ColorMode, value.BlendMode, value.ClutX, value.ClutY, value.Page, 0, textureEID, ref textureEIDs, ref objTranslate);

            Vector2 texsize = TextureUtils.TextureSize (value.ColorMode);
            
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
    
    public static VertexTexInfo? AddTexture (this OBJExporter exporter, NSF nsf, SceneryTriangle tri, SceneryEntry scenery, ref Dictionary <int, int> textureEIDs, ref Dictionary <VertexTexInfo, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3)
    {
        VertexTexInfo? material = null;
        uv1 = uv2 = uv3 = null;

        if (TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, scenery.Textures, scenery.AnimatedTextures, out ModelTexture value) && value is not null)
        {
            int textureEID = scenery.GetTPAG (value.Page);

            material = exporter.FindOrAddTexture (nsf, value.ColorMode, value.BlendMode, value.ClutX, value.ClutY, value.Page, 0, textureEID, ref textureEIDs, ref objTranslate);

            Vector2 texsize = TextureUtils.TextureSize (value.ColorMode);

            uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
            uv2 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
            uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
        }

        return material;
    }
    
    public static VertexTexInfo? AddTexture (this OBJExporter exporter, NSF nsf, SceneryQuad quad, SceneryEntry scenery,  ref Dictionary <int, int> textureEIDs, ref Dictionary <VertexTexInfo, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3, out Vector2? uv4)
    {
        VertexTexInfo? material = null;
        uv1 = uv2 = uv3 = uv4 = null;

        if (TextureUtils.ProcessTextureInfoC2 (0, quad.Texture, quad.Animated, scenery.Textures, scenery.AnimatedTextures, out ModelTexture value) && value is not null)
        {
            int textureEID = scenery.GetTPAG (value.Page);

            material = exporter.FindOrAddTexture (nsf, value.ColorMode, value.BlendMode, value.ClutX, value.ClutY, value.Page, 0, textureEID, ref textureEIDs, ref objTranslate);

            Vector2 texsize = TextureUtils.TextureSize (value.ColorMode);

            uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
            uv2 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
            uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
            uv4 = new (value.X4 / texsize.X, (128 - value.Y4) / texsize.Y);
        }

        return material;
    }
    
    public static VertexTexInfo? AddTexture (this OBJExporter exporter, NSF nsf, OldSceneryTexture t, int textureEID, ref Dictionary<int, int> textureEIDs, ref Dictionary<VertexTexInfo, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3)
    {
        VertexTexInfo? material = exporter.FindOrAddTexture (nsf, t.ColorMode, t.BlendMode, t.ClutX, t.ClutY, (short) t.UVIndex, 0, textureEID, ref textureEIDs, ref objTranslate);
        Vector2 texsize = TextureUtils.TextureSize (t.ColorMode);
                
        uv3 = new (t.U1 / texsize.X, (128 - t.V1) / texsize.Y);
        uv2 = new (t.U2 / texsize.X, (128 - t.V2) / texsize.Y);
        uv1 = new (t.U3 / texsize.X, (128 - t.V3) / texsize.Y);

        return material;
    }
    
    public static VertexTexInfo? AddTexture (this OBJExporter exporter, NSF nsf, OldModelTexture t, int textureEID, ref Dictionary<int, int> textureEIDs, ref Dictionary<VertexTexInfo, VertexTexInfo> objTranslate, out Vector2? uv1, out Vector2? uv2, out Vector2? uv3)
    {
        VertexTexInfo? material = exporter.FindOrAddTexture (nsf, t.ColorMode, t.BlendMode, t.ClutX, t.ClutY, (short) t.UVIndex, Convert.ToInt32 (t.N), textureEID, ref textureEIDs, ref objTranslate);
        Vector2 texsize = TextureUtils.TextureSize (t.ColorMode);

        uv3 = new (t.U3 / texsize.X, (128 - t.V3) / texsize.Y);
        uv2 = new (t.U2 / texsize.X, (128 - t.V2) / texsize.Y);
        uv1 = new (t.U1 / texsize.X, (128 - t.V1) / texsize.Y);

        return material;
    }
}