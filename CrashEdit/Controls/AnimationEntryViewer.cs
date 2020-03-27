using Crash;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryViewer : ThreeDimensionalViewer
    {
        private static readonly int[] SignTable = { -1, -2, -4, -8, -16, -32, -64, -128 }; // used for decompression

        private bool init = false;
        private TextureChunk[] texturechunks;

        private List<Frame> frames;
        private ModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private int interi;
        private int interp = 2;
        private bool collision_enabled;
        private bool textures_enabled = true;
        private bool normals_enabled = false;
        private bool interp_startend = false;
        private int cullmode = 0;

        public AnimationEntryViewer(Frame frame,ModelEntry model,TextureChunk[] texturechunks)
        {
            collision_enabled = Properties.Settings.Default.DisplayFrameCollision;
            frames = new List<Frame>();
            this.model = model;
            if (model != null && model.Positions != null) // FIXME this later
                frames.Add(UncompressFrame(frame));
            else
                frames.Add(LoadFrame(frame));
            frameid = 0;
            interi = 0;
            this.texturechunks = texturechunks;
            InitTextures(1);
        }

        public AnimationEntryViewer(IEnumerable<Frame> frames,ModelEntry model,TextureChunk[] texturechunks)
        {
            collision_enabled = Properties.Settings.Default.DisplayFrameCollision;
            this.frames = new List<Frame>();
            this.model = model;
            interi = 0;
            if (model != null && model.Positions != null)
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
                    this.frames.Add(LoadFrame(frame));
                }
            }
            this.texturechunks = texturechunks;
            InitTextures(1);
            frameid = 0;
            animatetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate() / interp,
                Enabled = true
            };
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = (int)(1000.0f / OldMainForm.GetRate() / interp);
                ++interi;
                if (interi == interp)
                {
                    interi = 0;
                    frameid = (frameid + 1) % this.frames.Count;
                }
                Refresh();
            };
        }
        
        private int MinScale => model != null ? Math.Min(BitConv.FromInt32(model.Info, 8), Math.Min(BitConv.FromInt32(model.Info, 0), BitConv.FromInt32(model.Info, 4))) : 0x1000;
        private int MaxScale => model != null ? Math.Max(BitConv.FromInt32(model.Info, 8), Math.Max(BitConv.FromInt32(model.Info, 0), BitConv.FromInt32(model.Info, 4))) : 0x1000;

        protected override int CameraRangeMargin => 256;
        protected override float ScaleFactor => 12;
        protected override float FarPlane => (float)MaxScale/MinScale*256*4;
        protected override float NearPlane => (float)MaxScale/MinScale;

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var vec = model != null ? new Vector3(BitConv.FromInt32(model.Info,0),BitConv.FromInt32(model.Info,4),BitConv.FromInt32(model.Info,8))/MinScale : new Vector3(1,1,1);
                foreach (Frame frame in frames)
                {
                    foreach (FrameVertex vertex in frame.Vertices)
                    {
                        float f = frame.IsNew ? 4.0f * 8 : 4.0f;
                        float x = (vertex.X + frame.XOffset / f) * vec.X;
                        float y = (vertex.Z + frame.YOffset / f) * vec.Y;
                        float z = (vertex.Y + frame.ZOffset / f) * vec.Z;
                        yield return new Position(x,y,z);
                    }
                }
            }
        }

        protected override void RenderObjects()
        {
            if (!init && model != null)
            {
                init = true;
                ConvertTexturesToGL(0, texturechunks, model.Textures);
            }
            if (normals_enabled)
            {
                GL.Enable(EnableCap.Lighting);
            }
            if ((frameid + 1) == frames.Count)
            {
                if (interp_startend)
                {
                    RenderFrame(frames[frameid], frames[0]);
                }
                else
                {
                    RenderFrame(frames[frameid]);
                    interi = interp - 1;
                }
            }
            else
            {
                RenderFrame(frames[frameid], frames[frameid+1]);
            }
            if (normals_enabled)
            {
                GL.Disable(EnableCap.Lighting);
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.C:
                case Keys.I:
                case Keys.N:
                case Keys.T:
                case Keys.U:
                case Keys.Enter:
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
                case Keys.I:
                    interp_startend = !interp_startend;
                    break;
                case Keys.N:
                    normals_enabled = !normals_enabled;
                    break;
                case Keys.T:
                    textures_enabled = !textures_enabled;
                    break;
                case Keys.U:
                    cullmode = ++cullmode % 3;
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.RgbScale, 2.0f);
        }

        private void RenderFrame(Frame f1, Frame f2 = null)
        {
            //LoadTexture(OldResources.PointTexture);
            //RenderPoints(f2);
            if (model != null)
            {
                if (cullmode < 2)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(cullFaceModes[cullmode]);
                }
                if (textures_enabled)
                    GL.Enable(EnableCap.Texture2D);
                else
                    GL.Disable(EnableCap.Texture2D);
                GL.PushMatrix();
                GL.Scale(new Vector3(BitConv.FromInt32(model.Info,0),BitConv.FromInt32(model.Info,4),BitConv.FromInt32(model.Info,8))/MinScale);
                float[] uvs = new float[6];
                foreach (ModelTransformedTriangle tri in model.Triangles)
                {
                    if (tri.Tex != 0 || tri.Animated)
                    {
                        bool untex = false;
                        int tex = tri.Tex - 1;
                        if (tri.Animated)
                        {
                            ++tex;
                            var anim = model.AnimatedTextures[tex];
                            if (anim.Offset == 0)
                                untex = true;
                            else if (anim.IsLOD)
                            {
                                tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                            }
                            else
                            {
                                tex = anim.Offset - 1 + (int)((textureframe / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                                if (anim.Leap)
                                {
                                    ++tex;
                                    anim = model.AnimatedTextures[tex];
                                    tex = anim.Offset - 1 + anim.LOD0;
                                }
                            }
                        }
                        if (untex)
                        {
                            UnbindTexture();
                        }
                        else
                        {
                            BindTexture(0,tex);
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
                    }
                    else
                        UnbindTexture();
                    GL.Begin(PrimitiveType.Triangles);
                    if (normals_enabled)
                        GL.Color3(Color.White);
                    if ((tri.Subtype == 0 || tri.Subtype == 2) || ((tri.Subtype == 1) ^ tri.Type == 2))
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            SceneryColor c = model.Colors[tri.Color[j]];
                            if (!normals_enabled)
                                GL.Color3(c.Red,c.Green,c.Blue);
                            else
                            {
                                var normal = new Vector3d((sbyte)c.Red / 127.0,(sbyte)c.Green / 127.0,(sbyte)c.Blue / 127.0);
                                GL.Normal3(normal);
                            }
                            GL.TexCoord2(uvs[2 * j + 0], uvs[2 * j + 1]);
                            RenderVertex(f1,f2,tri.Vertex[j] + f1.SpecialVertexCount,f2 != null ? tri.Vertex[j] + f2.SpecialVertexCount : 0);
                        }
                    }
                    if ((tri.Subtype == 0 || tri.Subtype == 2) || ((tri.Subtype == 3) ^ tri.Type == 2))
                    {
                        for (int j = 2; j >= 0; --j)
                        {
                            SceneryColor c = model.Colors[tri.Color[j]];
                            if (!normals_enabled)
                                GL.Color3(c.Red,c.Green,c.Blue);
                            else
                            {
                                var normal = new Vector3d((sbyte)c.Red / 127.0,(sbyte)c.Green / 127.0,(sbyte)c.Blue / 127.0);
                                GL.Normal3(normal);
                            }
                            GL.TexCoord2(uvs[2 * j + 0], uvs[2 * j + 1]);
                            RenderVertex(f1,f2,tri.Vertex[j] + f1.SpecialVertexCount,f2 != null ? tri.Vertex[j] + f2.SpecialVertexCount : 0);
                        }
                    }
                    GL.End();
                    if (normals_enabled && Properties.Settings.Default.DisplayNormals)
                    {
                        GL.Disable(EnableCap.Lighting);
                        if (textures_enabled)
                            GL.Disable(EnableCap.Texture2D);
                        GL.Color3(Color.Cyan);
                        GL.Begin(PrimitiveType.Lines);
                        for (int j = 0; j < 3; ++j)
                        {
                            Vector3 v;
                            if (f2 == null)
                            {
                                FrameVertex vertex = f1.Vertices[tri.Vertex[j] + f1.SpecialVertexCount];
                                v = new Vector3(vertex.X + f1.XOffset / 4, vertex.Z + f1.YOffset / 4, vertex.Y + f1.ZOffset / 4);
                            }
                            else
                            {
                                FrameVertex v1 = f1.Vertices[tri.Vertex[j] + f1.SpecialVertexCount];
                                FrameVertex v2 = f2.Vertices[tri.Vertex[j] + f2.SpecialVertexCount];
                                int x1 = v1.X + f1.XOffset / 4;
                                int x2 = v2.X + f2.XOffset / 4;
                                int y1 = v1.Z + f1.YOffset / 4;
                                int y2 = v2.Z + f2.YOffset / 4;
                                int z1 = v1.Y + f1.ZOffset / 4;
                                int z2 = v2.Y + f2.ZOffset / 4;
                                v = new Vector3(x1 + (x2 - x1) / (float)interp * interi, y1 + (y2 - y1) / (float)interp * interi, z1 + (z2 - z1) / (float)interp * interi);
                            }
                            SceneryColor c = model.Colors[tri.Color[j]];
                            GL.Vertex3(v);
                            GL.Vertex3(v + new Vector3((sbyte)c.Red,(sbyte)c.Green,(sbyte)c.Blue) / 127 * 8);
                        }
                        GL.End();
                        if (textures_enabled)
                            GL.Enable(EnableCap.Texture2D);
                        GL.Enable(EnableCap.Lighting);
                    }
                }
                GL.Disable(EnableCap.CullFace);
                GL.PopMatrix();
                if (textures_enabled)
                    GL.Disable(EnableCap.Texture2D);
                else
                    GL.Enable(EnableCap.Texture2D);
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
            UnbindTexture();
            if (collision_enabled)
            {
                Frame fcol;
                if (f2 == null)
                    fcol = f1;
                else
                    fcol = f2;
                for (int i = 0; i < fcol.Collision.Count; ++i)
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
            float f = f1.IsNew ? 4.0f * 8 : 4.0f;
            if (f2 == null)
            {
                FrameVertex vertex = f1.Vertices[id1];
                GL.Vertex3(vertex.X + f1.XOffset / f, vertex.Z + f1.YOffset / f, vertex.Y + f1.ZOffset / f);
            }
            else
            {
                float fac = (float)interi / interp;
                FrameVertex v1 = f1.Vertices[id1];
                FrameVertex v2 = f2.Vertices[id2];
                float x1 = v1.X + f1.XOffset / f;
                float x2 = v2.X + f2.XOffset / f;
                float y1 = v1.Z + f1.YOffset / f;
                float y2 = v2.Z + f2.YOffset / f;
                float z1 = v1.Y + f1.ZOffset / f;
                float z2 = v2.Y + f2.ZOffset / f;
                GL.Vertex3(x1 + (x2 - x1) * fac, y1 + (y2 - y1) * fac, z1 + (z2 - z1) * fac);
            }
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
            if (frame.Temporals.Length < frame.Vertices.Count * 8 * 3) return frame; // failsafe
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
            if (normals_enabled)
            {
                GL.Disable(EnableCap.Lighting);
            }
            GL.DepthMask(false);
            GL.Color4(0f, 1f, 0f, 0.2f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            RenderCollisionBox(frame, col);
            GL.Color4(0f, 1f, 0f, 1f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            RenderCollisionBox(frame, col);
            GL.DepthMask(true);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            if (normals_enabled)
            {
                GL.Enable(EnableCap.Lighting);
            }
        }

        private void RenderCollisionBox(Frame frame, int col)
        {
            GL.PushMatrix();
            GL.Scale(new Vector3((frame.IsNew?0.5F:4F)/MinScale));
            GL.Translate(frame.Collision[col].XO, frame.Collision[col].YO, frame.Collision[col].ZO);
            GL.Begin(PrimitiveType.QuadStrip);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y1, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y2, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y1, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y2, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y1, frame.Collision[col].Z2);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y2, frame.Collision[col].Z2);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y1, frame.Collision[col].Z2);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y2, frame.Collision[col].Z2);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y1, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y2, frame.Collision[col].Z1);
            GL.End();
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y1, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y1, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y1, frame.Collision[col].Z2);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y1, frame.Collision[col].Z2);

            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y2, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y2, frame.Collision[col].Z1);
            GL.Vertex3(frame.Collision[col].X2, frame.Collision[col].Y2, frame.Collision[col].Z2);
            GL.Vertex3(frame.Collision[col].X1, frame.Collision[col].Y2, frame.Collision[col].Z2);
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
