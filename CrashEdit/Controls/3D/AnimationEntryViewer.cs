using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public sealed class AnimationEntryViewer : BaseAnimationEntryViewer
    {
        private bool _halfspeed = false;

        public AnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1, int model_eid = Entry.NullEID) : base(nsf, anim_eid, frame, model_eid) { }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var frames = nsf.GetEntry<AnimationEntry>(animId)?.Frames;
                if (frames != null)
                {
                    var usedframes = new List<Frame>();
                    if (animFrame != -1)
                        usedframes.Add(frames[animFrame]);
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
                        var frame_offset = new Position(frame.XOffset / 4f, frame.YOffset / 4f, frame.ZOffset / 4f);
                        var scale = new Position(mx, my, mz);
                        foreach (var vert in frame.MakeVertices(model))
                        {
                            yield return (new Position(vert.X, vert.Z, vert.Y) + frame_offset) * scale;
                        }
                    }
                }
            }
        }

        private int GetModelEID(Frame frame)
        {
            return modelId != Entry.NullEID ? modelId : frame.ModelEID;
        }

        protected override void Render()
        {
            base.Render();

            tpages.Clear();

            var frames = nsf.GetEntry<AnimationEntry>(animId)?.Frames;
            if (frames != null)
            {
                Frame? frame2 = null;
                float interp = 0;
                if (frames.Count == 1)
                    _curframe = 0;
                else if (animFrame != -1)
                    _curframe = animFrame;
                else
                {
                    double prog = render.FullCurrentFrame / 2;
                    if (_halfspeed)
                        prog /= 2;
                    _curframe = (int)((long)Math.Floor(prog) % frames.Count);
                    if (_interpolate)
                    {
                        frame2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - Math.Truncate(prog));
                    }
                    else if (_halfspeed)
                    {
                        if (((long)(prog * 2) & 0x1) != 0)
                        {
                            frame2 = frames[(_curframe + 1) % frames.Count];
                            interp = 0.5f;
                        }
                    }
                }
                Frame frame1 = frames[_curframe];

                blend_mask = BlendMode.Solid;

                RenderFrame(frame1, 0);

                if (frame2 != null && frame2 != frame1 && frame1.Vertices.Count == frame2.Vertices.Count)
                {
                    // lerp results
                    RenderFrame(frame2, 1);

                    for (int i = 0; i < frame1.SpecialVertexCount; ++i)
                    {
                        MathExt.Lerp(ref vertCache[0][i], vertCache[1][i], interp);
                    }
                    for (int i = 0; i < vaoModel[0].CurVert; ++i)
                    {
                        MathExt.Lerp(ref vaoModel[0].Verts[i].trans, vaoModel[1].Verts[i].trans, interp);
                        MathExt.Lerp(ref vaoModel[0].Verts[i].rgba, vaoModel[1].Verts[i].rgba, interp);
                    }
                }

                UploadTPAGs();

                for (int i = 0; i < frame1.SpecialVertexCount; ++i)
                {
                    AddSprite(vertCache[0][i], new Vector2(0.32f), (Rgba)Color4.Magenta, OldResources.PointTexture);
                }

                // note: only buffer 0 is rendered
                vaoModel[0].UserCullMode = _cullmode;
                RenderFramePass(BlendMode.Solid);
                if (render.EnableTexture)
                {
                    RenderFramePass(BlendMode.Trans);
                    RenderFramePass(BlendMode.Subtractive);
                    RenderFramePass(BlendMode.Additive);
                }

                if (_collision)
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
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleSlowAnim.Print(OnOffName(_halfspeed));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleSlowAnim)) _halfspeed = !_halfspeed;
        }

        private void AddTPAGs(ModelEntry model)
        {
            // collect tpag eids
            for (int i = 0, m = model.TPAGCount; i < m; ++i)
            {
                int tpag_eid = model.GetTPAG(i);
                if (!tpages.ContainsKey(tpag_eid))
                    tpages[tpag_eid] = (short)tpages.Count;
            }
        }

        private void RenderFrame(Frame frame, int buf)
        {
            var vao = vaoModel[buf];
            vao.DiscardVerts();

            var model = nsf.GetEntry<ModelEntry>(GetModelEID(frame));
            if (model != null)
            {
                // setup textures
                AddTPAGs(model);

                // alloc buffers
                vao.TestRealloc(model.Triangles.Count * 3);
                if (vertCache[buf].Length < frame.SpecialVertexCount)
                {
                    Array.Resize(ref vertCache[buf], frame.SpecialVertexCount);
                }

                // decompress vertices, on the fly right now
                var verts = frame.MakeVertices(model);
                var trans = new Vector3(frame.XOffset, frame.YOffset, frame.ZOffset) / 4;
                var scale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);

                // render stuff
                for (int i = 0; i < frame.SpecialVertexCount; ++i)
                {
                    vertCache[buf][i] = (new Vector3(verts[i].X, verts[i].Z, verts[i].Y) + trans) * scale;
                }
                foreach (var tri in model.Triangles)
                {
                    if (!ProcessTextureInfoC2(tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures, out var polygon_texture_info))
                        continue;
                    bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                    bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
                    VertexTexInfo tex = new(-1, face: nocull ? 1 : 0); // completely untextured
                    if (polygon_texture_info != null)
                    {
                        var info = polygon_texture_info;
                        tex = new(tpages[model.GetTPAG(info.Page)], color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, face: nocull ? 1 : 0);
                        vao.Verts[vao.CurVert + 1].st = new(info.X2, info.Y2);
                        if ((tri.Type != 2 && !flip) || (tri.Type == 2 && tri.Subtype == 1))
                        {
                            vao.Verts[vao.CurVert + 0].st = new(info.X3, info.Y3);
                            vao.Verts[vao.CurVert + 1].st = new(info.X2, info.Y2);
                            vao.Verts[vao.CurVert + 2].st = new(info.X1, info.Y1);
                        }
                        else
                        {
                            vao.Verts[vao.CurVert + 0].st = new(info.X1, info.Y1);
                            vao.Verts[vao.CurVert + 1].st = new(info.X2, info.Y2);
                            vao.Verts[vao.CurVert + 2].st = new(info.X3, info.Y3);
                        }

                        blend_mask |= VertexTexInfo.GetBlendMode(info.BlendMode);
                    }
                    vao.Verts[vao.CurVert + 0].tex = tex;
                    vao.Verts[vao.CurVert + 1].tex = tex;
                    vao.Verts[vao.CurVert + 2].tex = tex;

                    for (int i = 0; i < 3; ++i)
                    {
                        var v_n = !flip ? i : 2 - i;
                        var c = model.Colors[tri.Color[v_n]];
                        var v = verts[tri.Vertex[v_n] + frame.SpecialVertexCount];
                        vao.Verts[vao.CurVert].rgba = new(c.Red, c.Green, c.Blue, 255);
                        vao.Verts[vao.CurVert].trans = (new Vector3(v.X, v.Z, v.Y) + trans) * scale;
                        vao.CurVert++;
                    }
                }
            }
        }
    }
}
