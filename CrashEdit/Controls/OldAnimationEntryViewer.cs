using Crash;
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
        private List<ProtoFrame> protoframes;
        private OldModelEntry model;
        private int frameid;
        private Timer animatetimer;
        private bool isproto;
        private bool colored;
        private float r, g, b;
        private bool collisionenabled;
        private bool texturesenabled = true;
        private bool normalsenabled = true;
        private int cullmode = 0;

        private Dictionary<int,TextureChunk> texturechunks;
        private bool init;

        public OldAnimationEntryViewer(ProtoFrame frame,OldModelEntry model,Dictionary<int,TextureChunk> texturechunks)
        {
            collisionenabled = Properties.Settings.Default.DisplayFrameCollision;
            protoframes = new List<ProtoFrame> { frame };
            frames = new List<OldFrame>();
            this.model = model;
            this.texturechunks = texturechunks;
            init = false;
            colored = false;
            InitTextures(1);
            frameid = 0;
            isproto = true;
        }

        public OldAnimationEntryViewer(IEnumerable<ProtoFrame> frames,OldModelEntry model,Dictionary<int,TextureChunk> texturechunks)
        {
            collisionenabled = Properties.Settings.Default.DisplayFrameCollision;
            protoframes = new List<ProtoFrame>(frames);
            this.frames = new List<OldFrame>();
            this.model = model;
            this.texturechunks = texturechunks;
            init = false;
            colored = false;
            InitTextures(1);
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

        public OldAnimationEntryViewer(OldFrame frame,bool colored,OldModelEntry model,Dictionary<int,TextureChunk> texturechunks)
        {
            collisionenabled = Properties.Settings.Default.DisplayFrameCollision;
            protoframes = new List<ProtoFrame>();
            frames = new List<OldFrame>() { frame };
            this.model = model;
            this.texturechunks = texturechunks;
            this.colored = colored;
            init = false;
            InitTextures(1);
            frameid = 0;
            isproto = false;
        }

        public OldAnimationEntryViewer(IEnumerable<OldFrame> frames,bool colored,OldModelEntry model,Dictionary<int,TextureChunk> texturechunks)
        {
            collisionenabled = Properties.Settings.Default.DisplayFrameCollision;
            protoframes = new List<ProtoFrame>();
            this.frames = new List<OldFrame>(frames);
            this.model = model;
            this.texturechunks = texturechunks;
            this.colored = colored;
            init = false;
            InitTextures(1);
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

        private int MinScale => model != null ? Math.Min(BitConv.FromInt32(model.Info, 12), Math.Min(BitConv.FromInt32(model.Info, 4), BitConv.FromInt32(model.Info, 8))) : 0x1000;
        private int MaxScale => model != null ? Math.Max(BitConv.FromInt32(model.Info, 12), Math.Max(BitConv.FromInt32(model.Info, 4), BitConv.FromInt32(model.Info, 8))) : 0x1000;

        protected override int CameraRangeMargin => 128;
        protected override float ScaleFactor => 8;

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var vec = new Vector3(BitConv.FromInt32(model.Info,4),BitConv.FromInt32(model.Info,8),BitConv.FromInt32(model.Info,12))/MinScale;
                foreach (OldFrame frame in frames)
                {
                    foreach (OldFrameVertex vertex in frame.Vertices)
                    {
                        float x = vertex.X + frame.XOffset;
                        float y = vertex.Y + frame.YOffset;
                        float z = vertex.Z + frame.ZOffset;
                        x *= vec.X;
                        y *= vec.Y;
                        z *= vec.Z;
                        yield return new Position(x,y,z);
                    }
                }
                foreach (ProtoFrame frame in protoframes)
                {
                    foreach (OldFrameVertex vertex in frame.Vertices)
                    {
                        float x = vertex.X + frame.XOffset;
                        float y = vertex.Y + frame.YOffset;
                        float z = vertex.Z + frame.ZOffset;
                        x *= vec.X;
                        y *= vec.Y;
                        z *= vec.Z;
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
            if (isproto)
                RenderFrame(protoframes[frameid % protoframes.Count]);
            else
                RenderFrame(frames[frameid % frames.Count]);
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

        private void RenderFrame(ProtoFrame frame)
        {
            if (model != null)
            {
                if (cullmode < 2)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(cullFaceModes[cullmode]);
                }
                if (texturesenabled)
                    GL.Enable(EnableCap.Texture2D);
                else
                    GL.Disable(EnableCap.Texture2D);
                if (normalsenabled)
                    GL.Enable(EnableCap.Lighting);
                else
                    GL.Disable(EnableCap.Lighting);
                GL.PushMatrix();
                GL.Scale(new Vector3(BitConv.FromInt32(model.Info,4),BitConv.FromInt32(model.Info,8),BitConv.FromInt32(model.Info,12))/MinScale);
                for (int i = 0; i < model.Polygons.Count; ++i)
                {
                    OldModelPolygon polygon = model.Polygons[i];
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        BindTexture(0,polygon.Unknown & 0x7FFF);
                        GL.Color3(tex.R,tex.G,tex.B);
                        GL.Begin(PrimitiveType.Triangles);
                        GL.TexCoord2(tex.X1,tex.Y1);
                        RenderVertex(frame,frame.Vertices[polygon.VertexA / 6]);
                        GL.TexCoord2(tex.X3,tex.Y3);
                        RenderVertex(frame,frame.Vertices[polygon.VertexC / 6]);
                        GL.TexCoord2(tex.X2,tex.Y2);
                        RenderVertex(frame,frame.Vertices[polygon.VertexB / 6]);
                        GL.End();
                    }
                    else
                    {
                        UnbindTexture();
                        OldSceneryColor col = (OldSceneryColor)str;
                        GL.Color3(col.R,col.G,col.B);
                        GL.Begin(PrimitiveType.Triangles);
                        RenderVertex(frame,frame.Vertices[polygon.VertexA / 6]);
                        RenderVertex(frame,frame.Vertices[polygon.VertexC / 6]);
                        RenderVertex(frame,frame.Vertices[polygon.VertexB / 6]);
                        GL.End();
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
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    RenderVertex(frame, vertex);
                }
                GL.End();
            }
            if (normalsenabled && Properties.Settings.Default.DisplayNormals)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Cyan);
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    GL.Vertex3(vertex.X + frame.XOffset,vertex.Y + frame.YOffset,vertex.Z + frame.ZOffset);
                    GL.Vertex3(vertex.X + (sbyte)vertex.NormalX / 127F * 4 + frame.XOffset,
                        vertex.Y + (sbyte)vertex.NormalY / 127F * 4 + frame.YOffset,
                        vertex.Z + (sbyte)vertex.NormalZ / 127F * 4 + frame.ZOffset);
                }
                GL.End();
            }
        }

        private void RenderFrame(OldFrame frame)
        {
            if (model != null)
            {
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
                GL.Scale(new Vector3(BitConv.FromInt32(model.Info,4),BitConv.FromInt32(model.Info,8),BitConv.FromInt32(model.Info,12))/MinScale);
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
                        GL.Begin(PrimitiveType.Triangles);
                        GL.TexCoord2(tex.X1,tex.Y1);
                        RenderVertex(frame,frame.Vertices[polygon.VertexA / 6]);
                        GL.TexCoord2(tex.X3,tex.Y3);
                        RenderVertex(frame,frame.Vertices[polygon.VertexC / 6]);
                        GL.TexCoord2(tex.X2,tex.Y2);
                        RenderVertex(frame,frame.Vertices[polygon.VertexB / 6]);
                        GL.End();
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
                        GL.Begin(PrimitiveType.Triangles);
                        RenderVertex(frame,frame.Vertices[polygon.VertexA / 6]);
                        RenderVertex(frame,frame.Vertices[polygon.VertexC / 6]);
                        RenderVertex(frame,frame.Vertices[polygon.VertexB / 6]);
                        GL.End();
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
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    RenderVertex(frame, vertex);
                }
                GL.End();
            }
            if (!colored && normalsenabled && Properties.Settings.Default.DisplayNormals)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Cyan);
                foreach (OldFrameVertex vertex in frame.Vertices)
                {
                    GL.Vertex3(vertex.X + frame.XOffset,vertex.Y + frame.YOffset,vertex.Z + frame.ZOffset);
                    GL.Vertex3(vertex.X + (sbyte)vertex.NormalX / 127F * 4 + frame.XOffset,
                        vertex.Y + (sbyte)vertex.NormalY / 127F * 4 + frame.YOffset,
                        vertex.Z + (sbyte)vertex.NormalZ / 127F * 4 + frame.ZOffset);
                }
                GL.End();
            }
            if (collisionenabled)
            {
                RenderCollision(frame);
            }
        }

        private void RenderVertex(ProtoFrame frame, OldFrameVertex vertex)
        {
            if (normalsenabled)
            {
                GL.Normal3((sbyte)vertex.NormalX / 127F,(sbyte)vertex.NormalY / 127F,(sbyte)vertex.NormalZ / 127F);
            }
            GL.Vertex3(vertex.X + frame.XOffset,vertex.Y + frame.YOffset,vertex.Z + frame.ZOffset);
        }

        private void RenderVertex(OldFrame frame, OldFrameVertex vertex)
        {
            if (colored)
            {
                byte nr = (byte)(vertex.NormalX * r);
                byte ng = (byte)(vertex.NormalY * g);
                byte nb = (byte)(vertex.NormalZ * b);
                GL.Color3(nr,ng,nb);
            }
            else if (normalsenabled)
            {
                GL.Normal3((sbyte)vertex.NormalX/127F,(sbyte)vertex.NormalY/127F,(sbyte)vertex.NormalZ/127F);
            }
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
            GL.Translate(128,128,128);
            //GL.Scale(0.00125f,0.00125f,0.00125f);
            GL.Scale(new Vector3(4F/MinScale));
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
