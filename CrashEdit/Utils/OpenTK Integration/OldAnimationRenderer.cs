using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    internal sealed class OldAnimationRenderer
    {
        public TexturePageList TPages { get; set; }
        public RenderInfo Render { get; set; }

        public bool Interpolate { get; set; }
        public BlendMode BlendMask { get; private set; }
        public OldFrame BaseFrame { get; private set; }
        public bool Colored { get; private set; }

        private Vector3 _globaltrans;
        private Vector3 _globalscale;
        private Matrix3 _globalrot;
        private Func<OldFrame, OldModelEntry?> _getmodelfunc;

        public void Setup(bool interpolate)
        {
            Interpolate = interpolate;
            BlendMask = BlendMode.None;
        }

        public bool RenderAnimFrame(Vector3 trans, VAO[] vaos, Entry? anim, double frame, Func<OldFrame, OldModelEntry?> get_model_func, Vector3 scale = default, Vector3 rot = default)
        {
            BaseFrame = null;

            List<OldFrame>? frames = null;

            if (anim is OldAnimationEntry svtx)
            {
                Colored = false;
                frames = svtx.Frames;
            }
            else if (anim is ColoredAnimationEntry cvtx)
            {
                Colored = true;
                frames = cvtx.Frames;
            }

            if (frames == null)
                return false;

            _globaltrans = trans;
            _globalscale = scale == Vector3.Zero ? Vector3.One : scale;
            _globalrot = MathExt.EulerToMat3_Z_XY(rot);
            _getmodelfunc = get_model_func!;

            OldFrame? frame2 = null;
            float interp = 0;
            int curframe = 0;
            if (frames.Count != 1)
            {
                curframe = (int)((long)Math.Floor(frame) % frames.Count);
                if (Interpolate)
                {
                    frame2 = frames[(int)((long)Math.Ceiling(frame) % frames.Count)];
                    interp = (float)frame.TruncatePart();
                }
            }
            var frame1 = frames[curframe]!;

            BlendMask = BlendMode.Solid;

            int startvert1 = vaos[0].CurVert;
            int startvert2 = vaos[1] == null ? 0 : vaos[1].CurVert;

            if (!RenderFrame(vaos, frame1, 0))
                return false;

            if (frame2 != null && frame2 != frame1 && frame1.Vertices.Count == frame2.Vertices.Count && RenderFrame(vaos, frame2, 1))
            {
                for (int i = 0; i < (vaos[0].CurVert - startvert1); ++i)
                {
                    MathExt.Lerp(ref vaos[0].Verts[i + startvert1].trans, vaos[1].Verts[i + startvert2].trans, interp);
                    MathExt.Lerp(ref vaos[0].Verts[i + startvert1].rgba, vaos[1].Verts[i + startvert2].rgba, interp);
                }
                // these won't be rendered, so who cares
                vaos[1].CurVert = startvert2;
            }

            BaseFrame = frame1;

            return true;
        }

        private void AddTPAGs(OldModelEntry model)
        {
            // collect tpag eids
            foreach (OldModelStruct str in model.Structs)
            {
                if (str is OldModelTexture tex)
                {
                    TPages.AddTexturePage(tex.EID);
                }
            }
        }

        private bool RenderFrame(VAO[] vaos, OldFrame frame, int buf)
        {
            var vao = vaos[buf];

            var model = _getmodelfunc(frame);
            if (model == null)
                return false;

            // setup textures
            AddTPAGs(model);

            // alloc buffers
            vao.TestReallocExtra(model.Polygons.Count * 3);

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

                    vao.Verts[cur_idx].tex = new VertexTexInfo(TPages[tex.EID], color: tex.ColorMode, blend: tex.BlendMode,
                                                                                clutx: tex.ClutX, cluty: tex.ClutY,
                                                                                face: Convert.ToInt32(tex.N));

                    vao.BlendModes |= VertexTexInfo.GetBlendMode(tex.BlendMode);
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

            return true;
        }

        private void RenderVertex(VAO vao, in OldFrameVertex vert, Vector3 trans, Vector3 scale)
        {
            int cur_vert_idx = vao.CurVert;
            vao.Verts[cur_vert_idx].trans = _globalrot * ((new Vector3(vert.X, vert.Y, vert.Z) + trans) * scale) * _globalscale + _globaltrans;
            if (Colored)
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
