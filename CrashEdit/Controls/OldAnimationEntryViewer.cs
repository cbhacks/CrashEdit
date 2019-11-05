using Crash;
using System;
using System.Drawing;
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
        private int xglobal;
        private int yglobal;
        private int zglobal;
        private int xcol1;
        private int ycol1;
        private int zcol1;
        private int xcol2;
        private int ycol2;
        private int zcol2;
        private bool pal;

        public OldAnimationEntryViewer(OldFrame frame,OldModelEntry model)
        {
            frames = new List<OldFrame>() { frame };
            this.model = model;
            frameid = 0;
            xoffset = frame.XOffset;
            yoffset = frame.YOffset;
            zoffset = frame.ZOffset;
            xglobal = frame.XGlobal;
            yglobal = frame.YGlobal;
            zglobal = frame.ZGlobal;
            xcol1 = frame.X1;
            xcol2 = frame.X2;
            ycol1 = frame.Y1;
            ycol2 = frame.Y2;
            zcol1 = frame.Z1;
            zcol2 = frame.Z2;
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
                frameid = ++frameid % this.frames.Count;
                xoffset = this.frames[frameid].XOffset;
                yoffset = this.frames[frameid].YOffset;
                zoffset = this.frames[frameid].ZOffset;
                xglobal = this.frames[frameid].XGlobal;
                yglobal = this.frames[frameid].YGlobal;
                zglobal = this.frames[frameid].ZGlobal;
                xcol1 = this.frames[frameid].X1;
                xcol2 = this.frames[frameid].X2;
                ycol1 = this.frames[frameid].Y1;
                ycol2 = this.frames[frameid].Y2;
                zcol1 = this.frames[frameid].Z1;
                zcol2 = this.frames[frameid].Z2;
                Refresh();
            };
        }

        protected override int CameraRangeMargin => 256;

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
                        yield return new Position(x,y,z);
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
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
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
                GL.Color3(Color.White);
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
            GL.DepthMask(false);
            GL.Color4(0f, 1f, 0f, 0.2f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            RenderCollisionBox();
            GL.Color4(0f, 1f, 0f, 1f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            RenderCollisionBox();
            GL.DepthMask(true);
        }

        private void RenderCollisionBox()
        {
            GL.PushMatrix();
            //GL.Translate(xoffset,yoffset,zoffset);
            GL.Scale(0.00125f,0.00125f,0.00125f);
            GL.Translate(yglobal*2,yglobal*2,yglobal*2);
            GL.Begin(PrimitiveType.QuadStrip);
            GL.Vertex3(xcol1,ycol1,zcol1);
            GL.Vertex3(xcol1,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol1);
            GL.Vertex3(xcol2,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol2);
            GL.Vertex3(xcol2,ycol2,zcol2);
            GL.Vertex3(xcol1,ycol1,zcol2);
            GL.Vertex3(xcol1,ycol2,zcol2);
            GL.Vertex3(xcol1,ycol1,zcol1);
            GL.Vertex3(xcol1,ycol2,zcol1);
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(xcol1,ycol1,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol1);
            GL.Vertex3(xcol2,ycol1,zcol2);
            GL.Vertex3(xcol1,ycol1,zcol2);

            GL.Vertex3(xcol1,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol2,zcol1);
            GL.Vertex3(xcol2,ycol2,zcol2);
            GL.Vertex3(xcol1,ycol2,zcol2);
            GL.End();
            GL.PopMatrix();
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
