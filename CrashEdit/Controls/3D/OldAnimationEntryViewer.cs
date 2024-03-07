using CrashEdit.CE.Properties;
using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public sealed class OldAnimationEntryViewer : BaseAnimationEntryViewer
    {
        private bool colored;
        private bool _normals = Settings.Default.DisplayNormals;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1) : base(nsf, anim_eid, frame) { }

        private List<OldFrame>? GetFrames()
        {
            List<OldFrame>? frames = null;
            {
                var entry = nsf.GetEntry<Entry>(animId);
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
                var frames = GetFrames();
                if (frames != null)
                {
                    var usedframes = new List<OldFrame>();
                    if (animFrame != -1)
                        usedframes.Add(frames[animFrame]);
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
                        var frame_offset = new Position(frame.XOffset, frame.YOffset, frame.ZOffset) - new Position(128, 128, 128);
                        var scale = new Position(mx, my, mz);
                        foreach (var vert in frame.Vertices)
                        {
                            yield return (new Position(vert.X, vert.Y, vert.Z) + frame_offset) * scale;
                        }
                    }
                }

                yield return new Position(0, 0, 0);
            }
        }

        protected override void Render()
        {
            base.Render();

            tpages.Clear();

            var frames = GetFrames();
            if (frames != null)
            {
                OldFrame? frame2 = null;
                float interp = 0;
                if (frames.Count == 1)
                    _curframe = 0;
                else if (animFrame != -1)
                    _curframe = animFrame;
                else
                {
                    double prog = render.FullCurrentFrame / 2;
                    _curframe = (int)Math.Floor(prog) % frames.Count;
                    if (_interpolate)
                    {
                        frame2 = frames[(int)Math.Ceiling(prog) % frames.Count];
                        interp = (float)(prog - Math.Truncate(prog));
                        // Console.WriteLine(string.Format("Render frame {1}+{2}/{0} (i {3})", anim.Frames.Count, _curframe, (int)Math.Ceiling(prog) % anim.Frames.Count, interp));
                    }
                }
                OldFrame frame1 = frames[_curframe];

                blend_mask = BlendMode.Solid;

                RenderFrame(frame1, 0);

                if (frame2 != null && frame2 != frame1 && frame1.Vertices.Count == frame2.Vertices.Count)
                {
                    // lerp results
                    RenderFrame(frame2, 1);

                    for (int i = 0; i < vaoModel[0].CurVert; ++i)
                    {
                        MathExt.Lerp(ref vaoModel[0].Verts[i].trans, vaoModel[1].Verts[i].trans, interp);
                        if (!colored)
                            vaoModel[0].Verts[i].normal = Vertex.PackNormal(MathExt.Lerp(Vertex.UnpackNormal(vaoModel[0].Verts[i].normal), Vertex.UnpackNormal(vaoModel[1].Verts[i].normal), interp));
                        else
                            MathExt.Lerp(ref vaoModel[0].Verts[i].rgba, vaoModel[1].Verts[i].rgba, interp);
                    }
                }

                UploadTPAGs();

                RenderPasses();

                if (_normals && !colored)
                {
                    for (int i = 0; i < vaoModel[0].CurVert; ++i)
                    {
                        vaoLines.PushAttrib(trans: vaoModel[0].Verts[i].trans, rgba: (Rgba)Color4.White);
                        vaoLines.PushAttrib(trans: vaoModel[0].Verts[i].trans + Vertex.UnpackNormal(vaoModel[0].Verts[i].normal) * 0.1f, rgba: (Rgba)Color4.Cyan);
                    }
                }
                if (_collision)
                {
                    var c1 = new Vector3(frame1.collision.X1, frame1.collision.Y1, frame1.collision.Z1) / GameScales.CollisionC1;
                    var c2 = new Vector3(frame1.collision.X2, frame1.collision.Y2, frame1.collision.Z2) / GameScales.CollisionC1;
                    var ct = new Vector3(frame1.collision.XOffset, frame1.collision.YOffset, frame1.collision.ZOffset) / GameScales.CollisionC1;
                    var pos = c1 + ct;
                    var size = c2 - c1;
                    AddBox(pos, size, new Rgba(0, 255, 0, 255 / 5), false);
                    AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
                }
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleNormals.Print(OnOffName(_normals));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleNormals)) _normals = !_normals;
        }

        private void AddTPAGs(OldModelEntry model)
        {
            // collect tpag eids
            foreach (OldModelStruct str in model.Structs)
            {
                if (str is OldModelTexture tex && !tpages.ContainsKey(tex.EID))
                {
                    tpages[tex.EID] = (short)tpages.Count;
                }
            }
        }

        private void RenderFrame(OldFrame frame, int buf)
        {
            var vao = vaoModel[buf];
            vao.DiscardVerts();

            var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
            if (model != null)
            {
                // setup textures
                AddTPAGs(model);

                // alloc buffers
                vao.TestRealloc(model.Polygons.Count * 3);

                var trans = new Vector3(frame.XOffset, frame.YOffset, frame.ZOffset) - new Vector3(128);
                var scale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);

                // render stuff
                foreach (OldModelPolygon polygon in model.Polygons)
                {
                    int cur_idx = vao.CurVert;
                    OldModelStruct str = model.Structs[polygon.Unknown & 0x7FFF];
                    if (str is OldModelTexture tex)
                    {
                        vao.Verts[cur_idx].rgba = new(tex.R, tex.G, tex.B, 255);

                        vao.Verts[cur_idx + 0].st = new(tex.U3, tex.V3);
                        vao.Verts[cur_idx + 1].st = new(tex.U2, tex.V2);
                        vao.Verts[cur_idx + 2].st = new(tex.U1, tex.V1);

                        vao.Verts[cur_idx].tex = new VertexTexInfo(tpages[tex.EID], color: tex.ColorMode, blend: tex.BlendMode,
                                                                                    clutx: tex.ClutX, cluty: tex.ClutY,
                                                                                    face: Convert.ToInt32(tex.N));
                        
                        blend_mask |= VertexTexInfo.GetBlendMode(tex.BlendMode);
                    }
                    else
                    {
                        OldSceneryColor col = (OldSceneryColor)str;
                        vao.Verts[cur_idx].rgba = new(col.R, col.G, col.B, 255);
                        vao.Verts[cur_idx].tex = new VertexTexInfo(-1, face: Convert.ToInt32(col.N));
                    }
                    vao.Verts[cur_idx + 1].rgba = vao.Verts[cur_idx].rgba;
                    vao.Verts[cur_idx + 2].rgba = vao.Verts[cur_idx].rgba;
                    vao.Verts[cur_idx + 1].tex = vao.Verts[cur_idx + 0].tex;
                    vao.Verts[cur_idx + 2].tex = vao.Verts[cur_idx + 0].tex;
                    RenderVertex(vao, frame.Vertices[polygon.VertexC / 6], trans, scale);
                    RenderVertex(vao, frame.Vertices[polygon.VertexB / 6], trans, scale);
                    RenderVertex(vao, frame.Vertices[polygon.VertexA / 6], trans, scale);
                }
            }
        }

        private void RenderVertex(VAO vao, in OldFrameVertex vert, Vector3 trans, Vector3 scale)
        {
            int cur_vert_idx = vao.CurVert;
            vao.Verts[cur_vert_idx].trans = (new Vector3(vert.X, vert.Y, vert.Z) + trans) * scale;
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
            vao.CurVert++;
        }
    }
}
