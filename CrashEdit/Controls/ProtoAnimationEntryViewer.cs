using Crash;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class ProtoAnimationEntryViewer : ThreeDimensionalViewer
    {
        private List<ProtoFrame> frames;
        private OldModelEntry model;
        private int frameid;
        private Timer animatetimer;

        public ProtoAnimationEntryViewer(ProtoFrame frame,OldModelEntry model)
        {
            frames = new List<ProtoFrame>();
            frames.Add(frame);
            this.model = model;
            frameid = 0;
        }

        public ProtoAnimationEntryViewer(IEnumerable<ProtoFrame> frames,OldModelEntry model)
        {
            this.frames = new List<ProtoFrame>(frames);
            this.model = model;
            frameid = 0;
            animatetimer = new Timer();
            animatetimer.Interval = 40;
            animatetimer.Enabled = true;
            animatetimer.Tick += delegate (object sender,EventArgs e)
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
                foreach (ProtoFrame frame in frames)
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

        private void RenderFrame(ProtoFrame frame)
        {
            if (model != null)
            {
                GL.Begin(PrimitiveType.Triangles);
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
                GL.Begin(PrimitiveType.Points);
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
