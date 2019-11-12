using Crash;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public class SceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<SceneryEntry> entries;
        private int displaylist;

        public SceneryEntryViewer(SceneryEntry entry)
        {
            entries = new List<SceneryEntry>();
            entries.Add(entry);
            displaylist = -1;
        }

        public SceneryEntryViewer(IEnumerable<SceneryEntry> entries)
        {
            this.entries = new List<SceneryEntry>(entries);
            displaylist = -1;
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
                    if (entry != null)
                    {
                        GL.Begin(PrimitiveType.Triangles);
                        foreach (SceneryTriangle triangle in entry.Triangles)
                        {
                            if (triangle.VertexA < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[triangle.VertexA]);
                            if (triangle.VertexB < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[triangle.VertexB]);
                            if (triangle.VertexC < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[triangle.VertexC]);
                        }
                        GL.End();
                        GL.Begin(PrimitiveType.Quads);
                        foreach (SceneryQuad quad in entry.Quads)
                        {
                            if (quad.VertexA < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[quad.VertexA]);
                            if (quad.VertexB < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[quad.VertexB]);
                            if (quad.VertexC < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[quad.VertexC]);
                            if (quad.VertexD < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[quad.VertexD]);
                        }
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
            base.Dispose(disposing);
        }
    }
}
