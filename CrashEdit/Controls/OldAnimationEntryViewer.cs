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
    public sealed class OldAnimationEntryViewer : ThreeDimensionalViewer
    {
        private List<OldFrame> frames;
        private OldModelEntry model;
        private int frameid;
        private Timer animatetimer;

        public OldAnimationEntryViewer(OldFrame frame,OldModelEntry model)
        {
            this.frames = new List<OldFrame>();
            this.frames.Add(frame);
            this.model = model;
            this.frameid = 0;
        }

        public OldAnimationEntryViewer(IEnumerable<OldFrame> frames,OldModelEntry model)
        {
            this.frames = new List<OldFrame>(frames);
            this.model = model;
            this.frameid = 0;
            this.animatetimer = new Timer();
            this.animatetimer.Interval = 33;
            this.animatetimer.Enabled = true;
            this.animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                frameid++;
                Refresh();
            };
        }

        protected override int CameraRangeMargin
        {
            get { return 100; }
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (OldFrame frame in frames)
                {
                    foreach (OldFrameVertex vertex in frame.Vertices)
                    {
                        yield return vertex;
                    }
                }
            }
        }

        protected override void RenderObjects()
        {
            RenderFrame(frames[frameid % frames.Count]);
        }

        private void RenderFrame(OldFrame frame)
        {
            if (model != null)
            {
                GL.Begin(BeginMode.Triangles);
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    int coloroffset = 20 + (polygon.Unknown & 0x7FFF) * 4;
                    byte r = model.Info[coloroffset + 0];
                    byte g = model.Info[coloroffset + 1];
                    byte b = model.Info[coloroffset + 2];
                    GL.Color3(r,g,b);
                    RenderVertex(frame.Vertices[polygon.VertexA / 6]);
                    RenderVertex(frame.Vertices[polygon.VertexB / 6]);
                    RenderVertex(frame.Vertices[polygon.VertexC / 6]);
                }
                GL.End();
            }
            else
            {
                GL.Begin(BeginMode.Points);
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    RenderVertex(vertex);
                }
                GL.End();
            }
        }

        private void RenderVertex(OldFrameVertex vertex)
        {
            GL.Vertex3(vertex.X,vertex.Y,vertex.Z);
        }

        protected override void Dispose(bool disposing)
        {
            if (animatetimer != null)
            {
                animatetimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
