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
        private int xoffset;
        private int yoffset;
        private int zoffset;

        public ProtoAnimationEntryViewer(ProtoFrame frame,OldModelEntry model)
        {
            frames = new List<ProtoFrame>();
            frames.Add(frame);
            this.model = model;
            frameid = 0;
            xoffset = frame.XOffset;
            yoffset = frame.YOffset;
            zoffset = frame.ZOffset;
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
                frameid = ++frameid % this.frames.Count;
                xoffset = this.frames[frameid].XOffset;
                yoffset = this.frames[frameid].YOffset;
                zoffset = this.frames[frameid].ZOffset;
                Refresh();
            };
        }

        protected override int CameraRangeMargin => 256;

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
            GL.PushMatrix();
            GL.Translate(frame.XOffset,frame.YOffset,frame.ZOffset);
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
            GL.PopMatrix();
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
