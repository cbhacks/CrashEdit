using Crash;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryViewer : ThreeDimensionalViewer
    {
        private List<OldFrame> frames;
        private List<ProtoFrame> protoframes;
        private OldModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private bool isproto;

        public OldAnimationEntryViewer(ProtoFrame frame,OldModelEntry model)
        {
            protoframes = new List<ProtoFrame> { frame };
            frames = new List<OldFrame>();
            this.model = model;
            frameid = 0;
            isproto = true;
        }

        public OldAnimationEntryViewer(IEnumerable<ProtoFrame> frames,OldModelEntry model)
        {
            protoframes = new List<ProtoFrame>(frames);
            this.frames = new List<OldFrame>();
            this.model = model;
            frameid = 0;
            isproto = true;
            animatetimer = new Timer();
            animatetimer.Interval = 1000 / OldMainForm.GetRate();
            animatetimer.Enabled = true;
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = 1000 / OldMainForm.GetRate();
                frameid = ++frameid % protoframes.Count;
                Refresh();
            };
        }

        public OldAnimationEntryViewer(OldFrame frame,OldModelEntry model)
        {
            protoframes = new List<ProtoFrame>();
            frames = new List<OldFrame>() { frame };
            this.model = model;
            frameid = 0;
            isproto = false;
        }

        public OldAnimationEntryViewer(IEnumerable<OldFrame> frames,OldModelEntry model)
        {
            protoframes = new List<ProtoFrame>();
            this.frames = new List<OldFrame>(frames);
            this.model = model;
            frameid = 0;
            isproto = false;
            animatetimer = new Timer();
            animatetimer.Interval = 1000 / OldMainForm.GetRate();
            animatetimer.Enabled = true;
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = 1000 / OldMainForm.GetRate();
                frameid = ++frameid % this.frames.Count;
                Refresh();
            };
        }

        // Final animation scale is tiny,
        // we need to increase it to stay consistent
        // with other viewers which have a larger scale.
        protected override float ScaleFactor => 16;

        protected override int CameraRangeMargin => 64;

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
                foreach (ProtoFrame frame in protoframes)
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

        protected override void RenderObjects()
        {
            if (isproto)
                RenderFrame(protoframes[frameid % protoframes.Count]);
            else
                RenderFrame(frames[frameid % frames.Count]);
        }

        private void RenderFrame(ProtoFrame frame)
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
                    RenderVertex(frame, frame.Vertices[polygon.VertexA / 6]);
                    RenderVertex(frame, frame.Vertices[polygon.VertexB / 6]);
                    RenderVertex(frame, frame.Vertices[polygon.VertexC / 6]);
                }
                GL.End();
            }
            else
            {
                GL.Color3(Color.White);
                GL.Begin(PrimitiveType.Points);
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    RenderVertex(frame, vertex);
                }
                GL.End();
            }
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
                    RenderVertex(frame, frame.Vertices[polygon.VertexA / 6]);
                    RenderVertex(frame, frame.Vertices[polygon.VertexB / 6]);
                    RenderVertex(frame, frame.Vertices[polygon.VertexC / 6]);
                }
                GL.End();
            }
            else
            {
                GL.Color3(Color.White);
                GL.Begin(PrimitiveType.Points);
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    RenderVertex(frame, vertex);
                }
                GL.End();
            }
            //RenderCollision(frame);
        }

        private void RenderVertex(ProtoFrame frame, OldFrameVertex vertex)
        {
            GL.Vertex3(vertex.X + frame.XOffset,vertex.Y + frame.YOffset,vertex.Z + frame.ZOffset);
        }

        private void RenderVertex(OldFrame frame, OldFrameVertex vertex)
        {
            GL.Vertex3(vertex.X + frame.XOffset,vertex.Y + frame.YOffset,vertex.Z + frame.ZOffset);
        }

        private void RenderCollision(OldFrame frame)
        {
            GL.DepthMask(false);
            GL.Color4(0f, 1f, 0f, 0.2f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            RenderCollisionBox(frame);
            GL.Color4(0f, 1f, 0f, 1f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            RenderCollisionBox(frame);
            GL.DepthMask(true);
        }

        private void RenderCollisionBox(OldFrame frame)
        {
            int xoffset = frame.XOffset;
            int yoffset = frame.YOffset;
            int zoffset = frame.ZOffset;
            int xglobal = frame.XGlobal;
            int yglobal = frame.YGlobal;
            int zglobal = frame.ZGlobal;
            int xcol1 = frame.X1;
            int xcol2 = frame.X2;
            int ycol1 = frame.Y1;
            int ycol2 = frame.Y2;
            int zcol1 = frame.Z1;
            int zcol2 = frame.Z2;
            GL.PushMatrix();
            //GL.Translate(xoffset,yoffset,zoffset);
            GL.Scale(0.00125f,0.00125f,0.00125f);
            GL.Translate(xglobal*2,yglobal*2,zglobal*2);
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
