using Crash;
using CrashEdit.Properties;
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
        private int interp = 4;
        private bool collision_enabled;
        private bool textures_enabled = true;
        private bool normals_enabled = false;
        private bool interp_startend = false;
        private int cullmode = 0;

        public AnimationEntryViewer(Frame frame,ModelEntry model,TextureChunk[] texturechunks)
        {
            collision_enabled = Settings.Default.DisplayFrameCollision;
            frames = new List<Frame>();
            this.model = model;
            if (model != null && model.Positions != null)
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
            collision_enabled = Settings.Default.DisplayFrameCollision;
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
                Interval = 1000 / OldMainForm.GetRate() / 2 / (interp / 2),
                Enabled = true
            };
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = 1000 / OldMainForm.GetRate() / 2 / (interp / 2);
                ++interi;
                if (interi >= interp)
                {
                    interi = 0;
                    frameid = (frameid + 1) % this.frames.Count;
                }
                Refresh();
            };
        }

        protected override int CameraRangeMargin => 400;
        protected override float NearPlane => 40;
        protected override float FarPlane => 400*200;

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                int mdlX = 0x1000;
                int mdlY = 0x1000;
                int mdlZ = 0x1000;
                if (model != null)
                {
                    mdlX = model.ScaleX;
                    mdlY = model.ScaleY;
                    mdlZ = model.ScaleZ;
                }
                yield return new Position(0,0,0);
                foreach (Frame frame in frames)
                {
                    foreach (FrameVertex vertex in frame.Vertices)
                    {
                        int s = frame.IsNew ? 32 : 4;
                        int x = vertex.X + frame.XOffset/s;
                        int y = vertex.Z + frame.YOffset/s;
                        int z = vertex.Y + frame.ZOffset/s;
                        x = x*mdlX>>10;
                        y = y*mdlY>>10;
                        z = z*mdlZ>>10;
                        yield return frame.IsNew ? new Position(x,y,z)*8 : new Position(x,y,z);
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
                case Keys.P:
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
                case Keys.P:
                    interp = interp == 2 ? 4 : 2;
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
            //RenderPoints(f1,f2);
            if (model != null)
            {
                if (Settings.Default.DisplayAnimGrid)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color3(Color.Red);
                    GL.Vertex3(-200, 0, 0);
                    GL.Vertex3(+200, 0, 0);
                    GL.Color3(Color.Green);
                    GL.Vertex3(0, -200, 0);
                    GL.Vertex3(0, +200, 0);
                    GL.Color3(Color.Blue);
                    GL.Vertex3(0, 0, -200);
                    GL.Vertex3(0, 0, +200);
                    GL.Color3(Color.Gray);
                    int gridamt = Settings.Default.AnimGridLen;
                    int gridlen = 400 * gridamt - 200;
                    for (int i = 0; i < gridamt; ++i)
                    {
                        GL.Vertex3(+200 + i * 400, 0, +gridlen);
                        GL.Vertex3(+200 + i * 400, 0, -gridlen);
                        GL.Vertex3(-200 - i * 400, 0, +gridlen);
                        GL.Vertex3(-200 - i * 400, 0, -gridlen);
                        GL.Vertex3(+gridlen, 0, +200 + i * 400);
                        GL.Vertex3(-gridlen, 0, +200 + i * 400);
                        GL.Vertex3(+gridlen, 0, -200 - i * 400);
                        GL.Vertex3(-gridlen, 0, -200 - i * 400);
                    }
                    GL.End();
                }
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
                double[] uvs = new double[6];
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
                            var t = model.Textures[tex];
                            switch (tri.Type)
                            {
                                case 0:
                                case 1:
                                    uvs[0] = t.X3;
                                    uvs[1] = t.Y3;
                                    uvs[4] = t.X1;
                                    uvs[5] = t.Y1;
                                    break;
                                case 2:
                                    uvs[0] = t.X1;
                                    uvs[1] = t.Y1;
                                    uvs[4] = t.X3;
                                    uvs[5] = t.Y3;
                                    break;
                            }
                            uvs[2] = t.X2;
                            uvs[3] = t.Y2;
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
                            RenderVertex(f1,f2,tri.Vertex[j] + f1.SpecialVertexCount);
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
                            RenderVertex(f1,f2,tri.Vertex[j] + f1.SpecialVertexCount);
                        }
                    }
                    GL.End();
                    if (normals_enabled && Settings.Default.DisplayNormals)
                    {
                        GL.Disable(EnableCap.Lighting);
                        if (textures_enabled)
                            GL.Disable(EnableCap.Texture2D);
                        GL.Color3(Color.Cyan);
                        GL.Begin(PrimitiveType.Lines);
                        for (int j = 0; j < 3; ++j)
                        {
                            Vector3 v = GetVertex(f1, f2, tri.Vertex[j] + f1.SpecialVertexCount);
                            SceneryColor c = model.Colors[tri.Color[j]];
                            GL.Vertex3(v);
                            GL.Vertex3(v + new Vector3((sbyte)c.Red,(sbyte)c.Green,(sbyte)c.Blue) / 127 * 16);
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
                    RenderVertex(f1,f2,i);
                }
                GL.End();
            }
            UnbindTexture();
            if (collision_enabled)
            {
                Frame fcol = f1;
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
                GL.Scale(12.8f,12.8f,12.8f);
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

        private void RenderVertex(Frame f1, Frame f2, int id)
        {
            GL.Vertex3(GetVertex(f1, f2, id));
        }

        private Vector3 GetVertex(Frame f1, Frame f2, int id)
        {
            float s = f1.IsNew ? 32 : 4;
            Vector3 v;
            if (f2 == null)
            {
                FrameVertex vertex = f1.Vertices[id];
                v = new Vector3(vertex.X + f1.XOffset/s, vertex.Z + f1.YOffset/s, vertex.Y + f1.ZOffset/s);
            }
            else
            {
                float fac = (float)interi / interp;
                FrameVertex v1 = f1.Vertices[id];
                FrameVertex v2 = f2.Vertices[id];
                float x1 = v1.X + f1.XOffset/s;
                float x2 = v2.X + f2.XOffset/s;
                float y1 = v1.Z + f1.YOffset/s;
                float y2 = v2.Z + f2.YOffset/s;
                float z1 = v1.Y + f1.ZOffset/s;
                float z2 = v2.Y + f2.ZOffset/s;
                v =  new Vector3(NumberExt.GetFac(x1,x2,fac),NumberExt.GetFac(y1,y2,fac),NumberExt.GetFac(z1,z2,fac));
            }
            if (model != null)
            {
                v *= new Vector3(model.ScaleX/256F/4,model.ScaleY/256F/4,model.ScaleZ/256F/4);
            }
            return f1.IsNew ? v*8 : v;
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
            GL.Scale(new Vector3(1/256F));
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
