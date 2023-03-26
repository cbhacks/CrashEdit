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
        private int cullmode = 1;

        // note: there's multiple buffers because of blending
        private const int ANIM_BUF_MAX = 2;
        private VAO[] vaoModel = new VAO[ANIM_BUF_MAX];
        private BlendMode blendMask;

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

            for (int i = 0; i < ANIM_BUF_MAX; ++i)
            {
                vaoModel[i] = new(shaderContext, "crash1", PrimitiveType.Triangles);
            }
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
                    double prog = render.FullCurrentFrame / 2;
                    cur_frame = (int)Math.Floor(prog) % frames.Count;
                    if (interpenabled)
                    {
                        frame2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - Math.Floor(prog));
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, cur_frame, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                OldFrame frame1 = frames[cur_frame];
                if (frame2 != null && (frame2.ModelEID != frame1.ModelEID || frame1.Vertices.Count != frame2.Vertices.Count)) frame2 = null;

                var model = nsf.GetEntry<OldModelEntry>(frame1.ModelEID);
                if (model == null) return;

                blendMask = BlendMode.Solid;

                RenderFrame(frame1, 0);

                // uniforms and static data
                vaoModel[0].UserTrans = new(frame1.XOffset, frame1.YOffset, frame1.ZOffset);
                vaoModel[0].UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
                vaoModel[0].UserCullMode = cullmode;

                if (frame2 != null)
                {
                    // lerp results
                    RenderFrame(frame2, 1);

                    vaoModel[0].UserTrans = MathExt.Lerp(vaoModel[0].UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset), interp);

                    for (int i = 0; i < vaoModel[0].VertCount; ++i)
                    {
                        vaoModel[0].Verts[i].trans = MathExt.Lerp(vaoModel[0].Verts[i].trans, vaoModel[1].Verts[i].trans, interp);
                        if (!colored)
                            vaoModel[0].Verts[i].normal = MathExt.Lerp(vaoModel[0].Verts[i].normal, vaoModel[1].Verts[i].normal, interp);
                        else
                            vaoModel[0].Verts[i].rgba = MathExt.Lerp(vaoModel[0].Verts[i].rgba, vaoModel[1].Verts[i].rgba, interp);
                    }
                }

                vaoModel[0].UserTrans -= new Vector3(128);

                // note: only buffer 0 is rendered
                RenderFramePass(BlendMode.Solid);
                RenderFramePass(BlendMode.Trans);
                RenderFramePass(BlendMode.Subtractive);
                RenderFramePass(BlendMode.Additive);

                if (normalsenabled && !colored)
                {
                    var ofs = vaoModel[0].UserTrans;
                    for (int i = 0; i < vaoModel[0].VertCount; ++i)
                    {
                        var p = (vaoModel[0].Verts[i].trans + ofs) * vaoModel[0].UserScale;
                        vaoLines.PushAttrib(trans: p, rgba: (Rgba)Color4.White);
                        vaoLines.PushAttrib(trans: p + vaoModel[0].Verts[i].normal * 0.1f, rgba: (Rgba)Color4.Cyan);
                    }
                }
                if (collisionenabled)
                {
                    var c1 = new Vector3(frame1.X1, frame1.Y1, frame1.Z1) / GameScales.CollisionC1;
                    var c2 = new Vector3(frame1.X2, frame1.Y2, frame1.Z2) / GameScales.CollisionC1;
                    var ct = new Vector3(frame1.XGlobal, frame1.YGlobal, frame1.ZGlobal) / GameScales.CollisionC1;
                    var pos = c1 + ct;
                    var size = c2 - c1;
                    AddBox(pos, size, new Rgba(0, 255, 0, 255/5), false);
                    AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
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
                vaoModel[buf].VertCount = nb;
                vaoModel[buf].TestRealloc();
                vaoModel[buf].DiscardVerts();

                // render stuff
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    int cur_idx = vaoModel[buf].VertCount;
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        vaoModel[buf].Verts[cur_idx].rgba = new(tex.R, tex.G, tex.B, 255);
                        vaoModel[buf].Verts[cur_idx + 1].rgba = vaoModel[buf].Verts[cur_idx].rgba;
                        vaoModel[buf].Verts[cur_idx + 2].rgba = vaoModel[buf].Verts[cur_idx].rgba;
                        vaoModel[buf].Verts[cur_idx + 0].st = new(tex.U3, tex.V3);
                        vaoModel[buf].Verts[cur_idx + 1].st = new(tex.U2, tex.V2);
                        vaoModel[buf].Verts[cur_idx + 2].st = new(tex.U1, tex.V1);
                        vaoModel[buf].Verts[cur_idx + 2].tex = TexInfoUnpacked.Pack(true, color: tex.ColorMode, blend: tex.BlendMode,
                                                                                          clutx: tex.ClutX, cluty: tex.ClutY,
                                                                                          face: Convert.ToInt32(tex.N),
                                                                                          page: tex_eids[tex.EID]);
                        RenderVertex(frame, polygon.VertexC / 6, buf);
                        RenderVertex(frame, polygon.VertexB / 6, buf);
                        RenderVertex(frame, polygon.VertexA / 6, buf);

                        blendMask |= TexInfoUnpacked.GetBlendMode(tex.BlendMode);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        vaoModel[buf].Verts[cur_idx].rgba = new(col.R, col.G, col.B, 255);
                        vaoModel[buf].Verts[cur_idx + 1].rgba = vaoModel[buf].Verts[cur_idx].rgba;
                        vaoModel[buf].Verts[cur_idx + 2].rgba = vaoModel[buf].Verts[cur_idx].rgba;
                        vaoModel[buf].Verts[cur_idx + 2].tex = TexInfoUnpacked.Pack(false, face: Convert.ToInt32(col.N));
                        RenderVertex(frame, polygon.VertexC / 6, buf);
                        RenderVertex(frame, polygon.VertexB / 6, buf);
                        RenderVertex(frame, polygon.VertexA / 6, buf);
                    }
                }
            }
        }

        private void RenderFramePass(BlendMode pass)
        {
            if ((pass & blendMask) != BlendMode.None)
            {
                SetBlendMode(pass);
                vaoModel[0].BlendMask = BlendModeIndex(pass);
                vaoModel[0].Render(render);
            }
        }

        private void RenderVertex(OldFrame frame, int vert_idx, int buf)
        {
            OldFrameVertex vert = frame.Vertices[vert_idx];
            int cur_vert_idx = vaoModel[buf].VertCount;
            vaoModel[buf].Verts[cur_vert_idx].trans = new(vert.X, vert.Y, vert.Z);
            if (colored)
            {
                Rgba old_rgba = vaoModel[buf].Verts[cur_vert_idx].rgba;
                vaoModel[buf].Verts[cur_vert_idx].rgba = new Rgba((byte)(old_rgba.r * 2 * vert.Red),
                                                                  (byte)(old_rgba.g * 2 * vert.Green),
                                                                  (byte)(old_rgba.b * 2 * vert.Blue), 255);
            }
            else
            {
                vaoModel[buf].Verts[cur_vert_idx].normal = new Vector3(vert.NormalX, vert.NormalY, vert.NormalZ) / 127;
            }
            vaoModel[buf].VertCount++;
        }

        protected override void Dispose(bool disposing)
        {
            for (int i = 0; i < ANIM_BUF_MAX; ++i)
            {
                vaoModel[i]?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
