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
    public class SceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<SceneryEntry> entries;
        private int displaylist;

        public SceneryEntryViewer(SceneryEntry entry)
        {
            this.entries = new List<SceneryEntry>();
            this.entries.Add(entry);
            this.displaylist = -1;
        }

        public SceneryEntryViewer(IEnumerable<SceneryEntry> entries)
        {
            this.entries = new List<SceneryEntry>(entries);
            this.displaylist = -1;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (SceneryEntry entry in entries)
                {
                    foreach (SceneryVertex vertex in entry.Vertices)
                    {
                        yield return new Position(entry.XOffset + vertex.X * 16,entry.YOffset + vertex.Y * 16,entry.ZOffset + vertex.Z * 16);
                    }
                }
            }
        }

        protected override void RenderObjects()
        {
            if (displaylist == -1)
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist,ListMode.CompileAndExecute);
                foreach (SceneryEntry entry in entries)
                {
                    foreach (SceneryTriangle triangle in entry.Triangles)
                    {
                        GL.Begin(BeginMode.Triangles);
                        if (triangle.VertexA < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[triangle.VertexA]);
                        if (triangle.VertexB < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[triangle.VertexB]);
                        if (triangle.VertexC < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[triangle.VertexC]);
                        GL.End();
                    }
                    foreach (SceneryQuad quad in entry.Quads)
                    {
                        GL.Begin(BeginMode.Quads);
                        if (quad.VertexA < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[quad.VertexA]);
                        if (quad.VertexB < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[quad.VertexB]);
                        if (quad.Unknown1 < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[quad.Unknown1]);
                        if (quad.VertexC < entry.Vertices.Count)
                            RenderVertex(entry,entry.Vertices[quad.VertexC]);
                        GL.End();
                    }
                }
                GL.EndList();
            }
            else
            {
                GL.CallList(displaylist);
            }
        }

        private void RenderVertex(SceneryEntry entry,SceneryVertex vertex)
        {
            if (vertex.Color < entry.Colors.Count)
            {
                SceneryColor color = entry.Colors[vertex.Color];
                GL.Color3(color.Red,color.Green,color.Blue);
            }
            else
            {
                GL.Color3(Color.Fuchsia);
            }
            GL.Vertex3(entry.XOffset + vertex.X * 16,entry.YOffset + vertex.Y * 16,entry.ZOffset + vertex.Z * 16);
        }

        protected override void Dispose(bool disposing)
        {
            if (displaylist != -1)
            {
                // Crashes when closing the program
                //GL.DeleteLists(displaylist,1);
            }
            base.Dispose(disposing);
        }
    }
}
