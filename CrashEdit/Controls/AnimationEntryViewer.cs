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
        private static readonly int[] SignTable = { -1, -2, -4, -8, -16, -32, -64, -128 }; // used for decompression

        private static int textureframe = 0;
        private static Timer texturetimer;

        private List<Frame> frames;
        private ModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private int interi;
        private int interp = 2;
        private bool collision_enabled = false;
        private bool textures_enabled = true;

        static AnimationEntryViewer()
        {
            texturetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate(),
                Enabled = true
            };
            texturetimer.Tick += delegate (object sender,EventArgs e)
            {
                textureframe = ++textureframe % 128;
            };
        }

        public AnimationEntryViewer(Frame frame,ModelEntry model,TextureChunk[] texturechunks)
        {
            frames = new List<Frame>();
            this.model = model;
            ConvertTexturesToGL(texturechunks, model.Textures, model.Info, 0xC);
            if (model.Positions != null) // fix this later
                frames.Add(UncompressFrame(frame));
            else
                frames.Add(LoadFrame(frame));
            frameid = 0;
            interi = 0;
        }

        public AnimationEntryViewer(IEnumerable<Frame> frames,ModelEntry model,TextureChunk[] texturechunks)
        {
            this.frames = new List<Frame>();
            this.model = model;
            ConvertTexturesToGL(texturechunks, model.Textures, model.Info, 0xC);
            frameid = 0;
            interi = 0;
            if (model.Positions != null)
            {
                foreach (Frame frame in frames)
                {
                    this.frames.Add(UncompressFrame(frame));
                    frameid++;
                }
            }
            else
            {
                foreach (Frame frame in frames)
                {
                    this.frames.Add(LoadFrame(frame));
                    frameid++;
                }
            }
            frameid = 0;
            animatetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate() / interp,
                Enabled = true
            };
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = 1000 / OldMainForm.GetRate() / interp;
                interi = ++interi % interp;
                frameid = (frameid + (interi == 1 ? 1 : 0)) % this.frames.Count;
                Refresh();
            };
        }
        
        private int MinScale => model != null ? Math.Min(BitConv.FromInt32(model.Info, 8), Math.Min(BitConv.FromInt32(model.Info, 0), BitConv.FromInt32(model.Info, 4))) : 0x1000;
        private int MaxScale => model != null ? Math.Max(BitConv.FromInt32(model.Info, 8), Math.Max(BitConv.FromInt32(model.Info, 0), BitConv.FromInt32(model.Info, 4))) : 0x1000;

        protected override int CameraRangeMargin => 200000;

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (Frame frame in frames)
                {
                    if (!frame.Decompressed)
                    {
                        if (model.Positions == null)
                            UncompressFrame(frame);
                        else
                            LoadFrame(frame);
                    }
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
            if (interi == 0 || frameid == 0)
            {
                RenderFrame(frames[frameid]);
            }
            else
            {
                RenderFrame(frames[frameid-1], frames[frameid]);
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.C:
                case Keys.T:
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
                case Keys.C:
                    collision_enabled = !collision_enabled;
                    break;
                case Keys.T:
                    textures_enabled = !textures_enabled;
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.CombineRgb, (int)TextureEnvModeCombine.Modulate);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.RgbScale, 2.0f);
        }

        private void RenderFrame(Frame f1)
        {
            RenderFrame(f1, null);
        }

        private void RenderFrame(Frame f1, Frame f2)
        {
            //LoadTexture(OldResources.PointTexture);
            //RenderPoints(f2);
            if (model != null)
            {
                GL.PushMatrix();
                GL.Scale(BitConv.FromInt32(model.Info,0), BitConv.FromInt32(model.Info,4), BitConv.FromInt32(model.Info,8));
                if (textures_enabled)
                {
                    float[] uvs = new float[6];
                    for (int i = 0; i < model.Triangles.Count; ++i)
                    {
                        var tri = model.Triangles[i];
                        if (tri.Tex != 0)
                        {
                            //GL.Enable(EnableCap.Texture2D);
                            int tex = tri.Tex - 1;
                            //LoadTexture(bitmaps[tex]);
                            if (!tri.Animated)
                                GL.BindTexture(TextureTarget.Texture2D, textures[tex]);
                            //else
                            //{
                            //    var anim = model.AnimatedTextures[tex];
                            //    if (!anim.IsLOD && !anim.Leap && anim.Offset != 0)
                            //    {
                            //        tex = anim.Offset - 1 + (textureframe & anim.Mask);
                            //        GL.BindTexture(TextureTarget.Texture2D, textures[tex]);
                            //    }
                            //    else
                            //        GL.BindTexture(TextureTarget.Texture2D, 0);
                            //}
                            switch (tri.Type)
                            {
                                case 0:
                                case 1:
                                    uvs[0] = model.Textures[tex].X3;
                                    uvs[1] = model.Textures[tex].Y3;
                                    uvs[4] = model.Textures[tex].X1;
                                    uvs[5] = model.Textures[tex].Y1;
                                    break;
                                case 2:
                                    uvs[0] = model.Textures[tex].X1;
                                    uvs[1] = model.Textures[tex].Y1;
                                    uvs[4] = model.Textures[tex].X3;
                                    uvs[5] = model.Textures[tex].Y3;
                                    break;
                            }
                            uvs[2] = model.Textures[tex].X2;
                            uvs[3] = model.Textures[tex].Y2;
                        }
                        else
                            GL.BindTexture(TextureTarget.Texture2D, 0);
                        GL.Begin(PrimitiveType.Triangles);
                        for (int j = 0; j < 3; ++j)
                        {
                            int c = tri.Color[j];
                            GL.Color3(model.Colors[c].Red, model.Colors[c].Green, model.Colors[c].Blue);
                            GL.TexCoord2(uvs[2 * j + 0], uvs[2 * j + 1]);
                            RenderVertex(f1,f2,tri.Vertex[j] + f1.SpecialVertexCount,f2 != null ? tri.Vertex[j] + f2.SpecialVertexCount : 0);
                        }
                        GL.End();
                    }
                }
                else
                {
                    GL.Begin(PrimitiveType.Triangles);
                    for (int i = 0; i < model.Triangles.Count; ++i)
                    {
                        var tri = model.Triangles[i];
                        for (int j = 0; j < 3; ++j)
                        {
                            int c = tri.Color[j];
                            GL.Color3(model.Colors[c].Red, model.Colors[c].Green, model.Colors[c].Blue);
                            RenderVertex(f1,f2,tri.Vertex[j] + f1.SpecialVertexCount,f2 != null ? tri.Vertex[j] + f2.SpecialVertexCount : 0);
                        }
                    }
                    GL.End();
                }
                GL.PopMatrix();
            }
            else
            {
                GL.Color3(Color.White);
                GL.Begin(PrimitiveType.Points);
                for (int i = 0; i < f1.Vertices.Count; ++i)
                {
                    RenderVertex(f1,f2,i,i);
                }
                GL.End();
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
            if (collision_enabled)
            {
                Frame fcol;
                if (f2 == null)
                    fcol = f1;
                else
                    fcol = f2;
                for (int i = 0; i < fcol.Collision; ++i)
                {
                    RenderCollision(fcol, i);
                }
            }
        }

        private void RenderPoints(Frame f)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.Fuchsia);
            for (int i = 0; i < f.SpecialVertexCount; ++i)
            {
                GL.PushMatrix();
                GL.Translate((f.Vertices[i].X + f.XOffset / 4) * BitConv.FromInt32(model.Info,0),(f.Vertices[i].Z + f.YOffset / 4) * BitConv.FromInt32(model.Info,4),(f.Vertices[i].Y + f.ZOffset / 4) * BitConv.FromInt32(model.Info,8));
                GL.Rotate(-rotx, 0, 1, 0);
                GL.Rotate(-roty, 1, 0, 0);
                GL.Scale(12.8f*MinScale,12.8f*MinScale,12.8f*MinScale);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0,0);
                GL.Vertex2(-1,+1);
                GL.TexCoord2(1,0);
                GL.Vertex2(+1,+1);
                GL.TexCoord2(1,1);
                GL.Vertex2(+1,-1);
                GL.TexCoord2(0,1);
                GL.Vertex2(-1,-1);
                GL.End();
                GL.PopMatrix();
            }
            GL.Disable(EnableCap.Texture2D);
        }

        private void RenderVertex(Frame f1, Frame f2, int id1, int id2)
        {
            if (f2 == null)
            {
                FrameVertex vertex = f1.Vertices[id1];
                GL.Vertex3(vertex.X + f1.XOffset / 4, vertex.Z + f1.YOffset / 4, vertex.Y + f1.ZOffset / 4);
            }
            else
            {
                FrameVertex v1 = f1.Vertices[id1];
                FrameVertex v2 = f2.Vertices[id2];
                int x1 = v1.X + f1.XOffset / 4;
                int x2 = v2.X + f2.XOffset / 4;
                int y1 = v1.Z + f1.YOffset / 4;
                int y2 = v2.Z + f2.YOffset / 4;
                int z1 = v1.Y + f1.ZOffset / 4;
                int z2 = v2.Y + f2.ZOffset / 4;
                GL.Vertex3(x1 + (x2 - x1) / (float)interp * interi, y1 + (y2 - y1) / (float)interp * interi, z1 + (z2 - z1) / (float)interp * interi);
            }
        }

        private void UncompressFrame(ref Frame frame)
        {
            frame = UncompressFrame(frame);
        }

        private Frame UncompressFrame(Frame frame)
        {
            if (frame.Decompressed) return frame; // already decompressed
            int x_alu = 0;
            int y_alu = 0;
            int z_alu = 0;
            int bi = frame.SpecialVertexCount * 8 * 3;
            for (int i = 0; i < model.Positions.Count; ++i)
            {
                int bx = model.Positions[i].X << 1;
                int by = model.Positions[i].Y;
                int bz = model.Positions[i].Z;
                if (model.Positions[i].XBits == 7)
                {
                    x_alu = 0;
                }
                if (model.Positions[i].YBits == 7)
                {
                    y_alu = 0;
                }
                if (model.Positions[i].ZBits == 7)
                {
                    z_alu = 0;
                }
                
                // XZY frame data
                int vx = frame.Temporals[bi++] ? SignTable[model.Positions[i].XBits] : 0;
                for (int j = 0; j < model.Positions[i].XBits; ++j)
                {
                    vx |= Convert.ToByte(frame.Temporals[bi++]) << (model.Positions[i].XBits - 1 - j);
                }

                int vz = frame.Temporals[bi++] ? SignTable[model.Positions[i].ZBits] : 0;
                for (int j = 0; j < model.Positions[i].ZBits; ++j)
                {
                    vz |= Convert.ToByte(frame.Temporals[bi++]) << (model.Positions[i].ZBits - 1 - j);
                }

                int vy = frame.Temporals[bi++] ? SignTable[model.Positions[i].YBits] : 0;
                for (int j = 0; j < model.Positions[i].YBits; ++j)
                {
                    vy |= Convert.ToByte(frame.Temporals[bi++]) << (model.Positions[i].YBits - 1 - j);
                }

                x_alu = (x_alu + vx + bx) % 256;
                y_alu = (y_alu + vy + by) % 256;
                z_alu = (z_alu + vz + bz) % 256;

                frame.Vertices[i] = new FrameVertex((byte)x_alu, (byte)y_alu, (byte)z_alu);
            }
            frame.Decompressed = true;
            return frame;
        }

        private Frame LoadFrame(Frame frame)
        {
            if (frame.Decompressed) return frame; // already loaded (no decompression was necessary though)
            bool[] uncompressedbitstream = new bool[frame.Temporals.Length];
            for (int i = 0; i < uncompressedbitstream.Length / 32;++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    for (int k = 0; k < 8; ++k)
                    {
                        uncompressedbitstream[32*i+24-j*8+k] = frame.Temporals[32*i+j*8+k]; // replace this with a cool formula one day
                    }
                }
            }
            int bi = frame.SpecialVertexCount * 8 * 3;
            for (int i = frame.SpecialVertexCount; i < frame.Vertices.Count; ++i)
            {
                byte x = 0;
                for (int j = 0; j < 8; ++j)
                {
                    x |= (byte)(Convert.ToByte(uncompressedbitstream[bi++]) << (7-j));
                }
                
                byte y = 0;
                for (int j = 0; j < 8; ++j)
                {
                    y |= (byte)(Convert.ToByte(uncompressedbitstream[bi++]) << (7-j));
                }
                
                byte z = 0;
                for (int j = 0; j < 8; ++j)
                {
                    z |= (byte)(Convert.ToByte(uncompressedbitstream[bi++]) << (7-j));
                }
                
                frame.Vertices[i] = new FrameVertex(x,y,z);
            }
            frame.Decompressed = true;
            return frame;
        }

        private void RenderCollision(Frame frame, int col)
        {
            GL.DepthMask(false);
            GL.Color4(0f, 1f, 0f, 0.2f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            RenderCollisionBox(frame, col);
            GL.Color4(0f, 1f, 0f, 1f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            RenderCollisionBox(frame, col);
            GL.DepthMask(true);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private void RenderCollisionBox(Frame frame, int col)
        {
            int xglobal = BitConv.FromInt32(frame.Settings,4+col*40);
            int yglobal = BitConv.FromInt32(frame.Settings,8+col*40);
            int zglobal = BitConv.FromInt32(frame.Settings,12+col*40);
            int xcol1 = BitConv.FromInt32(frame.Settings,16+col*40);
            int ycol1 = BitConv.FromInt32(frame.Settings,20+col*40);
            int zcol1 = BitConv.FromInt32(frame.Settings,24+col*40);
            int xcol2 = BitConv.FromInt32(frame.Settings,28+col*40);
            int ycol2 = BitConv.FromInt32(frame.Settings,32+col*40);
            int zcol2 = BitConv.FromInt32(frame.Settings,36+col*40);
            GL.PushMatrix();
            GL.Scale(4, 4, 4);
            GL.Translate(xglobal, yglobal, zglobal);
            GL.Begin(PrimitiveType.QuadStrip);
            GL.Vertex3(xcol1, ycol1, zcol1);
            GL.Vertex3(xcol1, ycol2, zcol1);
            GL.Vertex3(xcol2, ycol1, zcol1);
            GL.Vertex3(xcol2, ycol2, zcol1);
            GL.Vertex3(xcol2, ycol1, zcol2);
            GL.Vertex3(xcol2, ycol2, zcol2);
            GL.Vertex3(xcol1, ycol1, zcol2);
            GL.Vertex3(xcol1, ycol2, zcol2);
            GL.Vertex3(xcol1, ycol1, zcol1);
            GL.Vertex3(xcol1, ycol2, zcol1);
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(xcol1, ycol1, zcol1);
            GL.Vertex3(xcol2, ycol1, zcol1);
            GL.Vertex3(xcol2, ycol1, zcol2);
            GL.Vertex3(xcol1, ycol1, zcol2);

            GL.Vertex3(xcol1, ycol2, zcol1);
            GL.Vertex3(xcol2, ycol2, zcol1);
            GL.Vertex3(xcol2, ycol2, zcol2);
            GL.Vertex3(xcol1, ycol2, zcol2);
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
