using Crash;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class OldAnimationEntryViewer : ThreeDimensionalViewer
    {
        private List<OldFrame> frames;
        private OldModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private int xoffset;
        private int yoffset;
        private int zoffset;
        private int xcol1;
        private int ycol1;
        private int zcol1;
        private int xcol2;
        private int ycol2;
        private int zcol2;
        private bool pal;

        public OldAnimationEntryViewer(OldFrame frame,OldModelEntry model)
        {
            frames = new List<OldFrame>();
            frames.Add(frame);
            this.model = model;
            frameid = 0;
            xoffset = frame.XOffset;
            yoffset = frame.YOffset;
            zoffset = frame.ZOffset;
            xcol1 = frame.X1 + frame.XGlobal;
            ycol1 = frame.Y1 + frame.YGlobal;
            zcol1 = frame.Z1 + frame.ZGlobal;
            xcol2 = frame.X2 + frame.XGlobal;
            ycol2 = frame.Y2 + frame.YGlobal;
            zcol2 = frame.Z2 + frame.ZGlobal;
        }

        public OldAnimationEntryViewer(IEnumerable<OldFrame> frames,OldModelEntry model)
        {
            pal = false;
            this.frames = new List<OldFrame>(frames);
            this.model = model;
            frameid = 0;
            animatetimer = new Timer();
            animatetimer.Interval = 1000/30;
            if (pal)
                animatetimer.Interval = 1000/25;
            animatetimer.Enabled = true;
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = 1000 / 30;
                if (pal)
                    animatetimer.Interval = 1000 / 25;
                frameid++;
                if (frameid == this.frames.Count)
                {
                    frameid = 0;
                }
                xoffset = this.frames[frameid].XOffset;
                yoffset = this.frames[frameid].YOffset;
                zoffset = this.frames[frameid].ZOffset;
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
                        int x = vertex.X + frame.XOffset;
                        int y = vertex.Y + frame.YOffset;
                        int z = vertex.Z + frame.ZOffset;
                        yield return new Position(x, y, z);
                    }
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.F:
                    pal = !pal;
                    break;
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
            //RenderCollision();
        }

        private void RenderVertex(OldFrameVertex vertex)
        {
            GL.Vertex3(vertex.X + xoffset,vertex.Y + yoffset,vertex.Z + zoffset);
        }

        private void RenderCollision()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0f, 1f, 0f, 0.5f);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0f,1f,0f,0.5f);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol2 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(0f, 1f, 0f, 0.5f);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol1 * 0.00128 + zoffset);
            GL.Vertex3(xcol2 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.Vertex3(xcol1 * 0.00128 + xoffset,ycol1 * 0.00128 + yoffset,zcol2 * 0.00128 + zoffset);
            GL.End();
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
