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
                        yield return new Position((double)(entry.XOffset + vertex.X * 16),(double)(entry.YOffset + vertex.Y * 16),(double)(entry.ZOffset + vertex.Z * 16));
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
                        foreach (SceneryColorList colorlist in entry.ColorList)
                        {
                            foreach (SceneryTriangle triangle in entry.Triangles)
                            {
                                GL.Begin(PrimitiveType.Triangles);
                                if (triangle.VertexA < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[triangle.VertexA], colorlist);
                                if (triangle.VertexB < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[triangle.VertexB], colorlist);
                                if (triangle.VertexC < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[triangle.VertexC], colorlist);
                                GL.End();
                            }
                            foreach (SceneryQuad quad in entry.Quads)
                            {
                                GL.Begin(PrimitiveType.Quads);
                                if (quad.VertexA < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[quad.VertexA], colorlist);
                                if (quad.VertexB < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[quad.VertexB], colorlist);
                                if (quad.VertexC < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[quad.VertexC], colorlist);
                                if (quad.VertexD < entry.Vertices.Count)
                                    RenderVertex(entry, entry.Vertices[quad.VertexD], colorlist);
                                GL.End();
                            }
                        }
                    }
                }
                GL.EndList();
            }
            else
            {
                GL.CallList(displaylist);
            }
        }

        private void RenderVertex(SceneryEntry entry,SceneryVertex vertex,SceneryColorList colorlist)
        {
            if (vertex.Color < colorlist.Colors.Count)
            {
                SceneryColor color = colorlist.Colors[vertex.Color];
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
