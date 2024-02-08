using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Crash;
using OpenTK;

namespace CrashEdit.Exporters;

// TODO: ALL SCENERY CLASSES/STRUCTS SHOULD HAVE A BASE INTERFACE WITH DATA IN COMMON
// TODO: THAT WOULD MAKE WORKING WITH THEM EASIER FOR THINGS LIKE THESE WHERE YOU ONLY NEED THE DATA
// TODO: THEY HAVE IN COMMON, BUT CHANGING THAT IS OUT OF THE SCOPE OF THESE COMMITS
// TODO: BUT THAT WOULD CUT DOWN THE METHODS HERE TO JUST ONE OR TWO
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
                int textureEID = scenery.GetTPAG (polygon.Page);
                material = exporter.AddTexture (nsf, t, textureEID, ref textureEIDs, ref objTranslate, out color, out uv1, out uv2, out uv3);
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
        //var scale = new Vector3 (1 / GameScales.WorldC1);
        
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
            
            Vector2? uv1 = null, uv2 = null, uv3 = null;

            string material = exporter.AddTexture (nsf, tri, scenery, ref textureEIDs, ref objTranslate, out uv1, out uv2, out uv3);

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
                (v1 * 16 + offset) / GameScales.WorldC1,
                (v2 * 16 + offset) / GameScales.WorldC1,
                (v3 * 16 + offset) / GameScales.WorldC1,
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
            
            Vector2? uv1 = null, uv2 = null, uv3 = null, uv4 = null;
            string material = exporter.AddTexture (nsf, quad, scenery, ref textureEIDs, ref objTranslate, out uv1, out uv2, out uv3, out uv4);

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
                (v1 * 16 + offset) / GameScales.WorldC1,
                (v2 * 16 + offset) / GameScales.WorldC1,
                (v3 * 16 + offset) / GameScales.WorldC1,
                (v4 * 16 + offset) / GameScales.WorldC1,
                c1, c2, c3, c4,
                material,
                uv1, uv2, uv3, uv4
            );
        }
    }
}