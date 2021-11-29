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
        private readonly NSF nsf;

        private readonly int eid_anim;
        private readonly int frame_id;
        private int cur_frame;
        private bool colored;
        private bool collisionenabled;
        private bool texturesenabled = true;
        private bool normalsenabled = true;
        private bool interpenabled = true;
        private float interp;
        private int cullmode = 0;

        private int tpage;
        private VAO vaoModel;
        private Vector3[] buf_vtx;
        private Vector3[] buf_nor;
        private Color4[] buf_col;
        private Vector2[] buf_uv;
        private int[] buf_tex;
        private Dictionary<int, int> tex_eids;
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

            vaoModel = new VAO(render.ShaderContext, "anim_c1", PrimitiveType.Triangles);

            tpage = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tpage);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8ui, 512, 0, 0, PixelFormat.RedInteger, PixelType.UnsignedByte, IntPtr.Zero);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
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
                        float mx = 1;
                        float my = 1;
                        float mz = 1;
                        if (model != null)
                        {
                            mx = (float)model.ScaleX / 3200 / 128;
                            my = (float)model.ScaleY / 3200 / 128;
                            mz = (float)model.ScaleZ / 3200 / 128;
                        }
                        foreach (var vert in frame.Vertices)
                        {
                            yield return (new Position(vert.X, vert.Y, vert.Z)
                                        - new Position(128, 128, 128)
                                        + new Position(frame.XOffset, frame.YOffset, frame.ZOffset)) * new Position(mx, my, mz);
                        }
                    }
                }
                
                yield return new Position(0, 0, 0);
            }
        }

        protected override void Render()
        {
            base.Render();

            var anim = nsf.GetEntry<OldAnimationEntry>(eid_anim);
            if (anim != null)
            {
                OldFrame f2 = null;
                if (anim.Frames.Count == 1)
                {
                    cur_frame = 0;
                }
                else if (frame_id != -1)
                {
                    cur_frame = frame_id;
                }
                else
                {
                    double prog = render.FullCurrentFrame / 2 % anim.Frames.Count;
                    cur_frame = (int)Math.Floor(prog);
                    if (interpenabled)
                    {
                        f2 = anim.Frames[(int)Math.Ceiling(prog) % anim.Frames.Count];
                        interp = (float)(prog - cur_frame);
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, cur_frame, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                RenderFrame(anim.Frames[cur_frame], f2);
            }
        }

        protected override void ActualRunLogic()
        {
            base.ActualRunLogic();
            if (KPress(Keys.C)) collisionenabled = !collisionenabled;
            if (KPress(Keys.N)) normalsenabled = !normalsenabled;
            if (KPress(Keys.T)) texturesenabled = !texturesenabled;
            if (KPress(Keys.I)) interpenabled = !interpenabled;
            if (KPress(Keys.U)) cullmode = ++cullmode % 3;
        }

        private void UploadTPAGs(OldModelEntry model)
        {
            // collect tpag eids
            tex_eids = new();
            foreach (OldModelStruct str in model.Structs)
            {
                if (str is OldModelTexture tex && !tex_eids.ContainsKey(tex.EID))
                {
                    tex_eids[tex.EID] = tex_eids.Count;
                }
            }
            // fill texture
            GL.GetTextureLevelParameter(tpage, 0, GetTextureParameter.TextureHeight, out int tpage_h);
            if (tpage_h < tex_eids.Count * 128)
            {
                // realloc if not enough texture mem
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8ui, 512, tex_eids.Count * 128, 0, PixelFormat.RedInteger, PixelType.UnsignedByte, IntPtr.Zero);
            }
            foreach (var kvp in tex_eids)
            {
                var tpag = nsf.GetEntry<TextureChunk>(kvp.Key);
                if (tpag != null)
                {
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, kvp.Value * 128, 512, 128, PixelFormat.RedInteger, PixelType.UnsignedByte, tpag.Data);
                }
            }
        }

        private void RenderFrame(OldFrame frame, OldFrame frame2 = null)
        {
            if (frame2 != null && (frame2.ModelEID != frame.ModelEID || frame.Vertices.Count != frame2.Vertices.Count)) frame2 = null;

            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                // setup textures
                GL.BindTexture(TextureTarget.Texture2D, tpage);
                UploadTPAGs(model);

                // alloc buffers
                int nb = model.Polygons.Count * 3;
                buf_vtx = new Vector3[nb];
                buf_nor = new Vector3[nb];
                buf_col = new Color4[nb];
                buf_uv = new Vector2[nb];
                buf_tex = new int[nb]; // enable: 1, colormode: 2, blendmode: 2, clutx: 4, cluty: 7, doubleface: 1, page: X (>17 total)

                // render stuff
                RenderFramePass(model, frame, frame2, RenderPass.Solid);
                RenderFramePass(model, frame, frame2, RenderPass.Subtractive);
                RenderFramePass(model, frame, frame2, RenderPass.Additive);
            }
        }

        private void RenderFramePass(OldModelEntry model, OldFrame frame, OldFrame frame2, RenderPass pass)
        {
            buf_idx = 0;

            foreach (OldModelPolygon polygon in model.Polygons)
            {
                OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                if (str is OldModelTexture tex)
                {
                    if (pass != RenderPass.Solid && (tex.BlendMode == 0 || tex.BlendMode == 3)) continue;
                    if (pass != RenderPass.Additive && tex.BlendMode == 1) continue;
                    if (pass != RenderPass.Subtractive && tex.BlendMode == 2) continue;
                    buf_col[buf_idx + 0] = new(tex.R, tex.G, tex.B, 255);
                    buf_col[buf_idx + 1] = buf_col[buf_idx];
                    buf_col[buf_idx + 2] = buf_col[buf_idx];
                    buf_uv[buf_idx + 0] = new(tex.U3, tex.V3);
                    buf_uv[buf_idx + 1] = new(tex.U2, tex.V2);
                    buf_uv[buf_idx + 2] = new(tex.U1, tex.V1);
                    buf_tex[buf_idx + 2] = 1
                                        | (tex.ColorMode << 1)
                                        | (tex.BlendMode << 3)
                                        | (tex.ClutX << 5)
                                        | (tex.ClutY << 9)
                                        | (Convert.ToInt32(tex.N) << 16)
                                        | (tex_eids[tex.EID] << 17)
                                        ;
                    RenderVertex(frame, frame2, polygon.VertexC / 6);
                    RenderVertex(frame, frame2, polygon.VertexB / 6);
                    RenderVertex(frame, frame2, polygon.VertexA / 6);
                }
                else
                {
                    if (pass != RenderPass.Solid) continue;
                    OldSceneryColor col = (OldSceneryColor)str;
                    buf_col[buf_idx + 0] = new(col.R, col.G, col.B, 255);
                    buf_col[buf_idx + 1] = buf_col[buf_idx];
                    buf_col[buf_idx + 2] = buf_col[buf_idx];
                    buf_tex[buf_idx + 2] = 0 | (Convert.ToInt32(col.N) << 16);
                    RenderVertex(frame, frame2, polygon.VertexC / 6);
                    RenderVertex(frame, frame2, polygon.VertexB / 6);
                    RenderVertex(frame, frame2, polygon.VertexA / 6);
                }
            }

            if (buf_idx > 0)
            {
                // uniforms and static data
                render.Projection.UserTrans = new(frame.XOffset, frame.YOffset, frame.ZOffset);
                if (frame2 != null)
                {
                    render.Projection.UserTrans = MathExt.Lerp(render.Projection.UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset), interp);
                }
                render.Projection.UserScale = new(model.ScaleX, model.ScaleY, model.ScaleZ);
                render.Projection.UserInt1 = cullmode;

                vaoModel.UpdateAttrib(0, "position", buf_vtx, 12, 3, buf_idx);
                //vaoModel.UpdateNormals(buf_nor);
                vaoModel.UpdateColors(buf_col);
                vaoModel.UpdateAttrib(0, "uv", buf_uv, 8, 2, buf_idx);
                vaoModel.UpdateAttrib(1, "tex", buf_tex, 4, 1, buf_idx);

                SetBlendForRenderPass(pass);
                vaoModel.Render(render, vertcount: buf_idx);
            }
        }

        private void RenderVertex(OldFrame f1, OldFrame f2, int id)
        {
            if (f2 == null)
            {
                OldFrameVertex v = f1.Vertices[id];
                buf_vtx[buf_idx] = new(v.X, v.Y, v.Z);
                //buf_nor[buf_idx] = new(v.NormalX, v.NormalY, v.NormalZ);
                buf_idx++;
            }
            else
            {
                OldFrameVertex v1 = f1.Vertices[id];
                OldFrameVertex v2 = f2.Vertices[id];
                buf_vtx[buf_idx] = MathExt.Lerp(new Vector3(v1.X, v1.Y, v1.Z), new Vector3(v2.X, v2.Y, v2.Z), interp);
                //buf_nor[buf_idx] = MathExt.Lerp(new Vector3(v1.NormalX, v1.NormalY, v1.NormalZ), new Vector3(v2.NormalX, v2.NormalY, v2.NormalZ), interp);
                buf_idx++;
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
            vaoModel.GLDispose();
            base.GLDispose();
        }
    }
}
