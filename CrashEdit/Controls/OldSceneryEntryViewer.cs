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
        private List<OldSceneryEntry> entries;

        public OldSceneryEntryViewer(OldSceneryEntry entry)
        {
            this.entries = new List<OldSceneryEntry>();
            this.entries.Add(entry);
        }

        public OldSceneryEntryViewer(IEnumerable<OldSceneryEntry> entries)
        {
            this.entries = new List<OldSceneryEntry>(entries);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (OldSceneryEntry entry in entries)
                {
                    foreach (OldSceneryVertex vertex in entry.Vertices)
                    {
                        yield return new Position(vertex.X + entry.XOffset,vertex.Y + entry.YOffset,vertex.Z + entry.ZOffset);
                    }
                }
            }
        }

        protected override void RenderObjects()
        {
            foreach (OldSceneryEntry entry in entries)
            {
                GL.PushMatrix();
                GL.Translate(entry.XOffset,entry.YOffset,entry.ZOffset);
                GL.Begin(BeginMode.Triangles);
                foreach (OldSceneryPolygon polygon in entry.Polygons)
                {
                    RenderVertex(entry.Vertices[polygon.VertexA]);
                    RenderVertex(entry.Vertices[polygon.VertexB]);
                    RenderVertex(entry.Vertices[polygon.VertexC]);
                }
                GL.End();
                GL.PopMatrix();
            }
        }

        private void RenderVertex(OldSceneryVertex vertex)
        {
            GL.Color3(vertex.F0,vertex.F1,vertex.F2);
            GL.Vertex3(vertex.X,vertex.Y,vertex.Z);
        }
    }
}
