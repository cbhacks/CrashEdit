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
    public sealed class OldAnimationEntryViewer : ThreeDimensionalViewer
    {
        private List<OldFrame> frames;
        private OldModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private int interi;
        private int interp = 2;
        private bool colored;
        private float r, g, b;
        private bool collisionenabled;
        private bool texturesenabled = true;
        private bool normalsenabled = true;
        private bool interp_startend = false;
        private int cullmode = 0;

        private Dictionary<int,TextureChunk> texturechunks;
        private bool init;

        public OldAnimationEntryViewer(OldFrame frame,bool colored,OldModelEntry model,Dictionary<int,TextureChunk> texturechunks)
        {
            collisionenabled = Settings.Default.DisplayFrameCollision;
            frames = new List<OldFrame>() { frame };
            this.model = model;
            this.texturechunks = texturechunks;
            this.colored = colored;
            init = false;
            InitTextures(1);
            frameid = 0;
        }

        public OldAnimationEntryViewer(IEnumerable<OldFrame> frames,bool colored,OldModelEntry model,Dictionary<int,TextureChunk> texturechunks)
        {
            collisionenabled = Settings.Default.DisplayFrameCollision;
            this.frames = new List<OldFrame>(frames);
            this.model = model;
            this.texturechunks = texturechunks;
            this.colored = colored;
            init = false;
            InitTextures(1);
            frameid = 0;
            animatetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate() / interp,
                Enabled = true
            };
            animatetimer.Tick += delegate (object sender,EventArgs e)
            {
                animatetimer.Interval = 1000 / OldMainForm.GetRate() / interp;
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
                foreach (OldFrame frame in frames)
                {
                    foreach (OldFrameVertex vertex in frame.Vertices)
                    {
                        int x = vertex.X-128 + frame.XOffset;
                        int y = vertex.Y-128 + frame.YOffset;
                        int z = vertex.Z-128 + frame.ZOffset;
                        x = x*mdlX>>10;
                        y = y*mdlY>>10;
                        z = z*mdlZ>>10;
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
                ConvertTexturesToGL(0,texturechunks,model.Structs);
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
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.C:
                case Keys.N:
                case Keys.T:
                case Keys.U:
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
                    collisionenabled = !collisionenabled;
                    break;
                case Keys.N:
                    normalsenabled = !normalsenabled;
                    break;
                case Keys.T:
                    texturesenabled = !texturesenabled;
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

        private void RenderFrame(OldFrame frame, OldFrame f2 = null)
        {
            if (model != null)
            {
                if (Settings.Default.DisplayAnimGrid)
                {
                    GL.PushMatrix();
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
                    GL.PopMatrix();
                }
                if (cullmode < 2)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(cullFaceModes[cullmode]);
                }
                if (texturesenabled)
                    GL.Enable(EnableCap.Texture2D);
                else
                    GL.Disable(EnableCap.Texture2D);
                if (normalsenabled && !colored)
                    GL.Enable(EnableCap.Lighting);
                else
                    GL.Disable(EnableCap.Lighting);
                GL.PushMatrix();
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        BindTexture(0,polygon.Unknown & 0x7FFF);
                        GL.Color3(tex.R,tex.G,tex.B);
                        if (colored)
                        {
                            r = tex.R / 128F;
                            g = tex.G / 128F;
                            b = tex.B / 128F;
                        }
                        if (tex.N && cullmode < 2)
                        {
                            GL.Disable(EnableCap.CullFace);
                        }
                        GL.Begin(PrimitiveType.Triangles);
                        GL.TexCoord2(tex.X1,tex.Y1);
                        RenderVertex(frame,f2,polygon.VertexA / 6);
                        GL.TexCoord2(tex.X3,tex.Y3);
                        RenderVertex(frame,f2,polygon.VertexC / 6);
                        GL.TexCoord2(tex.X2,tex.Y2);
                        RenderVertex(frame,f2,polygon.VertexB / 6);
                        GL.End();
                        if (tex.N && cullmode < 2)
                        {
                            GL.Enable(EnableCap.CullFace);
                        }
                    }
                    else
                    {
                        UnbindTexture();
                        OldSceneryColor col = (OldSceneryColor)str;
                        GL.Color3(col.R,col.G,col.B);
                        if (colored)
                        {
                            r = col.R / 128F;
                            g = col.G / 128F;
                            b = col.B / 128F;
                        }
                        if (col.N && cullmode < 2)
                        {
                            GL.Disable(EnableCap.CullFace);
                        }
                        GL.Begin(PrimitiveType.Triangles);
                        RenderVertex(frame,f2,polygon.VertexA / 6);
                        RenderVertex(frame,f2,polygon.VertexC / 6);
                        RenderVertex(frame,f2,polygon.VertexB / 6);
                        GL.End();
                        if (col.N && cullmode < 2)
                        {
                            GL.Enable(EnableCap.CullFace);
                        }
                    }
                }
                GL.Disable(EnableCap.CullFace);
                GL.PopMatrix();
                GL.Disable(EnableCap.Texture2D);
                GL.Disable(EnableCap.Lighting);
            }
            else
            {
                GL.Color3(Color.White);
                GL.Begin(PrimitiveType.Points);
                for (int i = 0, m = frame.Vertices.Count; i < m; ++i)
                {
                    RenderVertex(frame, f2, i);
                }
                GL.End();
            }
            if (!colored && normalsenabled && Settings.Default.DisplayNormals)
            {
                GL.PushMatrix();
                GL.Scale(new Vector3d(model.ScaleX, model.ScaleY, model.ScaleZ)/256/4);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Cyan);
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    Vector3 v = new Vector3(vertex.X-128 + frame.XOffset,vertex.Y-128 + frame.YOffset,vertex.Z-128 + frame.ZOffset);
                    GL.Vertex3(v);
                    GL.Vertex3(v + new Vector3(vertex.NormalX,vertex.NormalY,vertex.NormalZ) / 127 * 4);
                }
                GL.End();
                GL.PopMatrix();
            }
            if (collisionenabled)
            {
                RenderCollision(frame);
            }
        }

        private void RenderVertex(OldFrame f1, OldFrame f2, int id)
        {
            if (f2 == null)
            {
                OldFrameVertex v = f1.Vertices[id];
                if (colored)
                {
                    GL.Color3((byte)((byte)v.NormalX * r),(byte)((byte)v.NormalY * g),(byte)((byte)v.NormalZ * b));
                }
                else if (normalsenabled)
                {
                    GL.Normal3(v.NormalX/127.0,v.NormalY/127.0,v.NormalZ/127.0);
                }
                float x = v.X-128 + f1.XOffset;
                float y = v.Y-128 + f1.YOffset;
                float z = v.Z-128 + f1.ZOffset;
                if (model != null)
                {
                    GL.Vertex3(x*model.ScaleX/256/4,y*model.ScaleY/256/4,z*model.ScaleZ/256/4);
                }
                else
                {
                    GL.Vertex3(x,y,z);
                }
            }
            else
            {
                float f = (float)interi / interp;
                OldFrameVertex v1 = f1.Vertices[id];
                OldFrameVertex v2 = f2.Vertices[id];
                if (colored)
                {
                    int r1 = (byte)v1.NormalX;
                    int r2 = (byte)v2.NormalX;
                    int g1 = (byte)v1.NormalY;
                    int g2 = (byte)v2.NormalY;
                    int b1 = (byte)v1.NormalZ;
                    int b2 = (byte)v2.NormalZ;
                    byte nr = (byte)(NumberExt.GetFac(r1,r2,f) * r);
                    byte ng = (byte)(NumberExt.GetFac(g1,g2,f) * g);
                    byte nb = (byte)(NumberExt.GetFac(b1,b2,f) * b);
                    GL.Color3(nr,ng,nb);
                }
                else if (normalsenabled)
                {
                    int nx1 = v1.NormalX;
                    int nx2 = v2.NormalX;
                    int ny1 = v1.NormalY;
                    int ny2 = v2.NormalY;
                    int nz1 = v1.NormalZ;
                    int nz2 = v2.NormalZ;
                    GL.Normal3(NumberExt.GetFac(nx1,nx2,f)/127.0,NumberExt.GetFac(ny1,ny2,f)/127.0,NumberExt.GetFac(nz1,nz2,f)/127.0);
                }
                int x1 = v1.X-128 + f1.XOffset;
                int x2 = v2.X-128 + f2.XOffset;
                int y1 = v1.Y-128 + f1.YOffset;
                int y2 = v2.Y-128 + f2.YOffset;
                int z1 = v1.Z-128 + f1.ZOffset;
                int z2 = v2.Z-128 + f2.ZOffset;
                if (model != null)
                {
                    float x = NumberExt.GetFac(x1,x2,f) * model.ScaleX/256/4;
                    float y = NumberExt.GetFac(y1,y2,f) * model.ScaleY/256/4;
                    float z = NumberExt.GetFac(z1,z2,f) * model.ScaleZ/256/4;
                    GL.Vertex3(x,y,z);
                }
                else
                {
                    GL.Vertex3(NumberExt.GetFac(x1,x2,f),NumberExt.GetFac(y1,y2,f),NumberExt.GetFac(z1,z2,f));
                }
            }
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
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private void RenderCollisionBox(OldFrame frame)
        {
            int xcol1 = frame.X1;
            int xcol2 = frame.X2;
            int ycol1 = frame.Y1;
            int ycol2 = frame.Y2;
            int zcol1 = frame.Z1;
            int zcol2 = frame.Z2;
            GL.PushMatrix();
            GL.Scale(new Vector3(1/256F));
            GL.Translate(frame.XGlobal,frame.YGlobal,frame.ZGlobal);
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
