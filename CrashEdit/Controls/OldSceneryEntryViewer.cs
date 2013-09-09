using Crash;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class OldSceneryEntryViewer : ThreeDimensionalViewer
    {
        private OldSceneryEntry entry;

        public OldSceneryEntryViewer(OldSceneryEntry entry)
        {
            this.entry = entry;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (OldSceneryVertex vertex in entry.Vertices)
                {
                    yield return vertex;
                }
            }
        }

        protected override void RenderObjects()
        {
            GL.Begin(BeginMode.Triangles);
            foreach (OldSceneryPolygon polygon in entry.Polygons)
            {
                RenderVertex(entry.Vertices[polygon.VertexA]);
                RenderVertex(entry.Vertices[polygon.VertexB]);
                RenderVertex(entry.Vertices[polygon.VertexC]);
            }
            GL.End();
        }

        private void RenderVertex(OldSceneryVertex vertex)
        {
            GL.Color3(vertex.F0,vertex.F1,vertex.F2);
            GL.Vertex3(vertex.X,vertex.Y,vertex.Z);
        }
    }
}
