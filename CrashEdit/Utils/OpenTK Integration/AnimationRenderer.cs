using CrashEdit.Crash;
using OpenTK.Mathematics;
using static CrashEdit.CE.GLViewer;

namespace CrashEdit.CE
{
    internal sealed class AnimationRenderer
    {
        public TexturePageList TPages { get; set; }
        public RenderInfo Render { get; set; }

        public bool Interpolate { get; set; }
        public bool HalfSpeed { get; set; }
        public BlendMode BlendMask { get; private set; }
        public Frame BaseFrame { get; private set; }
        
        private Vector3 _globaltrans;
        private Vector3 _globalscale;
        private Matrix3 _globalrot;
        private Func<Frame, ModelEntry?> _getmodelfunc;

        private Vector3[]?[] _uncompressedverts = new Vector3[2][];
        private int _uncompressedvertcount;

        public Vector3[]? GetUncompressedVerts()
        {
            return _uncompressedverts[0]?.Take(new Range(0, _uncompressedvertcount)).ToArray();
        }

        public void Setup(bool interpolate, bool halfspeed)
        {
            Interpolate = interpolate;
            HalfSpeed = halfspeed;
            BlendMask = BlendMode.None;
        }

        public bool RenderAnimFrame(Vector3 trans, VAO[] vaos, AnimationEntry? anim, double frame, Func<Frame, ModelEntry?> get_model_func, Vector3 scale = default, Vector3 rot = default)
        {
            BaseFrame = null;

            var frames = anim?.Frames;
            if (frames == null)
                return false;

            _globaltrans = trans;
            _globalscale = scale == Vector3.Zero ? Vector3.One : scale;
            _globalrot = MathExt.EulerToMat3_Z_XY(rot);
            _getmodelfunc = get_model_func!;

            Frame? frame2 = null;
            float interp = 0;
            int curframe = 0;
            if (frames.Count != 1)
            {
                if (HalfSpeed)
                    frame /= 2;
                curframe = (int)((long)Math.Floor(frame) % frames.Count);
                if (vaos[1] != null)
                {
                    if (Interpolate)
                    {
                        frame2 = frames[(int)((long)Math.Ceiling(frame) % frames.Count)];
                        interp = (float)frame.TruncatePart();
                    }
                    else if (HalfSpeed)
                    {
                        if (((long)(frame * 2) & 0x1) != 0)
                        {
                            frame2 = frames[(curframe + 1) % frames.Count];
                            interp = 0.5f;
                        }
                    }
                }
            }
            var frame1 = frames[curframe]!;

            BlendMask = BlendMode.Solid;

            int startvert1 = vaos[0].CurVert;
            int startvert2 = vaos[1] == null? 0 : vaos[1].CurVert;

            if (!RenderFrame(vaos, frame1, 0))
                return false;

            if (frame2 != null && frame2 != frame1 && frame1.Vertices.Count == frame2.Vertices.Count && RenderFrame(vaos, frame2, 1))
            {
                for (int i = 0; i < frame1.SpecialVertexCount; ++i)
                {
                    MathExt.Lerp(ref _uncompressedverts[0][i], _uncompressedverts[1][i], interp);
                }
                for (int i = 0; i < (vaos[0].CurVert - startvert1); ++i)
                {
                    MathExt.Lerp(ref vaos[0].Verts[i + startvert1].trans, vaos[1].Verts[i + startvert2].trans, interp);
                    MathExt.Lerp(ref vaos[0].Verts[i + startvert1].rgba, vaos[1].Verts[i + startvert2].rgba, interp);
                }
                // these won't be rendered, so who cares
                vaos[1].CurVert = startvert2;
            }

            _uncompressedvertcount = frame1.SpecialVertexCount;

            BaseFrame = frame1;

            return true;
        }

        private void AddTPAGs(ModelEntry model)
        {
            // collect tpag eids
            for (int i = 0, m = model.TPAGCount; i < m; ++i)
            {
                TPages.AddTexturePage(model.GetTPAG(i));
            }
        }

        private bool RenderFrame(VAO[] vaos, Frame frame, int buf)
        {
            var vao = vaos[buf];

            var model = _getmodelfunc(frame);
            if (model == null)
                return false;

            // setup textures
            AddTPAGs(model);

            // alloc buffers
            vao.TestReallocExtra(model.Triangles.Count * 3);
            if (_uncompressedverts[buf] == null)
            {
                _uncompressedverts[buf] = new Vector3[frame.SpecialVertexCount];
            }
            else if (_uncompressedverts[buf].Length < frame.SpecialVertexCount)
            {
                Array.Resize(ref _uncompressedverts[buf], frame.SpecialVertexCount);
            }

            // decompress vertices, on the fly right now
            var verts = frame.MakeVertices(model);
            var trans = new Vector3(frame.XOffset, frame.YOffset, frame.ZOffset) / 4;
            var scale = new Vector3(model.ScaleX, model.ScaleY, model.ScaleZ) / (GameScales.ModelC1 * GameScales.AnimC1);

            // render stuff
            for (int i = 0; i < frame.SpecialVertexCount; ++i)
            {
                _uncompressedverts[buf][i] = (new Vector3(verts[i].X, verts[i].Z, verts[i].Y) + trans) * scale;
            }
            foreach (var tri in model.Triangles)
            {
                if (!TextureUtils.ProcessTextureInfoC2(Render.RealCurrentFrame / 2, tri.Texture, tri.Animated, model.Textures, model.AnimatedTextures, out var polygon_texture_info))
                    continue;
                bool nocull = tri.Subtype == 0 || tri.Subtype == 2;
                bool flip = (tri.Type == 2 ^ tri.Subtype == 3) && !nocull;
                VertexTexInfo tex = new(-1, face: nocull ? 1 : 0); // completely untextured
                if (polygon_texture_info != null)
                {
                    var info = polygon_texture_info;
                    tex = new(TPages[model.GetTPAG(info.Page)], color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, face: nocull ? 1 : 0);
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

                    BlendMask |= VertexTexInfo.GetBlendMode(info.BlendMode);
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
                    vao.Verts[vao.CurVert].trans = _globalrot * ((new Vector3(v.X, v.Z, v.Y) + trans) * scale) * _globalscale + _globaltrans;
                    vao.CurVert++;
                }
            }

            return true;
        }
    }
}
