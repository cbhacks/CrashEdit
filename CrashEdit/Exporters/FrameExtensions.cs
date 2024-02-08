using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crash;
using OpenTK;

namespace CrashEdit.Exporters;

// TODO: ALL FRAMES CLASSES/STRUCTS SHOULD HAVE A BASE INTERFACE WITH DATA IN COMMON
// TODO: THAT WOULD MAKE WORKING WITH THEM EASIER FOR THINGS LIKE THESE WHERE YOU ONLY NEED THE DATA
// TODO: THEY HAVE IN COMMON, BUT CHANGING THAT IS OUT OF THE SCOPE OF THESE COMMITS
// TODO: BUT THAT WOULD CUT DOWN THE METHODS HERE TO JUST ONE OR TWO
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
                material = exporter.AddTexture (nsf, t, ref textureEIDs, ref objTranslate, out color, out uv1, out uv2, out uv3);
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
        // TODO: SUPPORT CRASH2 AND CRASH3 PROPER SCALING
        // offset correction is 4f in Crash2, 32f in Crash3
        var model = nsf.GetEntry<ModelEntry>(frame.ModelEID);
        var vertices = frame.MakeVertices (model);
        var offset = new Vector3 (frame.XOffset, frame.YOffset, frame.ZOffset) / 4F;
        var scale = new Vector3 (model.ScaleX, model.ScaleY, model.ScaleZ) / GameScales.ModelC1 / GameScales.AnimC1;
        
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
            string material = exporter.AddTexture (
                nsf, tri, model, ref textureEIDs, ref objTranslate,
                out var uv1, out var uv2, out var uv3,
                out bool flip
            );

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