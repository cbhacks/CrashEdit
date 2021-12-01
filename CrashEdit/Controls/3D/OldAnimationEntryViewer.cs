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
        private int cur_frame = 0;
        private bool colored;
        private bool collisionenabled = Settings.Default.DisplayFrameCollision;
        private bool normalsenabled = true;
        private bool interpenabled = true;
        private float interp;
        private int cullmode = 0;

        private VAO vaoModel;
        private Vector3[] buf_vtx;
        //private Vector3[] buf_nor;
        private Color4[] buf_col;
        private Vector2[] buf_uv;
        private int[] buf_tex;
        private int buf_idx;

        protected override bool UseGrid => true;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame, bool colored)
        {
            this.nsf = nsf;
            eid_anim = anim_eid;
            frame_id = frame;
            this.colored = colored;
        }

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, bool colored)
        {
            this.nsf = nsf;
            eid_anim = anim_eid;
            frame_id = -1;
            this.colored = colored;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoModel = new VAO(render.ShaderContext, "anim_c1", PrimitiveType.Triangles);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                IList<OldFrame> frames = null;
                if (!colored)
                {
                    var anim = nsf.GetEntry<OldAnimationEntry>(eid_anim);
                    if (anim != null)
                        frames = anim.Frames;
                }
                else
                {
                    var anim = nsf.GetEntry<ColoredAnimationEntry>(eid_anim);
                    if (anim != null)
                        frames = anim.Frames;
                }
                if (frames != null)
                {
                    var usedframes = new List<OldFrame>();
                    if (frame_id != -1)
                        usedframes.Add(frames[frame_id]);
                    else
                        usedframes.AddRange(frames);

                    foreach (OldFrame frame in usedframes)
                    {
                        var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
                        float mx = 1 / 128f;
                        float my = 1 / 128f;
                        float mz = 1 / 128f;
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

            IList<OldFrame> frames = null;
            if (!colored)
            {
                var anim = nsf.GetEntry<OldAnimationEntry>(eid_anim);
                if (anim != null)
                    frames = anim.Frames;
            }
            else
            {
                var anim = nsf.GetEntry<ColoredAnimationEntry>(eid_anim);
                if (anim != null)
                    frames = anim.Frames;
            }
            if (frames != null)
            {
                OldFrame f2 = null;
                if (frames.Count == 1)
                {
                    cur_frame = 0;
                }
                else if (frame_id != -1)
                {
                    cur_frame = frame_id;
                }
                else
                {
                    double prog = render.FullCurrentFrame / 2 % frames.Count;
                    cur_frame = (int)Math.Floor(prog);
                    if (interpenabled)
                    {
                        f2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - cur_frame);
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, cur_frame, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                RenderFrame(frames[cur_frame], f2);
            }
        }

        protected override void ActualRunLogic()
        {
            base.ActualRunLogic();
            if (KPress(Keys.C)) collisionenabled = !collisionenabled;
            if (KPress(Keys.N)) normalsenabled = !normalsenabled;
            if (KPress(Keys.I)) interpenabled = !interpenabled;
            if (KPress(Keys.U)) cullmode = ++cullmode % 3;
        }

        private Dictionary<int, int> CollectTPAGs(OldModelEntry model)
        {
            // collect tpag eids
            Dictionary<int, int> tex_eids = new();
            foreach (OldModelStruct str in model.Structs)
            {
                if (str is OldModelTexture tex && !tex_eids.ContainsKey(tex.EID))
                {
                    tex_eids[tex.EID] = tex_eids.Count;
                }
            }
            return tex_eids;
        }

        private void RenderFrame(OldFrame frame, OldFrame frame2 = null)
        {
            if (frame2 != null && (frame2.ModelEID != frame.ModelEID || frame.Vertices.Count != frame2.Vertices.Count)) frame2 = null;

            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                // setup textures
                var tex_eids = CollectTPAGs(model);
                SetupTPAGs(nsf, tex_eids);

                // alloc buffers
                int nb = model.Polygons.Count * 3;
                buf_vtx = new Vector3[nb];
                //buf_nor = new Vector3[nb];
                buf_col = new Color4[nb];
                buf_uv = new Vector2[nb];
                buf_tex = new int[nb]; // enable: 1, colormode: 2, blendmode: 2, clutx: 4, cluty: 7, doubleface: 1, page: X (>17 total)

                // render stuff
                RenderFramePass(model, tex_eids, frame, frame2, RenderPass.Solid);
                RenderFramePass(model, tex_eids, frame, frame2, RenderPass.Trans);
                RenderFramePass(model, tex_eids, frame, frame2, RenderPass.Subtractive);
                RenderFramePass(model, tex_eids, frame, frame2, RenderPass.Additive);
            }

            if (collisionenabled)
            {
                SetBlendForRenderPass(RenderPass.Solid);
                GL.DepthMask(false);
                var c1 = new Vector3(frame.X1, frame.Y1, frame.Z1) / 102400;
                var c2 = new Vector3(frame.X2, frame.Y2, frame.Z2) / 102400;
                var ct = new Vector3(frame.XGlobal, frame.YGlobal, frame.ZGlobal) / 102400;
                var pos = (c1 + c2) / 2 + ct;
                var size = (c2 - c1) / 2;
                RenderBox(pos, size, new Color4(0, 1f, 0, 0.2f));
                RenderBoxLine(pos, size, new Color4(0, 1f, 0, 1f));
                GL.DepthMask(true);
            }
        }

        private void RenderFramePass(OldModelEntry model, Dictionary<int, int> tex_eids, OldFrame frame, OldFrame frame2, RenderPass pass)
        {
            buf_idx = 0;

            foreach (OldModelPolygon polygon in model.Polygons)
            {
                OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                if (str is OldModelTexture tex)
                {
                    //if (pass != RenderPass.Solid && (tex.BlendMode == 0 || tex.BlendMode == 3)) continue;
                    if (pass != RenderPass.Additive && tex.BlendMode == 1) continue;
                    if (pass != RenderPass.Subtractive && tex.BlendMode == 2) continue;
                    if (!colored)
                        buf_col[buf_idx] = new(tex.R, tex.G, tex.B, 255);
                    else
                        buf_col[buf_idx] = new(tex.R / (float)0x80, tex.G / (float)0x80, tex.B / (float)0x80, 255);
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
                    if (!colored)
                        buf_col[buf_idx] = new(col.R, col.G, col.B, 255);
                    else
                        buf_col[buf_idx] = new(col.R / (float)0x80, col.G / (float)0x80, col.B / (float)0x80, 255);
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
                render.Projection.UserBool1 = pass == RenderPass.Solid;

                vaoModel.UpdatePositions(buf_vtx, buf_idx);
                //vaoModel.UpdateNormals(buf_nor, buf_idx);
                vaoModel.UpdateColors(buf_col, buf_idx);
                vaoModel.UpdateUVs(buf_uv, buf_idx);
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
                if (colored)
                    buf_col[buf_idx] = new Color4(buf_col[buf_idx].R * v.Red, buf_col[buf_idx].G * v.Green, buf_col[buf_idx].B * v.Blue, 1);
                //buf_nor[buf_idx] = new(v.NormalX, v.NormalY, v.NormalZ);
                buf_idx++;
            }
            else
            {
                OldFrameVertex v1 = f1.Vertices[id];
                OldFrameVertex v2 = f2.Vertices[id];
                buf_vtx[buf_idx] = MathExt.Lerp(new Vector3(v1.X, v1.Y, v1.Z), new Vector3(v2.X, v2.Y, v2.Z), interp);
                if (colored)
                    buf_col[buf_idx] = new Color4(buf_col[buf_idx].R * MathExt.Lerp(v1.Red, v2.Red, interp),
                                                  buf_col[buf_idx].G * MathExt.Lerp(v1.Green, v2.Green, interp),
                                                  buf_col[buf_idx].B * MathExt.Lerp(v1.Blue, v2.Blue, interp),
                                                  1);
                //buf_nor[buf_idx] = MathExt.Lerp(new Vector3(v1.NormalX, v1.NormalY, v1.NormalZ), new Vector3(v2.NormalX, v2.NormalY, v2.NormalZ), interp);
                buf_idx++;
            }
        }

        public new void GLDispose()
        {
            vaoModel.GLDispose();
            base.GLDispose();
        }
    }
}
