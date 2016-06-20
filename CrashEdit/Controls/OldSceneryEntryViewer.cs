using Crash;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public class OldSceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<OldSceneryEntry> entries;
        private int displaylist;

        public OldSceneryEntryViewer(OldSceneryEntry entry)
        {
            entries = new List<OldSceneryEntry>();
            entries.Add(entry);
            displaylist = -1;
        }

        public OldSceneryEntryViewer(IEnumerable<OldSceneryEntry> entries)
        {
            this.entries = new List<OldSceneryEntry>(entries);
            displaylist = -1;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (OldSceneryEntry entry in entries)
                {
                    foreach (OldSceneryVertex vertex in entry.Vertices)
                    {
                        yield return new Position(entry.XOffset + vertex.X,entry.YOffset + vertex.Y,entry.ZOffset + vertex.Z);
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
                foreach (OldSceneryEntry entry in entries)
                {
                    if (entry != null)
                    {
                        foreach (OldSceneryPolygon polygon in entry.Polygons)
                        {
                            GL.Begin(PrimitiveType.Triangles);
                            if (polygon.VertexA < entry.Vertices.Count)
                                RenderVertex(entry,entry.Vertices[polygon.VertexA]);
                            if (polygon.VertexB < entry.Vertices.Count)
                                RenderVertex(entry,entry.Vertices[polygon.VertexB]);
                            if (polygon.VertexC < entry.Vertices.Count)
                                RenderVertex(entry,entry.Vertices[polygon.VertexC]);
                            GL.End();
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

        private void RenderVertex(OldSceneryEntry entry,OldSceneryVertex vertex)
        {
            GL.Color3(vertex.Red,vertex.Green,vertex.Blue);
            GL.Vertex3(entry.XOffset + vertex.X,entry.YOffset + vertex.Y,entry.ZOffset + vertex.Z);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
