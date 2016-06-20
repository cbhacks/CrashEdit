using Crash;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class AnimationEntryViewer : ThreeDimensionalViewer
    {
        private List<Frame> frames;
        //private ModelEntry model;
        private int frameid;
        private Timer animatetimer;

        public AnimationEntryViewer(Frame frame/*,ModelEntry model*/)
        {
            frames = new List<Frame>();
            frames.Add(frame);
            //this.model = model;
            frameid = 0;
        }

        public AnimationEntryViewer(IEnumerable<Frame> frames/*,ModelEntry model*/)
        {
            this.frames = new List<Frame>(frames);
            //this.model = model;
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
                foreach (Frame frame in frames)
                {
                    foreach (FrameVertex vertex in frame.Vertices)
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

        private void RenderFrame(Frame frame)
        {
            /*if (model != null)
            {
                GL.Begin(PrimitiveType.Triangles);
                foreach (ModelPolygon polygon in model.Polygons)
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
            */{
                GL.Begin(PrimitiveType.Points);
                foreach (FrameVertex vertex in frame.Vertices)
                {
                    RenderVertex(vertex);
                }
                GL.End();
            }
        }

        private void RenderVertex(FrameVertex vertex)
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
