using Crash;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public class ProtoSceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<ProtoSceneryEntry> entries;
        private int displaylist;

        public ProtoSceneryEntryViewer(ProtoSceneryEntry entry)
        {
            entries = new List<ProtoSceneryEntry>();
            entries.Add(entry);
            displaylist = -1;
        }

        public ProtoSceneryEntryViewer(IEnumerable<ProtoSceneryEntry> entries)
        {
            this.entries = new List<ProtoSceneryEntry>(entries);
            displaylist = -1;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (ProtoSceneryEntry entry in entries)
                {
                    foreach (ProtoSceneryVertex vertex in entry.Vertices)
                    {
                        yield return new Position(entry.XOffset + vertex.X, entry.YOffset + vertex.Y, entry.ZOffset + vertex.Z);
                    }
                }
            }
        }

        protected override void RenderObjects()
        {
            if (displaylist == -1)
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist, ListMode.CompileAndExecute);
                foreach (ProtoSceneryEntry entry in entries)
                {
                    if (entry != null)
                    {
                        foreach (ProtoSceneryPolygon polygon in entry.Polygons)
                        {
                            GL.Begin(PrimitiveType.Triangles);
                            if (polygon.VertexA < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[polygon.VertexA]);
                            if (polygon.VertexB < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[polygon.VertexB]);
                            if (polygon.VertexC < entry.Vertices.Count)
                                RenderVertex(entry, entry.Vertices[polygon.VertexC]);
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

        private void RenderVertex(ProtoSceneryEntry entry, ProtoSceneryVertex vertex)
        {
            Random random = new Random();
            byte[] color = new byte[3]; random.NextBytes(color); GL.Color3(color[0], color[1], color[2]);
            GL.Vertex3(entry.XOffset + vertex.X, entry.YOffset + vertex.Y, entry.ZOffset + vertex.Z);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
