using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class AnimationEntryViewer : GLViewer
    {
        private readonly int eid_anim;
        private readonly int frame_id;
        private readonly int model_eid;
        private int cur_frame = 0;
        private bool enable_collision = Settings.Default.DisplayFrameCollision;
        private bool enable_interp = true;
        private int cull_mode = 1;

        private VAO[] vaoModel => vaoListCrash1;
        private BlendMode blend_mask;

        private readonly Vector3[][] transUncompressedVerts = new Vector3[ANIM_BUF_MAX][];

        public AnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1, int model_eid = Entry.NullEID) : base(nsf)
        {
            eid_anim = anim_eid;
            frame_id = frame;
            this.model_eid = model_eid;

            for (int i = 0; i < ANIM_BUF_MAX; ++i)
            {
                transUncompressedVerts[i] = new Vector3[0];
            }
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                IList<Frame> frames = nsf.GetEntry<AnimationEntry>(eid_anim)?.Frames;
                if (frames != null)
                {
                    var usedframes = new List<Frame>();
                    if (frame_id != -1)
                        usedframes.Add(frames[frame_id]);
                    else
                        usedframes.AddRange(frames);

                    foreach (Frame frame in usedframes)
                    {
                        var model = nsf.GetEntry<ModelEntry>(GetModelEID(frame));
                        float mx = 1 / 128f;
                        float my = 1 / 128f;
                        float mz = 1 / 128f;
                        if (model != null)
                        {
                            mx = model.ScaleX / GameScales.ModelC1 / GameScales.AnimC1;
                            my = model.ScaleY / GameScales.ModelC1 / GameScales.AnimC1;
                            mz = model.ScaleZ / GameScales.ModelC1 / GameScales.AnimC1;
                        }
                        foreach (var vert in frame.MakeVertices(model))
                        {
                            yield return (new Position(vert.X, vert.Z, vert.Y)
                                        + new Position(frame.XOffset / 4f, frame.YOffset / 4f, frame.ZOffset / 4f)) * new Position(mx, my, mz);
                        }
                    }
                }
            }
        }

        private int GetModelEID(Frame frame)
        {
            return model_eid != Entry.NullEID ? model_eid : frame.ModelEID;
        }

        protected override void Render()
        {
            base.Render();

            IList<Frame> frames = nsf.GetEntry<AnimationEntry>(eid_anim)?.Frames;
            if (frames != null)
            {
                Frame frame2 = null;
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
                Frame frame1 = frames[cur_frame];
                if (frame2 != null && (GetModelEID(frame2) != GetModelEID(frame1) || frame1.Vertices.Count != frame2.Vertices.Count))
                    frame2 = null;

                var model = nsf.GetEntry<ModelEntry>(GetModelEID(frame1));
                if (model == null) return;

                blend_mask = BlendMode.Solid;

                // uniforms and static data
                vaoModel[0].UserTrans = new Vector3(frame1.XOffset, frame1.YOffset, frame1.ZOffset) / 4;
                vaoModel[0].UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
                vaoModel[0].UserCullMode = cull_mode;

                RenderFrame(frame1, 0);

                if (frame2 != null && frame2 != frame1)
                {
                    MathExt.Lerp(ref vaoModel[0].UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset) / 4, interp);

                    // lerp results
                    RenderFrame(frame2, 1);

                    for (int i = 0; i < frame1.SpecialVertexCount; ++i)
                    {
                        MathExt.Lerp(ref transUncompressedVerts[0][i], transUncompressedVerts[1][i], interp);
                    }
                    for (int i = 0; i < vaoModel[0].vert_count; ++i)
                    {
                        MathExt.Lerp(ref vaoModel[0].Verts[i].trans, vaoModel[1].Verts[i].trans, interp);
                        MathExt.Lerp(ref vaoModel[0].Verts[i].rgba, vaoModel[1].Verts[i].rgba, interp);
                    }
                }

                for (int i = 0; i < frame1.SpecialVertexCount; ++i)
                {
                    AddSprite((transUncompressedVerts[0][i] + vaoModel[0].UserTrans) * vaoModel[0].UserScale, new Vector2(0.35f), (Rgba)Color4.Magenta, OldResources.PointTexture);
                }

                // note: only buffer 0 is rendered
                RenderFramePass(BlendMode.Solid);
                RenderFramePass(BlendMode.Trans);
                RenderFramePass(BlendMode.Subtractive);
                RenderFramePass(BlendMode.Additive);

                if (enable_collision)
                {
                    foreach (var col in frame1.Collision)
                    {
                        var c1 = new Vector3(col.X1, col.Y1, col.Z1) / GameScales.CollisionC1;
                        var c2 = new Vector3(col.X2, col.Y2, col.Z2) / GameScales.CollisionC1;
                        var ct = new Vector3(col.XOffset, col.YOffset, col.ZOffset) / GameScales.CollisionC1;
                        var pos = c1 + ct;
                        var size = c2 - c1;
                        AddBox(pos, size, new Rgba(0, 255, 0, 255 / 5), false);
                        AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
                    }
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
            con_help += KeyboardControls.ToggleCollisionAnim.Print(BoolToEnable(enable_collision));
            con_help += KeyboardControls.ToggleLerp.Print(BoolToEnable(enable_interp));
            con_help += KeyboardControls.ChangeCullMode.Print(cull_mode);
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleCollisionAnim)) enable_collision = !enable_collision;
            if (KPress(KeyboardControls.ToggleLerp)) enable_interp = !enable_interp;
            if (KPress(KeyboardControls.ChangeCullMode)) cull_mode = ++cull_mode % 3;
        }

        private Dictionary<int, int> CollectTPAGs(ModelEntry model)
        {
            // collect tpag eids
            Dictionary<int, int> tex_eids = new();
            for (int i = 0, m = model.TPAGCount; i < m; ++i)
            {
                int tpag_eid = model.GetTPAG(i);
                if (!tex_eids.ContainsKey(tpag_eid))
                    tex_eids[tpag_eid] = tex_eids.Count;
            }
            return tex_eids;
        }

        private void RenderFrame(Frame frame, int buf)
        {
            var model = nsf.GetEntry<ModelEntry>(GetModelEID(frame));
            if (model != null)
            {
                // setup textures
                var tex_eids = CollectTPAGs(model);
                SetupTPAGs(tex_eids);

                // alloc buffers
                var vao = vaoModel[buf];
                vao.TestRealloc(model.Triangles.Count * 3);
                vao.DiscardVerts();
                if (transUncompressedVerts[buf].Length < frame.SpecialVertexCount)
                {
                    Array.Resize(ref transUncompressedVerts[buf], frame.SpecialVertexCount);
                }

                // decompress vertices, on the fly right now
                var verts = frame.MakeVertices(model);

                // render stuff
                for (int i = 0; i < frame.SpecialVertexCount; ++i)
                {
                    transUncompressedVerts[buf][i] = new Vector3(verts[i].X, verts[i].Z, verts[i].Y);
                }
                foreach (var tri in model.Triangles)
                {
                    var polygon_texture_info = ProcessTextureInfoC2(tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures);
                    if (!polygon_texture_info.Item1)
                        continue;
                    bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                    bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
                    VertexTexInfo tex = new(false, face: nocull ? 1 : 0); // completely untextured
                    if (polygon_texture_info.Item2.HasValue)
                    {
                        var info = polygon_texture_info.Item2.Value;
                        tex = new(true, color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, face: nocull ? 1 : 0, page: tex_eids[model.GetTPAG(info.Page)]);
                        vao.Verts[vao.vert_count + 1].st = new(info.X2, info.Y2);
                        if ((tri.Type != 2 && !flip) || (tri.Type == 2 && tri.Subtype == 1))
                        {
                            vao.Verts[vao.vert_count + 0].st = new(info.X3, info.Y3);
                            vao.Verts[vao.vert_count + 1].st = new(info.X2, info.Y2);
                            vao.Verts[vao.vert_count + 2].st = new(info.X1, info.Y1);
                        }
                        else
                        {
                            vao.Verts[vao.vert_count + 0].st = new(info.X1, info.Y1);
                            vao.Verts[vao.vert_count + 1].st = new(info.X2, info.Y2);
                            vao.Verts[vao.vert_count + 2].st = new(info.X3, info.Y3);
                        }

                        blend_mask |= VertexTexInfo.GetBlendMode(info.BlendMode);
                    }
                    vao.Verts[vao.vert_count + 0].tex = tex;
                    vao.Verts[vao.vert_count + 1].tex = tex;
                    vao.Verts[vao.vert_count + 2].tex = tex;

                    for (int i = 0; i < 3; ++i)
                    {
                        var v_n = !flip ? i : 2 - i;
                        var c = model.Colors[tri.Color[v_n]];
                        var v = verts[tri.Vertex[v_n] + frame.SpecialVertexCount];
                        vao.Verts[vao.vert_count].rgba = new(c.Red, c.Green, c.Blue, 255);
                        vao.Verts[vao.vert_count].trans = new(v.X, v.Z, v.Y);
                        vao.vert_count++;
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
    }
}
