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
        private ModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private int xoffset;
        private int yoffset;
        private int zoffset;

        public AnimationEntryViewer(Frame frame,ModelEntry model)
        {
            frames = new List<Frame>();
            this.model = model;
            if (model.Positions != null)
                frames.Add(UncompressFrame(frame));
            else
                frames.Add(frame);
            xoffset = frame.XOffset;
            yoffset = frame.YOffset;
            zoffset = frame.ZOffset;
            frameid = 0;
        }

        public AnimationEntryViewer(IEnumerable<Frame> frames,ModelEntry model)
        {
            this.frames = new List<Frame>();
            this.model = model;
            if (model.Positions != null)
            {
                foreach (Frame frame in frames)
                {
                    this.frames.Add(UncompressFrame(frame));
                }
            }
            else
            {
                foreach (Frame frame in frames)
                {
                    this.frames.Add(frame);
                }
            }
            frameid = 0;
            animatetimer = new Timer();
            animatetimer.Interval = 1000/30;
            animatetimer.Enabled = true;
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                frameid++;
                Refresh();
            };
        }

        protected override int CameraRangeMargin
        {
            get { return 400; }
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
            //if (model != null)
            //{
            //    GL.Begin(PrimitiveType.Triangles);
            //    foreach (ModelPolygon polygon in model.Polygons)
            //    {
            //        if (!polygon.ColorSlot)
            //        {

            //        }
            //    }
            //    GL.End();
            //}
            //else
            {
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

        private Frame UncompressFrame(Frame frame)
        {
            //FrameVertex[] uncompressedvertices = new FrameVertex[frame.VertexCount];
            int bit = 0;
            for (int i = 0;i < model.Positions.Count;i++)
            {
                byte modelx = (byte)(model.Positions[i].X * 2);
                if (model.Positions[i].XBits == 7)
                    modelx = 0;
                sbyte vertexx = 0;
                for (int ii = 0; ii < model.Positions[i].XBits; ii++)
                {
                    vertexx |= (sbyte)(Convert.ToByte(frame.Temporals[bit]) << ii);
                    bit++;
                }
                if (frame.Temporals[bit] == true)
                {
                    vertexx -= (sbyte)(1 << model.Positions[i].XBits);
                }
                bit++;
                byte modelz = model.Positions[i].Z;
                if (model.Positions[i].ZBits == 7)
                    modelz = 0;
                sbyte vertexz = 0;
                for (int ii = 0; ii < model.Positions[i].ZBits; ii++)
                {
                    vertexz |= (sbyte)(Convert.ToByte(frame.Temporals[bit]) << ii);
                    bit++;
                }
                if (frame.Temporals[bit] == true)
                {
                    vertexz -= (sbyte)(1 << model.Positions[i].ZBits);
                }
                bit++;
                byte modely = model.Positions[i].Y;
                if (model.Positions[i].YBits == 7)
                    modely = 0;
                sbyte vertexy = 0;
                for (int ii = 0; ii < model.Positions[i].YBits; ii++)
                {
                    vertexy |= (sbyte)(Convert.ToByte(frame.Temporals[bit]) << ii);
                    bit++;
                }
                if (frame.Temporals[bit] == true)
                {
                    vertexy -= (sbyte)(1 << model.Positions[i].YBits);
                }
                bit++;
                byte finalx = (byte)((modelx + vertexx) % 256);
                byte finalz = (byte)((modelz + vertexz) % 256);
                byte finaly = (byte)((modely + vertexy) % 256);
                frame.Vertices[i] = new FrameVertex(finalx,finaly,finalz);
            }
            //Frame uncompressedframe = new Frame(xoffset,yoffset,zoffset,frame.Unknown,frame.VertexCount,frame.Collision,frame.ModelEID,frame.HeaderSize,frame.Settings);
            return frame;
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
