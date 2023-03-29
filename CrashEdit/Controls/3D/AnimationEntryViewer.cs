using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryViewer : GLViewer
    {
        private readonly int eid_anim;
        private readonly int frame_id;
        private int cur_frame = 0;
        private bool collisionenabled = Settings.Default.DisplayFrameCollision;
        private bool interpenabled = true;
        private int cullmode = 1;

        private VAO[] vaoModel = vaoListCrash1;
        private BlendMode blendMask;

        protected override bool UseGrid => true;

        public AnimationEntryViewer(NSF nsf, int anim_eid, int frame) : base(nsf)
        {
            eid_anim = anim_eid;
            frame_id = frame;
        }

        public AnimationEntryViewer(NSF nsf, int anim_eid) : base(nsf)
        {
            eid_anim = anim_eid;
            frame_id = -1;
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
                        var model = nsf.GetEntry<ModelEntry>(frame.ModelEID);
                        float mx = 1 / 128f;
                        float my = 1 / 128f;
                        float mz = 1 / 128f;
                        if (model != null)
                        {
                            mx = model.ScaleX / GameScales.ModelC1 / GameScales.AnimC1;
                            my = model.ScaleY / GameScales.ModelC1 / GameScales.AnimC1;
                            mz = model.ScaleZ / GameScales.ModelC1 / GameScales.AnimC1;
                        }
                        foreach (var vert in frame.MakeVertices(nsf))
                        {
                            yield return (new Position(vert.X, vert.Y, vert.Z)
                                        + new Position(frame.XOffset / 4f, frame.YOffset / 4f, frame.ZOffset / 4f)) * new Position(mx, my, mz);
                        }
                    }
                }
            }
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
                    if (interpenabled)
                    {
                        frame2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - Math.Floor(prog));
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, cur_frame, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                Frame frame1 = frames[cur_frame];
                if (frame2 != null && (frame2.ModelEID != frame1.ModelEID || frame1.Vertices.Count != frame2.Vertices.Count)) frame2 = null;

                var model = nsf.GetEntry<ModelEntry>(frame1.ModelEID);
                if (model == null) return;

                blendMask = BlendMode.Solid;

                RenderFrame(frame1, 0);

                // uniforms and static data
                vaoModel[0].UserTrans = new Vector3(frame1.XOffset, frame1.YOffset, frame1.ZOffset) / 4;
                vaoModel[0].UserScale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);
                vaoModel[0].UserCullMode = cullmode;

                if (frame2 != null)
                {
                    // lerp results
                    RenderFrame(frame2, 1);

                    vaoModel[0].UserTrans = MathExt.Lerp(vaoModel[0].UserTrans, new Vector3(frame2.XOffset, frame2.YOffset, frame2.ZOffset) / 4, interp);

                    for (int i = 0; i < vaoModel[0].VertCount; ++i)
                    {
                        vaoModel[0].Verts[i].trans = MathExt.Lerp(vaoModel[0].Verts[i].trans, vaoModel[1].Verts[i].trans, interp);
                        vaoModel[0].Verts[i].rgba = MathExt.Lerp(vaoModel[0].Verts[i].rgba, vaoModel[1].Verts[i].rgba, interp);
                    }
                }

                // note: only buffer 0 is rendered
                RenderFramePass(BlendMode.Solid);
                RenderFramePass(BlendMode.Trans);
                RenderFramePass(BlendMode.Subtractive);
                RenderFramePass(BlendMode.Additive);

                if (collisionenabled)
                {
                    foreach (var col in frame1.Collision)
                    {
                        var c1 = new Vector3(col.X1, col.Y1, col.Z1) / GameScales.CollisionC1;
                        var c2 = new Vector3(col.X2, col.Y2, col.Z2) / GameScales.CollisionC1;
                        var ct = new Vector3(col.XO, col.YO, col.ZO) / GameScales.CollisionC1;
                        var pos = c1 + ct;
                        var size = c2 - c1;
                        AddBox(pos, size, new Rgba(0, 255, 0, 255 / 5), false);
                        AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
                    }
                }
            }
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(Keys.C)) collisionenabled = !collisionenabled;
            if (KPress(Keys.I)) interpenabled = !interpenabled;
            if (KPress(Keys.U)) cullmode = ++cullmode % 3;
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
            var model = nsf.GetEntry<ModelEntry>(frame.ModelEID);
            if (model != null)
            {
                // setup textures
                var tex_eids = CollectTPAGs(model);
                SetupTPAGs(tex_eids);

                // alloc buffers
                int nb = model.Triangles.Count * 3;
                vaoModel[buf].VertCount = nb;
                vaoModel[buf].TestRealloc();
                vaoModel[buf].DiscardVerts();

                // decompress vertices, on the fly right now
                IList<FrameVertex> verts = frame.MakeVertices(nsf);

                // render stuff
                foreach (var tri in model.Triangles)
                {
                    var polygon_texture_info = ProcessTextureInfoC2(tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures);
                    if (!polygon_texture_info.Item1)
                        continue;
                    bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                    bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
                    int tex = TexInfoUnpacked.Pack(false, face: nocull ? 1 : 0); // completely untextured
                    if (polygon_texture_info.Item2.HasValue)
                    {
                        var info = polygon_texture_info.Item2.Value;
                        tex |= TexInfoUnpacked.Pack(true, color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, page: tex_eids[model.GetTPAG(info.Page)]);
                        vaoModel[buf].Verts[vaoModel[buf].VertCount + 1].st = new(info.X2, info.Y2);
                        if ((tri.Type != 2 && !flip) || (tri.Type == 2 && tri.Subtype == 1))
                        {
                            vaoModel[buf].Verts[vaoModel[buf].VertCount + 0].st = new(info.X3, info.Y3);
                            vaoModel[buf].Verts[vaoModel[buf].VertCount + 1].st = new(info.X2, info.Y2);
                            vaoModel[buf].Verts[vaoModel[buf].VertCount + 2].st = new(info.X1, info.Y1);
                        }
                        else
                        {
                            vaoModel[buf].Verts[vaoModel[buf].VertCount + 0].st = new(info.X1, info.Y1);
                            vaoModel[buf].Verts[vaoModel[buf].VertCount + 1].st = new(info.X2, info.Y2);
                            vaoModel[buf].Verts[vaoModel[buf].VertCount + 2].st = new(info.X3, info.Y3);
                        }

                        blendMask |= TexInfoUnpacked.GetBlendMode(info.BlendMode);
                    }
                    vaoModel[buf].Verts[vaoModel[buf].VertCount + 2].tex = tex;

                    Rgba[] test = {
                        // AA
                        new Rgba(255, 0, 0, 255),
                        new Rgba(255, 128, 0, 255), // correct
                        new Rgba(255, 255, 0, 255),
                        new Rgba(128, 255, 0, 255), // correct
                        // BB
                        new Rgba(0, 255, 0, 255),
                        new Rgba(0, 255, 128, 255), // correct
                        new Rgba(0, 255, 255, 255),
                        new Rgba(0, 128, 255, 255), // correct
                        // CC
                        new Rgba(0, 0, 255, 255),
                        new Rgba(128, 0, 255, 255), // correct
                        new Rgba(255, 0, 255, 255),
                        new Rgba(255, 0, 128, 255), // correct
                    };

                    int[][] testi = {
                        // AA
                        new int[] { 0, 1, 2,},
                        new int[] { 0, 1, 2,},
                        new int[] { 0, 1, 2,},
                        new int[] { 2, 1, 0,},
                        // BB
                        new int[] { 0, 1, 2,},
                        new int[] { 0, 1, 2,},
                        new int[] { 0, 1, 2,},
                        new int[] { 2, 1, 0,},
                        // CC
                        new int[] { 0, 1, 2,},
                        new int[] { 2, 1, 0,},
                        new int[] { 0, 1, 2,},
                        new int[] { 0, 1, 2,},
                    };

                    for (int i = 0; i < 3; ++i)
                    {
                        var v_n = !flip ? i : 2 - i;
                        var c = model.Colors[tri.Color[v_n]];
                        var v = verts[tri.Vertex[v_n] + frame.SpecialVertexCount];
                        vaoModel[buf].Verts[vaoModel[buf].VertCount].rgba = new((byte)(test[tri.Type * 4 + tri.Subtype].r),
                                                                                (byte)(test[tri.Type * 4 + tri.Subtype].g),
                                                                                (byte)(test[tri.Type * 4 + tri.Subtype].b),
                                                                                255);
                        vaoModel[buf].Verts[vaoModel[buf].VertCount].rgba = new(c.Red, c.Green, c.Blue, 255);
                        vaoModel[buf].Verts[vaoModel[buf].VertCount].trans = new(v.X, v.Z, v.Y);
                        vaoModel[buf].VertCount++;
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
    }
}
