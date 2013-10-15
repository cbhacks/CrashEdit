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
        private int displaylist;

        public OldSceneryEntryViewer(OldSceneryEntry entry)
        {
            this.entries = new List<OldSceneryEntry>();
            this.entries.Add(entry);
            this.displaylist = -1;
        }

        public OldSceneryEntryViewer(IEnumerable<OldSceneryEntry> entries)
        {
            this.entries = new List<OldSceneryEntry>(entries);
            this.displaylist = -1;
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
            if (displaylist == -1)
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist,ListMode.CompileAndExecute);
                GL.Begin(BeginMode.Triangles);
                foreach (OldSceneryEntry entry in entries)
                {
                    foreach (OldSceneryPolygon polygon in entry.Polygons)
                    {
                        RenderVertex(entry,entry.Vertices[polygon.VertexA]);
                        RenderVertex(entry,entry.Vertices[polygon.VertexB]);
                        RenderVertex(entry,entry.Vertices[polygon.VertexC]);
                    }
                }
                GL.End();
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
            if (displaylist != -1)
            {
                // Crashes when closing the program
                //GL.DeleteLists(displaylist,1);
            }
            base.Dispose(disposing);
        }
    }
}
