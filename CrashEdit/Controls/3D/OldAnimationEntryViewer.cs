using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryViewer : GLViewer, IDisposable
    {
        private readonly int eid_anim;
        private readonly int frame_id;
        private int cur_frame = 0;
        private bool colored;
        private bool collisionenabled = Settings.Default.DisplayFrameCollision;
        private bool normalsenabled = true;
        private bool interpenabled = true;
        private int cullmode = 0;

        private VAO vaoModel;
        private Vector3[] buf_vtx;
        private Vector3[] buf_nor;
        private Color4[] buf_col;
        private Vector2[] buf_uv;
        private int[] buf_tex;
        private int buf_idx;

        protected override bool UseGrid => true;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame)
        {
            eid_anim = anim_eid;
            frame_id = frame;
        }

        public OldAnimationEntryViewer(NSF nsf, int anim_eid) : base(nsf)
        {
            eid_anim = anim_eid;
            frame_id = -1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoModel = new(render.ShaderContext, "crash1", PrimitiveType.Triangles);
            vaoModel.ArtType = VAO.ArtTypeEnum.Crash1Anim;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                IList<OldFrame> frames = null;
                {
                    var svtx = nsf.GetEntry<OldAnimationEntry>(eid_anim);
                    if (svtx != null)
                    {
                        frames = svtx.Frames;
                        colored = false;
                    }
                    else
                    {
                        var cvtx = nsf.GetEntry<ColoredAnimationEntry>(eid_anim);
                        if (cvtx != null)
                        {
                            frames = cvtx.Frames;
                            colored = true;
                        }
                    }
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
                            mx = model.ScaleX / GameScales.ModelC1 / GameScales.AnimC1;
                            my = model.ScaleY / GameScales.ModelC1 / GameScales.AnimC1;
                            mz = model.ScaleZ / GameScales.ModelC1 / GameScales.AnimC1;
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
            {
                var svtx = nsf.GetEntry<OldAnimationEntry>(eid_anim);
                if (svtx != null)
                {
                    frames = svtx.Frames;
                    colored = false;
                }
                else
                {
                    var cvtx = nsf.GetEntry<ColoredAnimationEntry>(eid_anim);
                    if (cvtx != null)
                    {
                        frames = cvtx.Frames;
                        colored = true;
                    }
                }
            }
            if (frames != null)
            {
                OldFrame frame2 = null;
                float interp = 0;
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
                        frame2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - cur_frame);
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, cur_frame, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                OldFrame frame1 = frames[cur_frame];
                if (frame2 != null && (frame2.ModelEID != frame1.ModelEID || frame1.Vertices.Count != frame2.Vertices.Count)) frame2 = null;

                var model = nsf.GetEntry<OldModelEntry>(frame1.ModelEID);
                if (model == null) return;

                RenderFrame(frame1);

                // uniforms and static data
                vaoModel.UserTrans = new(frame1.XOffset, frame1.YOffset, frame1.ZOffset);
                vaoModel.UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
                vaoModel.UserCullMode = cullmode;

                if (frame2 != null)
                {
                    // do lerp
                    var buf_vtx1 = buf_vtx;
                    var buf_nor1 = buf_nor;
                    var buf_col1 = buf_col;
                    RenderFrame(frame2);
                    var buf_vtx2 = buf_vtx;
                    var buf_nor2 = buf_nor;
                    var buf_col2 = buf_col;

                    vaoModel.UserTrans = MathExt.Lerp(vaoModel.UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset), interp);

                    for (int i = 0; i < buf_idx; ++i)
                    {
                        buf_vtx[i] = MathExt.Lerp(buf_vtx1[i], buf_vtx2[i], interp);
                        if (!colored)
                            buf_nor[i] = MathExt.Lerp(buf_nor1[i], buf_nor2[i], interp);
                        else
                            buf_col[i] = MathExt.Lerp(buf_col1[i], buf_col2[i], interp);
                    }
                }

                vaoModel.UpdatePositions(buf_vtx);
                if (!colored)
                    vaoModel.UpdateNormals(buf_nor);
                vaoModel.UpdateColors(buf_col);
                vaoModel.UpdateUVs(buf_uv);
                vaoModel.UpdateAttrib(1, "tex", buf_tex, 4, 1);

                RenderFramePass(BlendMode.Solid);
                RenderFramePass(BlendMode.Trans);
                RenderFramePass(BlendMode.Subtractive);
                RenderFramePass(BlendMode.Additive);

                if (collisionenabled)
                {
                    SetBlendMode(BlendMode.Solid);
                    GL.DepthMask(false);
                    var c1 = new Vector3(frame1.X1, frame1.Y1, frame1.Z1) / GameScales.CollisionC1;
                    var c2 = new Vector3(frame1.X2, frame1.Y2, frame1.Z2) / GameScales.CollisionC1;
                    var ct = new Vector3(frame1.XGlobal, frame1.YGlobal, frame1.ZGlobal) / GameScales.CollisionC1;
                    var pos = (c1 + c2) / 2 + ct;
                    var size = (c2 - c1) / 2;
                    RenderBox(pos, size, new Color4(0, 1f, 0, 0.2f));
                    RenderBoxLine(pos, size, new Color4(0, 1f, 0, 1f));
                    GL.DepthMask(true);
                }
            }
        }

        protected override void RunLogic()
        {
            base.RunLogic();
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

        private void RenderFrame(OldFrame frame)
        {
            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                // setup textures
                var tex_eids = CollectTPAGs(model);
                SetupTPAGs(tex_eids);

                // alloc buffers
                int nb = model.Polygons.Count * 3;
                buf_vtx = new Vector3[nb];
                if (!colored)
                    buf_nor = new Vector3[nb];
                buf_col = new Color4[nb];
                buf_uv = new Vector2[nb];
                buf_tex = new int[nb]; // enable: 1, colormode: 2, blendmode: 2, clutx: 4, cluty: 7, doubleface: 1, page: X (>17 total)

                // render stuff
                buf_idx = 0;

                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
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
                        RenderVertex(frame.Vertices[polygon.VertexC / 6]);
                        RenderVertex(frame.Vertices[polygon.VertexB / 6]);
                        RenderVertex(frame.Vertices[polygon.VertexA / 6]);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        if (!colored)
                            buf_col[buf_idx] = new(col.R, col.G, col.B, 255);
                        else
                            buf_col[buf_idx] = new(col.R / (float)0x80, col.G / (float)0x80, col.B / (float)0x80, 255);
                        buf_col[buf_idx + 1] = buf_col[buf_idx];
                        buf_col[buf_idx + 2] = buf_col[buf_idx];
                        buf_tex[buf_idx + 2] = 0 | (Convert.ToInt32(col.N) << 16);
                        RenderVertex(frame.Vertices[polygon.VertexC / 6]);
                        RenderVertex(frame.Vertices[polygon.VertexB / 6]);
                        RenderVertex(frame.Vertices[polygon.VertexA / 6]);
                    }
                }
            }
        }

        private void RenderFramePass(BlendMode pass)
        {
            SetBlendMode(pass);
            vaoModel.BlendMask = (int)pass;
            vaoModel.Render(render, vertcount: buf_idx);
        }

        private void RenderVertex(OldFrameVertex vert)
        {
            buf_vtx[buf_idx] = new(vert.X, vert.Y, vert.Z);
            if (colored)
                buf_col[buf_idx] = new Color4(buf_col[buf_idx].R * vert.Red, buf_col[buf_idx].G * vert.Green, buf_col[buf_idx].B * vert.Blue, 1);
            else
                buf_nor[buf_idx] = new(vert.NormalX, vert.NormalY, vert.NormalZ);
            buf_idx++;
        }

        protected override void Dispose(bool disposing)
        {
            vaoModel?.Dispose();

            base.Dispose(disposing);
        }
    }
}
