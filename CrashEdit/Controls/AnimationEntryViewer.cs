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
            frameid = 0;
            if (model.Positions != null)
            {
                foreach (Frame frame in frames)
                {
                    this.frames.Add(UncompressFrame(frame));
                    frameid++;
                }
            }
            else
                this.frames = new List<Frame>(frames);
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

        public AnimationEntryViewer(ModelEntry model)
        {
            frames = new List<Frame>();
            frames.Add(ModelToFrame(model));
            this.model = model;
            xoffset = 0;
            yoffset = 0;
            zoffset = 0;
            frameid = 0;
        }

        protected override int CameraRangeMargin
        {
            get
            {
                int i = Math.Max(BitConv.FromInt32(model.Info,0) * 256,BitConv.FromInt32(model.Info,4) * 256);
                return Math.Max(i,BitConv.FromInt32(model.Info,8) * 256);
            }
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (Frame frame in frames)
                {
                    foreach (FrameVertex vertex in frame.Vertices)
                    {
                        int x = vertex.X * BitConv.FromInt32(model.Info,0) + frame.XOffset;
                        int y = vertex.Y * BitConv.FromInt32(model.Info,4) + frame.YOffset;
                        int z = vertex.Z * BitConv.FromInt32(model.Info,8) + frame.ZOffset;
                        yield return new Position(x,y,z);
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
            GL.Vertex3(vertex.X * BitConv.FromInt32(model.Info,0),vertex.Y * BitConv.FromInt32(model.Info,4),vertex.Z * BitConv.FromInt32(model.Info,8));
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
                byte finalx = 0;
                byte finaly = 0;
                byte finalz = 0;
                if (frameid != 0)
                {
                    finalx = (byte)((modelx + vertexx + frames[frameid - 1].Vertices[i].X) % 256);
                    finaly = (byte)((modely + vertexy + frames[frameid - 1].Vertices[i].Y) % 256);
                    finalz = (byte)((modelz + vertexz + frames[frameid - 1].Vertices[i].Z) % 256);
                }
                else
                {
                    finalx = (byte)((modelx + vertexx) % 256);
                    finaly = (byte)((modely + vertexy) % 256);
                    finalz = (byte)((modelz + vertexz) % 256);
                }
                frame.Vertices[i] = new FrameVertex(finalx,finaly,finalz);
            }
            //Frame uncompressedframe = new Frame(xoffset,yoffset,zoffset,frame.Unknown,frame.VertexCount,frame.Collision,frame.ModelEID,frame.HeaderSize,frame.Settings);
            return frame;
        }

        private Frame ModelToFrame(ModelEntry model)
        {
            List<FrameVertex> vertices = new List<FrameVertex>();
            for (int i = 0; i < model.Positions.Count; i++)
            {
                vertices.Add(new FrameVertex(model.Positions[i].X,model.Positions[i].Y,model.Positions[i].Z));
            }
            Frame frame = new Frame(0,0,0,0,BitConv.FromInt32(model.Info,0x38),1,model.EID,24,null,vertices,null);
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
