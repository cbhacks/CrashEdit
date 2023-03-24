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
        // note: there's multiple buffers because of blending
        private const int ANIM_BUF_MAX = 2;
        private Vector3[][] buf_vtx;
        private Vector3[][] buf_nor;
        private Rgba[][] buf_col;
        private Vector2[][] buf_uv;
        private int[][] buf_tex;
        private int buf_idx;

        protected override bool UseGrid => true;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame) : base(nsf)
        {
            eid_anim = anim_eid;
            frame_id = frame;
        }

        public OldAnimationEntryViewer(NSF nsf, int anim_eid) : base(nsf)
        {
            eid_anim = anim_eid;
            frame_id = -1;
        }

        protected override void GLLoad()
        {
            base.GLLoad();

            vaoModel = new(render.ShaderContext, "crash1", PrimitiveType.Triangles);
            vaoModel.ArtType = VAO.ArtTypeEnum.Crash1Anim;
            buf_vtx = new Vector3[ANIM_BUF_MAX][];
            buf_nor = new Vector3[ANIM_BUF_MAX][];
            buf_col = new Rgba[ANIM_BUF_MAX][];
            buf_uv = new Vector2[ANIM_BUF_MAX][];
            buf_tex = new int[ANIM_BUF_MAX][];
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
            vaoModel.DiscardVerts();

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

                RenderFrame(frame1, 0);

                // uniforms and static data
                vaoModel.UserTrans = new(frame1.XOffset, frame1.YOffset, frame1.ZOffset);
                vaoModel.UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
                vaoModel.UserCullMode = cullmode;

                if (frame2 != null)
                {
                    // lerp results
                    RenderFrame(frame2, 1);

                    vaoModel.UserTrans = MathExt.Lerp(vaoModel.UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset), interp);

                    for (int i = 0; i < buf_idx; ++i)
                    {
                        buf_vtx[0][i] = MathExt.Lerp(buf_vtx[0][i], buf_vtx[1][i], interp);
                        if (!colored)
                            buf_nor[0][i] = MathExt.Lerp(buf_nor[0][i], buf_nor[1][i], interp);
                        else
                            buf_col[0][i] = MathExt.Lerp(buf_col[0][i], buf_col[1][i], interp);
                    }
                }

                for (int i = 0; i < buf_idx; ++i)
                {
                    vaoModel.PushAttrib(trans: buf_vtx[0][i], normal: buf_nor[0][i], rgba: buf_col[0][i], st: buf_uv[0][i], tex: buf_tex[0][i]);
                }

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

        private void RenderFrame(OldFrame frame, int buf)
        {
            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                // setup textures
                var tex_eids = CollectTPAGs(model);
                SetupTPAGs(tex_eids);

                // alloc buffers
                int nb = model.Polygons.Count * 3;
                if (buf_vtx[buf] == null || buf_vtx[buf].Length < nb)
                    buf_vtx[buf] = new Vector3[nb];
                if (buf_nor[buf] == null || buf_nor[buf].Length < nb)
                    buf_nor[buf] = new Vector3[nb];
                if (buf_col[buf] == null || buf_col[buf].Length < nb)
                    buf_col[buf] = new Rgba[nb];
                if (buf_uv[buf] == null || buf_uv[buf].Length < nb)
                    buf_uv[buf] = new Vector2[nb];
                if (buf_tex[buf] == null || buf_tex[buf].Length < nb)
                    buf_tex[buf] = new int[nb]; // enable: 1, colormode: 2, blendmode: 2, clutx: 4, cluty: 7, doubleface: 1, page: X (>17 total)

                // render stuff
                buf_idx = 0;

                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        buf_col[buf][buf_idx] = new(tex.R, tex.G, tex.B, 255);
                        buf_col[buf][buf_idx + 1] = buf_col[buf][buf_idx];
                        buf_col[buf][buf_idx + 2] = buf_col[buf][buf_idx];
                        buf_uv[buf][buf_idx + 0] = new(tex.U3, tex.V3);
                        buf_uv[buf][buf_idx + 1] = new(tex.U2, tex.V2);
                        buf_uv[buf][buf_idx + 2] = new(tex.U1, tex.V1);
                        buf_tex[buf][buf_idx + 2] = MakeTexInfo(true, color: tex.ColorMode, blend: tex.BlendMode,
                                                                      clutx: tex.ClutX, cluty: tex.ClutY,
                                                                      face: Convert.ToInt32(tex.N),
                                                                      page: tex_eids[tex.EID]);
                        RenderVertex(frame.Vertices[polygon.VertexC / 6], buf);
                        RenderVertex(frame.Vertices[polygon.VertexB / 6], buf);
                        RenderVertex(frame.Vertices[polygon.VertexA / 6], buf);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        buf_col[buf][buf_idx] = new(col.R, col.G, col.B, 255);
                        buf_col[buf][buf_idx + 1] = buf_col[buf][buf_idx];
                        buf_col[buf][buf_idx + 2] = buf_col[buf][buf_idx];
                        buf_tex[buf][buf_idx + 2] = 0 | (Convert.ToInt32(col.N) << 16);
                        buf_tex[buf][buf_idx + 2] = MakeTexInfo(false, face: Convert.ToInt32(col.N));
                        RenderVertex(frame.Vertices[polygon.VertexC / 6], buf);
                        RenderVertex(frame.Vertices[polygon.VertexB / 6], buf);
                        RenderVertex(frame.Vertices[polygon.VertexA / 6], buf);
                    }
                }
            }
        }

        private void RenderFramePass(BlendMode pass)
        {
            SetBlendMode(pass);
            vaoModel.BlendMask = (int)pass;
            vaoModel.Render(render);
        }

        private void RenderVertex(OldFrameVertex vert, int buf)
        {
            buf_vtx[buf][buf_idx] = new(vert.X, vert.Y, vert.Z);
            if (colored)
                buf_col[buf][buf_idx] = new Rgba((byte)(buf_col[buf][buf_idx].r * 2 * vert.Red),
                                                 (byte)(buf_col[buf][buf_idx].g * 2 * vert.Green),
                                                 (byte)(buf_col[buf][buf_idx].b * 2 * vert.Blue), 255);
            else
                buf_nor[buf][buf_idx] = new(vert.NormalX, vert.NormalY, vert.NormalZ);
            buf_idx++;
        }

        protected override void Dispose(bool disposing)
        {
            vaoModel?.Dispose();

            base.Dispose(disposing);
        }
    }
}
