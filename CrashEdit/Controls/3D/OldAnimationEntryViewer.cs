using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryViewer : GLViewer, IGLDisposable
    {
        private NSF nsf;

        private int frame_id;
        private int cur_frame;
        private int eid_anim;
        private int interi;
        private int interp = 2;
        private bool colored;
        private bool collisionenabled;
        private bool texturesenabled = true;
        private bool normalsenabled = true;
        private bool interp_startend = false;
        private int cullmode = 0;

        private int tpage;
        private VAO vaoModel;
        private Vector4[] buf_vtx;
        private Vector3[] buf_nor;
        private Color4[] buf_col;
        private Vector3[] buf_tex;
        private int buf_idx;

        protected override bool UseGrid => true;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame, bool colored)
        {
            this.nsf = nsf;
            collisionenabled = Settings.Default.DisplayFrameCollision;
            eid_anim = anim_eid;
            frame_id = frame;
            this.colored = colored;
            cur_frame = 0;
        }

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, bool colored)
        {
            this.nsf = nsf;
            collisionenabled = Settings.Default.DisplayFrameCollision;
            eid_anim = anim_eid;
            frame_id = -1;
            this.colored = colored;
            cur_frame = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CheckGLError("oldanimv postload");

            vaoModel = new VAO(render.ShaderContext, "anim_c1", PrimitiveType.Triangles);

            tpage = GL.GenTexture();
            CheckGLError("oldanimv tex gen");
            GL.BindTexture(TextureTarget.Texture2D, tpage);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R16ui, 1024, 128 * 8, 0, PixelFormat.RedInteger, PixelType.UnsignedInt, IntPtr.Zero);
            CheckGLError("oldanimv tex img 2d");
            GL.BindImageTexture(4, tpage, 0, false, 0, TextureAccess.ReadOnly, SizedInternalFormat.R16ui);
            CheckGLError("oldanimv tex img bind 1");
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                /*
                var anim = nsf.GetEntry<OldAnimationEntry>(eid_anim);
                if (anim != null)
                {
                    var frames = new List<OldFrame>();
                    if (frame_id != -1)
                        frames.Add(anim.Frames[frame_id]);
                    else
                        frames.AddRange(anim.Frames);

                    foreach (OldFrame frame in frames)
                    {
                        var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
                        if (model != null)
                        {
                            var mx = (float)model.ScaleX / 3200 / 128;
                            var my = (float)model.ScaleY / 3200 / 128;
                            var mz = (float)model.ScaleZ / 3200 / 128;
                            yield return new Position((frame.XOffset - 127) * mx, (frame.YOffset - 127) * my, (frame.ZOffset - 127) * mz);
                            yield return new Position((frame.XOffset + 127) * mx, (frame.YOffset + 127) * my, (frame.ZOffset + 127) * mz);
                        }
                    }
                }
                */
                yield return new Position(0, 0, 0);
            }
        }

        protected override void Render()
        {
            base.Render();

            buf_idx = 0;

            var anim = nsf.GetEntry<OldAnimationEntry>(eid_anim);
            if (anim != null)
            {
                cur_frame = frame_id == -1 ? (int)(render.CurrentFrame / 2 % anim.Frames.Count) : frame_id;
                RenderFrame(nsf.GetEntry<OldAnimationEntry>(eid_anim).Frames[cur_frame]);
            }
        }

        protected override void ActualRunLogic()
        {
            base.ActualRunLogic();
            if (KPress(Keys.C)) collisionenabled = !collisionenabled;
            if (KPress(Keys.N)) normalsenabled = !normalsenabled;
            if (KPress(Keys.T)) texturesenabled = !texturesenabled;
            if (KPress(Keys.U)) cullmode = ++cullmode % 3;
        }

        private void RenderFrame(OldFrame frame, OldFrame frame2 = null)
        {
            if (frame2 != null && (frame2.ModelEID != frame.ModelEID || frame.Vertices.Count != frame2.Vertices.Count)) frame2 = null;

            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                int[] tex_eids = new int[8];
                foreach (OldModelStruct str in model.Structs)
                {
                    if (str is OldModelTexture tex)
                    {
                        for (int i = 0; i < tex_eids.Length; ++i)
                        {
                            if (tex_eids[i] == 0)
                            {
                                var tpag = nsf.GetEntry<TextureChunk>(tex.EID);
                                if (tpag != null)
                                {
                                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, i * 128, 1024, 128, PixelFormat.RedInteger, PixelType.UnsignedInt, tpag.Data);
                                }
                                tex_eids[i] = tex.EID;
                                break;
                            }
                            else if (tex_eids[i] == tex.EID)
                            {
                                break;
                            }
                        }
                    }
                }

                int nb = model.Polygons.Count * 3;
                //render.Projection.UserMat3.Row0 = new Vector3(-4096, 2048, 4096) / 0x1000;
                //render.Projection.UserMat3.Row1 = new Vector3(-3563, 2048, 4096) / 0x1000;
                //render.Projection.UserMat3.Row2 = new Vector3(4096, -2048, 0) / 0x1000;
                //render.Projection.UserColorAmb = new Vector3(614, 614, 614) / 0x200;
                buf_vtx = new Vector4[nb];
                buf_nor = new Vector3[nb];
                buf_col = new Color4[nb];
                buf_tex = new Vector3[nb];
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        buf_col[buf_idx + 0] = new(tex.R, tex.G, tex.B, 255);
                        buf_col[buf_idx + 1] = buf_col[buf_idx];
                        buf_col[buf_idx + 2] = buf_col[buf_idx];
                        buf_tex[buf_idx + 0] = new(tex.U1, tex.V1, tex.ColorMode);
                        buf_tex[buf_idx + 1] = new(tex.U2, tex.V2, tex.ColorMode);
                        buf_tex[buf_idx + 2] = new(tex.U3, tex.V3, tex.ColorMode);
                        RenderVertex(frame, frame2, polygon.VertexA / 6);
                        RenderVertex(frame, frame2, polygon.VertexB / 6);
                        RenderVertex(frame, frame2, polygon.VertexC / 6);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        buf_col[buf_idx + 0] = new(col.R, col.G, col.B, 255);
                        buf_col[buf_idx + 1] = buf_col[buf_idx];
                        buf_col[buf_idx + 2] = buf_col[buf_idx];
                        buf_tex[buf_idx + 0] = new(-1);
                        buf_tex[buf_idx + 1] = new(-1);
                        buf_tex[buf_idx + 2] = new(-1);
                        RenderVertex(frame, frame2, polygon.VertexA / 6);
                        RenderVertex(frame, frame2, polygon.VertexB / 6);
                        RenderVertex(frame, frame2, polygon.VertexC / 6);
                    }
                }
                render.Projection.UserTrans = new(frame.XOffset, frame.YOffset, frame.ZOffset);
                render.Projection.UserScale = new(model.ScaleX, model.ScaleY, model.ScaleZ);
                vaoModel.UpdatePositions(buf_vtx);
                //vaoModel.UpdateNormals(buf_nor);
                vaoModel.UpdateColors(buf_col);
                vaoModel.UpdateAttrib("uvc", buf_tex, 12, 3);
                vaoModel.Render(render);
            }
        }

        private void RenderVertex(OldFrame f1, OldFrame f2, int id)
        {
            if (f2 == null)
            {
                OldFrameVertex v = f1.Vertices[id];
                float x = v.X-128;
                float y = v.Y-128;
                float z = v.Z-128;
                buf_vtx[buf_idx] = new(x, y, z, 1);
                float nx = v.NormalX;
                float ny = v.NormalY;
                float nz = v.NormalZ;
                buf_nor[buf_idx] = new(nx, ny, nz);
                buf_idx++;
            }
            else
            {
            }
        }
        /*
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
        }*/

        public new void GLDispose()
        {
            GL.DeleteTexture(tpage);
            base.GLDispose();
        }
    }
}
