using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class OldAnimationEntryViewer : GLViewer
    {
        private readonly int eid_anim;
        private readonly int frame_id;
        private int cur_frame = 0;
        private bool colored;
        private bool enable_collision = Settings.Default.DisplayFrameCollision;
        private bool enable_normals = Settings.Default.DisplayNormals;
        private bool enable_interp = true;
        private int cull_mode = 1;

        private VAO[] vaoModel => vaoListCrash1;
        private BlendMode blend_mask;

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

        private IList<OldFrame> GetFrames()
        {
            IList<OldFrame> frames = null;
            {
                var entry = nsf.GetEntry<Entry>(eid_anim);
                if (entry is OldAnimationEntry svtx)
                {
                    frames = svtx.Frames;
                    colored = false;
                }
                else if (entry is ColoredAnimationEntry cvtx)
                {
                    frames = cvtx.Frames;
                    colored = true;
                }
            }
            return frames;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                IList<OldFrame> frames = GetFrames();
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

            IList<OldFrame> frames = GetFrames();
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
                    if (enable_interp)
                    {
                        frame2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - Math.Floor(prog));
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, cur_frame, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                OldFrame frame1 = frames[cur_frame];
                if (frame2 != null && (frame2.ModelEID != frame1.ModelEID || frame1.Vertices.Count != frame2.Vertices.Count))
                    frame2 = null;

                var model = nsf.GetEntry<OldModelEntry>(frame1.ModelEID);
                if (model == null) return;

                blend_mask = BlendMode.Solid;

                RenderFrame(frame1, 0);

                // uniforms and static data
                vaoModel[0].UserTrans = new(frame1.XOffset, frame1.YOffset, frame1.ZOffset);
                vaoModel[0].UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
                vaoModel[0].UserCullMode = cull_mode;

                if (frame2 != null && frame2 != frame1)
                {
                    // lerp results
                    RenderFrame(frame2, 1);

                    MathExt.Lerp(ref vaoModel[0].UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset), interp);

                    for (int i = 0; i < vaoModel[0].vert_count; ++i)
                    {
                        MathExt.Lerp(ref vaoModel[0].Verts[i].trans, vaoModel[1].Verts[i].trans, interp);
                        if (!colored)
                            vaoModel[0].Verts[i].normal = Vertex.PackNormal(MathExt.Lerp(Vertex.UnpackNormal(vaoModel[0].Verts[i].normal), Vertex.UnpackNormal(vaoModel[1].Verts[i].normal), interp));
                        else
                            MathExt.Lerp(ref vaoModel[0].Verts[i].rgba, vaoModel[1].Verts[i].rgba, interp);
                    }
                }

                vaoModel[0].UserTrans -= new Vector3(128);

                // note: only buffer 0 is rendered
                RenderFramePass(BlendMode.Solid);
                RenderFramePass(BlendMode.Trans);
                RenderFramePass(BlendMode.Subtractive);
                RenderFramePass(BlendMode.Additive);

                if (enable_normals && !colored)
                {
                    var ofs = vaoModel[0].UserTrans;
                    for (int i = 0; i < vaoModel[0].vert_count; ++i)
                    {
                        var p = (vaoModel[0].Verts[i].trans + ofs) * vaoModel[0].UserScale;
                        vaoLines.PushAttrib(trans: p, rgba: (Rgba)Color4.White);
                        vaoLines.PushAttrib(trans: p + Vertex.UnpackNormal(vaoModel[0].Verts[i].normal) * 0.1f, rgba: (Rgba)Color4.Cyan);
                    }
                }
                if (enable_collision)
                {
                    var c1 = new Vector3(frame1.collision.X1, frame1.collision.Y1, frame1.collision.Z1) / GameScales.CollisionC1;
                    var c2 = new Vector3(frame1.collision.X2, frame1.collision.Y2, frame1.collision.Z2) / GameScales.CollisionC1;
                    var ct = new Vector3(frame1.collision.XOffset, frame1.collision.YOffset, frame1.collision.ZOffset) / GameScales.CollisionC1;
                    var pos = c1 + ct;
                    var size = c2 - c1;
                    AddBox(pos, size, new Rgba(0, 255, 0, 255 / 5), false);
                    AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
                }

                // restore things
                vaoModel[0].UserTrans = new Vector3(0);
                vaoModel[0].UserScale = new Vector3(1);
                vaoModel[0].UserCullMode = 0;
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleCollisionAnim.Print(OnOffName(enable_collision));
            con_help += KeyboardControls.ToggleLerp.Print(OnOffName(enable_interp));
            con_help += KeyboardControls.ChangeCullMode.Print(cull_mode);
            con_help += KeyboardControls.ToggleNormals.Print(OnOffName(enable_normals));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleCollisionAnim)) enable_collision = !enable_collision;
            if (KPress(KeyboardControls.ToggleLerp)) enable_interp = !enable_interp;
            if (KPress(KeyboardControls.ChangeCullMode)) cull_mode = ++cull_mode % 3;
            if (KPress(KeyboardControls.ToggleNormals)) enable_normals = !enable_normals;
        }

        private Dictionary<int, short> CollectTPAGs(OldModelEntry model)
        {
            // collect tpag eids
            Dictionary<int, short> tex_eids = new();
            foreach (OldModelStruct str in model.Structs)
            {
                if (str is OldModelTexture tex && !tex_eids.ContainsKey(tex.EID))
                {
                    tex_eids[tex.EID] = (short)tex_eids.Count;
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
                var vao = vaoModel[buf];
                int nb = model.Polygons.Count * 3;
                vao.TestRealloc(nb);
                vao.DiscardVerts();

                // render stuff
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    int cur_idx = vao.vert_count;
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        vao.Verts[cur_idx].rgba = new(tex.R, tex.G, tex.B, 255);
                        vao.Verts[cur_idx + 1].rgba = vao.Verts[cur_idx].rgba;
                        vao.Verts[cur_idx + 2].rgba = vao.Verts[cur_idx].rgba;
                        vao.Verts[cur_idx + 0].st = new(tex.U3, tex.V3);
                        vao.Verts[cur_idx + 1].st = new(tex.U2, tex.V2);
                        vao.Verts[cur_idx + 2].st = new(tex.U1, tex.V1);
                        vao.Verts[cur_idx + 0].tex = new VertexTexInfo(tex_eids[tex.EID], color: tex.ColorMode, blend: tex.BlendMode,
                                                                                          clutx: tex.ClutX, cluty: tex.ClutY,
                                                                                          face: Convert.ToInt32(tex.N));
                        vao.Verts[cur_idx + 1].tex = vao.Verts[cur_idx + 0].tex;
                        vao.Verts[cur_idx + 2].tex = vao.Verts[cur_idx + 0].tex;
                        RenderVertex(vao, frame, polygon.VertexC / 6);
                        RenderVertex(vao, frame, polygon.VertexB / 6);
                        RenderVertex(vao, frame, polygon.VertexA / 6);

                        blend_mask |= VertexTexInfo.GetBlendMode(tex.BlendMode);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        vao.Verts[cur_idx].rgba = new(col.R, col.G, col.B, 255);
                        vao.Verts[cur_idx + 1].rgba = vao.Verts[cur_idx].rgba;
                        vao.Verts[cur_idx + 2].rgba = vao.Verts[cur_idx].rgba;
                        vao.Verts[cur_idx + 0].tex = new VertexTexInfo(-1, face: Convert.ToInt32(col.N));
                        vao.Verts[cur_idx + 1].tex = vao.Verts[cur_idx + 0].tex;
                        vao.Verts[cur_idx + 2].tex = vao.Verts[cur_idx + 0].tex;
                        RenderVertex(vao, frame, polygon.VertexC / 6);
                        RenderVertex(vao, frame, polygon.VertexB / 6);
                        RenderVertex(vao, frame, polygon.VertexA / 6);
                    }
                }
            }
        }

        private void RenderFramePass(BlendMode pass)
        {
            if ((pass & blend_mask) != BlendMode.None)
            {
                SetBlendMode(pass);
                vaoModel[0].BlendMask = BlendModeIndex(pass);
                vaoModel[0].Render(render);
            }
        }

        private void RenderVertex(VAO vao, OldFrame frame, int vert_idx)
        {
            OldFrameVertex vert = frame.Vertices[vert_idx];
            int cur_vert_idx = vao.vert_count;
            vao.Verts[cur_vert_idx].trans = new(vert.X, vert.Y, vert.Z);
            if (colored)
            {
                Rgba old_rgba = vao.Verts[cur_vert_idx].rgba;
                vao.Verts[cur_vert_idx].rgba = new Rgba((byte)(old_rgba.r * 2 * vert.Red),
                                                        (byte)(old_rgba.g * 2 * vert.Green),
                                                        (byte)(old_rgba.b * 2 * vert.Blue), 255);
            }
            else
            {
                vao.Verts[cur_vert_idx].normal = Vertex.PackNormal(new Vector3(vert.NormalX, vert.NormalY, vert.NormalZ) / 127);
            }
            vao.vert_count++;
        }
    }
}
