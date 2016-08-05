using Crash;
using System;
using System.Drawing;
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
        private short xoffset;
        private short yoffset;
        private short zoffset;

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
            xoffset = this.frames[frameid].XOffset;
            yoffset = this.frames[frameid].YOffset;
            zoffset = this.frames[frameid].ZOffset;
            animatetimer = new Timer();
            animatetimer.Interval = 1000/30;
            animatetimer.Enabled = true;
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
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
                int i = Math.Max(BitConv.FromInt32(model.Info,0) * 64,BitConv.FromInt32(model.Info,4) * 64);
                return Math.Max(i,BitConv.FromInt32(model.Info,8) * 64);
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
                        int x = (vertex.X + frame.XOffset / 4) * BitConv.FromInt32(model.Info,0);
                        int y = (vertex.Z + frame.YOffset / 4) * BitConv.FromInt32(model.Info,4);
                        int z = (vertex.Y + frame.ZOffset / 4) * BitConv.FromInt32(model.Info,8);
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
            if (model != null)
            {
                int i = 0;
                int ii = 0;
                int vertexid = 0;
                int tris = 0;
                for (int iii = 0;iii < model.Polygons.Count;iii++)
                {
                    if (!model.Polygons[iii].ColorSlot && !model.Polygons[iii].Footer) tris++;
                }
                int[] indexes = new int[tris];
                GL.Begin(PrimitiveType.Triangles);
                foreach (ModelPolygon polygon in model.Polygons)
                {
                    if (!polygon.ColorSlot && !polygon.Footer)
                    {
                        if ((polygon.StructType & 0x4) == 0)
                        {
                            indexes[ii] = vertexid;
                            vertexid++;
                        }
                        else
                        {
                            int j = 1;
                            while (model.Polygons[i - j].Pointer != polygon.Pointer || model.Polygons[i - j].Pointer == 0x57 || ((model.Polygons[i - j].StructType & 4) == 4) || model.Polygons[i - j].ColorSlot)
                            {
                                j++;
                            }
                            int k = 0;
                            int l = 0;
                            while (i - j - l > -1)
                            {
                                if (model.Polygons[i - j - l].ColorSlot) k++;
                                l++;
                            }
                            indexes[ii] = indexes[i - j - k];
                        }
                        ii++;
                    }
                    i++;
                }
                i = 0;
                ii = 0;
                byte invalid = 0;
                foreach (ModelPolygon polygon in model.Polygons)
                {
                    byte r = 0;
                    byte g = 0;
                    byte b = 0;
                    int vertexa = 0;
                    int vertexb = 0;
                    int vertexc = 0;
                    if (!polygon.ColorSlot && !polygon.Footer)
                    {
                        GL.Color3(Color.Fuchsia);
                        vertexa = indexes[ii];
                        //Vertex IDs
                        if (polygon.TriType < 4) //AA
                        {
                            vertexb = indexes[ii - 1];
                            vertexc = indexes[ii - 2];
                        }
                        else if (polygon.TriType < 8) //BB
                        {
                            vertexb = indexes[ii - 1];
                            int j = 1;
                            while ((model.Polygons[i - j].TriType > 3 && model.Polygons[i - j].TriType < 8) || model.Polygons[i - j].ColorSlot)
                            {
                                j++;
                            }
                            int k = 0;
                            int l = 0;
                            while (i - j - l > -1)
                            {
                                if (model.Polygons[i - j - l].ColorSlot) k++;
                                l++;
                            }
                            if (model.Polygons[i - j].TriType < 4) //AA
                                vertexc = indexes[i - j - k - 2];
                            else if (invalid == 0) //CC
                                vertexc = indexes[i - j - k];
                            else
                                vertexc = 0;
                        }
                        else if (invalid == 0) //CC
                        {
                            vertexb = indexes[ii + 1];
                            vertexc = indexes[ii + 2];
                        }
                        else //Invalid CC
                        {
                            vertexa = 0;
                            vertexb = 0;
                            vertexc = 0;
                        }
                        //Vertex Colors
                        //Vertex A
                        //AA
                        //BB
                        //CC
                        r = model.Colors[polygon.ColorIndex].Red;
                        g = model.Colors[polygon.ColorIndex].Green;
                        b = model.Colors[polygon.ColorIndex].Blue;
                        GL.Color3(r,g,b);
                        RenderVertex(frame.Vertices[vertexa]);
                        GL.Color3(Color.Fuchsia);
                        //Vertex B
                        //AA
                        //BB
                        if (polygon.TriType < 8)
                        {
                            if (model.Polygons[i - 1].ColorSlot)
                            {
                                r = model.Colors[model.Polygons[i - 1].ColorA].Red;
                                g = model.Colors[model.Polygons[i - 1].ColorA].Green;
                                b = model.Colors[model.Polygons[i - 1].ColorA].Blue;
                            }
                            else
                            {
                                r = model.Colors[model.Polygons[i - 1].ColorIndex].Red;
                                g = model.Colors[model.Polygons[i - 1].ColorIndex].Green;
                                b = model.Colors[model.Polygons[i - 1].ColorIndex].Blue;
                            }
                        }
                        //CC
                        else if(invalid == 0)
                        {
                            if (!model.Polygons[i + 1].ColorSlot)
                            {
                                r = model.Colors[model.Polygons[i + 1].ColorIndex].Red;
                                g = model.Colors[model.Polygons[i + 1].ColorIndex].Green;
                                b = model.Colors[model.Polygons[i + 1].ColorIndex].Blue;
                            }
                            else
                            {
                                r = model.Colors[polygon.ColorIndex].Red;
                                g = model.Colors[polygon.ColorIndex].Green;
                                b = model.Colors[polygon.ColorIndex].Blue;
                            }
                        }
                        GL.Color3(r,g,b);
                        RenderVertex(frame.Vertices[vertexb]);
                        GL.Color3(Color.Fuchsia);
                        //Vertex C
                        //AA
                        if (polygon.TriType < 4)
                        {
                            if (model.Polygons[i - 1].ColorSlot)
                            {
                                r = model.Colors[model.Polygons[i - 1].ColorB].Red;
                                g = model.Colors[model.Polygons[i - 1].ColorB].Green;
                                b = model.Colors[model.Polygons[i - 1].ColorB].Blue;
                            }
                            else if (model.Polygons[i - 2].ColorSlot)
                            {
                                r = model.Colors[model.Polygons[i - 2].ColorA].Red;
                                g = model.Colors[model.Polygons[i - 2].ColorA].Green;
                                b = model.Colors[model.Polygons[i - 2].ColorA].Blue;
                            }
                            //else
                            //{
                            //    r = model.Colors[polygon.ColorIndex].Red;
                            //    g = model.Colors[polygon.ColorIndex].Green;
                            //    b = model.Colors[polygon.ColorIndex].Blue;
                            //}
                            else
                            {
                                int j = 2;
                                while (model.Polygons[i - j].ColorSlot)
                                    j++;
                                r = model.Colors[model.Polygons[i - j].ColorIndex].Red;
                                g = model.Colors[model.Polygons[i - j].ColorIndex].Green;
                                b = model.Colors[model.Polygons[i - j].ColorIndex].Blue;
                            }
                        }
                        //BB
                        else if (polygon.TriType < 8)
                        {
                            int j = 1;
                            while (model.Polygons[i - j].TriType > 3 && model.Polygons[i - j].TriType < 8)
                                j++;
                            int k = 1;
                            while (!model.Polygons[i - k].ColorSlot)
                                k++;
                            if (j + 2 < k) //Scenario 3
                            {
                                r = model.Colors[model.Polygons[i - j - 2].ColorIndex].Red;
                                g = model.Colors[model.Polygons[i - j - 2].ColorIndex].Green;
                                b = model.Colors[model.Polygons[i - j - 2].ColorIndex].Blue;
                            }
                            if (j + 2 == k) //Scenario 1
                            {
                                r = model.Colors[model.Polygons[i - k].ColorA].Red;
                                g = model.Colors[model.Polygons[i - k].ColorA].Green;
                                b = model.Colors[model.Polygons[i - k].ColorA].Blue;
                            }
                            if (j + 1 == k) //Scenario 2
                            {
                                r = model.Colors[model.Polygons[i - k].ColorB].Red;
                                g = model.Colors[model.Polygons[i - k].ColorB].Green;
                                b = model.Colors[model.Polygons[i - k].ColorB].Blue;
                            }
                            if (j + 2 > k) //Scenario 4
                            {
                                r = model.Colors[model.Polygons[i - k].ColorB].Red;
                                g = model.Colors[model.Polygons[i - k].ColorB].Green;
                                b = model.Colors[model.Polygons[i - k].ColorB].Blue;
                            }
                        }
                        //CC
                        else if (invalid == 0)
                        {
                            r = model.Colors[polygon.ColorIndex].Red;
                            g = model.Colors[polygon.ColorIndex].Green;
                            b = model.Colors[polygon.ColorIndex].Blue;
                            if (!model.Polygons[i + 2].ColorSlot)
                            {
                                r = model.Colors[model.Polygons[i + 2].ColorIndex].Red;
                                g = model.Colors[model.Polygons[i + 2].ColorIndex].Green;
                                b = model.Colors[model.Polygons[i + 2].ColorIndex].Blue;
                            }
                            else if (!model.Polygons[i + 1].ColorSlot)
                            {
                                r = model.Colors[model.Polygons[i + 1].ColorIndex].Red;
                                g = model.Colors[model.Polygons[i + 1].ColorIndex].Green;
                                b = model.Colors[model.Polygons[i + 1].ColorIndex].Blue;
                            }
                        }
                        GL.Color3(r,g,b);
                        RenderVertex(frame.Vertices[vertexc]);
                        ii++;
                        if (invalid > 0) invalid--;
                        else if (polygon.TriType > 7) invalid = 2;
                    }
                    i++;
                }
                GL.End();
            }
            else
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
            GL.Vertex3((vertex.X + xoffset / 4) * BitConv.FromInt32(model.Info,0),(vertex.Z + yoffset / 4) * BitConv.FromInt32(model.Info,4),(vertex.Y + zoffset / 4) * BitConv.FromInt32(model.Info,8));
        }

        private Frame UncompressFrame(Frame frame)
        {
            //FrameVertex[] uncompressedvertices = new FrameVertex[frame.VertexCount];
            int bit = 0;
            int modelxcum = 0;
            int modelycum = 0;
            int modelzcum = 0;
            for (int i = 0; i < model.Positions.Count; i++)
            {
                byte modelx = (byte)(model.Positions[i].X * 2);
                if (model.Positions[i].XBits == 7) { modelx = 0; modelxcum = 0; }
                sbyte vertexx = 0;
                bit += model.Positions[i].XBits;
                for (int ii = 0; ii < model.Positions[i].XBits; ii++)
                {
                    vertexx |= (sbyte)(Convert.ToByte(frame.Temporals[bit]) << ii);
                    bit--;
                }
                if (frame.Temporals[bit] == true)
                {
                    vertexx -= (sbyte)(1 << model.Positions[i].XBits);
                }
                bit += model.Positions[i].XBits + 1;
                byte modelz = model.Positions[i].Z;
                if (model.Positions[i].ZBits == 7) { modelz = 0; modelzcum = 0; }
                sbyte vertexz = 0;
                bit += model.Positions[i].ZBits;
                for (int ii = 0; ii < model.Positions[i].ZBits; ii++)
                {
                    vertexz |= (sbyte)(Convert.ToByte(frame.Temporals[bit]) << ii);
                    bit--;
                }
                if (frame.Temporals[bit] == true)
                {
                    vertexz -= (sbyte)(1 << model.Positions[i].ZBits);
                }
                bit += model.Positions[i].ZBits + 1;
                byte modely = model.Positions[i].Y;
                if (model.Positions[i].YBits == 7) { modely = 0; modelycum = 0; }
                sbyte vertexy = 0;
                bit += model.Positions[i].YBits;
                for (int ii = 0; ii < model.Positions[i].YBits; ii++)
                {
                    vertexy |= (sbyte)(Convert.ToByte(frame.Temporals[bit]) << ii);
                    bit--;
                }
                if (frame.Temporals[bit] == true)
                {
                    vertexy -= (sbyte)(1 << model.Positions[i].YBits);
                }
                bit += model.Positions[i].YBits + 1;
                byte finalx = 0;
                byte finaly = 0;
                byte finalz = 0;

                finalx = (byte)((modelxcum + modelx + vertexx) % 256);
                finaly = (byte)((modelycum + modely + vertexy) % 256);
                finalz = (byte)((modelzcum + modelz + vertexz) % 256);

                modelxcum += (byte)(modelx + vertexx);
                modelycum += (byte)(modely + vertexy);
                modelzcum += (byte)(modelz + vertexz);
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
