using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    internal sealed class OldAnimationRenderer
    {
        public TexturePageList TPages { get; set; }
        public RenderInfo Render { get; set; }

        public bool Interpolate { get; set; }
        public bool Player { get; set; }
        public bool DisableLighting { get; set; }
        public BlendMode BlendMask { get; private set; }
        public OldFrame BaseFrame { get; private set; }
        public bool Colored { get; private set; }

        private Vector3 _trans;
        private Vector3 _scale;
        private Matrix3 _rot;
        private Matrix3 _matlight;
        private Matrix3 _matcolor;
        private Vector3 _color;
        private Vector3 _colorint;
        private Func<OldFrame, OldModelEntry?> _getmodelfunc;

        public void Setup(bool interpolate)
        {
            Interpolate = interpolate;
            BlendMask = BlendMode.None;
        }

        private Matrix3 ZoneMatLight;
        private Matrix3 ZoneMatColor;
        private Vector3 ZoneColor;
        private Vector3 ZoneColorInt;
        private Matrix3 PlayerZoneMatLight;
        private Matrix3 PlayerZoneMatColor;
        private Vector3 PlayerZoneColor;
        private Vector3 PlayerZoneColorInt;

        public void SetZoneMatrices(OldZoneEntry zone)
        {
            ZoneColor = new(zone.ColorBaseR, zone.ColorBaseG, zone.ColorBaseB);
            PlayerZoneColor = new(zone.PlayerColorBaseR, zone.PlayerColorBaseG, zone.PlayerColorBaseB);
            ZoneColorInt = new(zone.ColorIntR, zone.ColorIntG, zone.ColorIntB);
            PlayerZoneColorInt = new(zone.PlayerColorIntR, zone.PlayerColorIntG, zone.PlayerColorIntB);
            ZoneMatLight = new(zone.LightMatrixL11, zone.LightMatrixL12, zone.LightMatrixL13,
                               zone.LightMatrixL21, zone.LightMatrixL22, zone.LightMatrixL23,
                               zone.LightMatrixL31, zone.LightMatrixL32, zone.LightMatrixL33);
            PlayerZoneMatLight = new(zone.PlayerLightMatrixL11, zone.PlayerLightMatrixL12, zone.PlayerLightMatrixL13,
                                     zone.PlayerLightMatrixL21, zone.PlayerLightMatrixL22, zone.PlayerLightMatrixL23,
                                     zone.PlayerLightMatrixL31, zone.PlayerLightMatrixL32, zone.PlayerLightMatrixL33);
            ZoneMatColor = new(zone.ColorMatrixLR1, zone.ColorMatrixLR2, zone.ColorMatrixLR3,
                               zone.ColorMatrixLG1, zone.ColorMatrixLG2, zone.ColorMatrixLG3,
                               zone.ColorMatrixLB1, zone.ColorMatrixLB2, zone.ColorMatrixLB3);
            PlayerZoneMatColor = new(zone.PlayerColorMatrixLR1, zone.PlayerColorMatrixLR2, zone.PlayerColorMatrixLR3,
                                     zone.PlayerColorMatrixLG1, zone.PlayerColorMatrixLG2, zone.PlayerColorMatrixLG3,
                                     zone.PlayerColorMatrixLB1, zone.PlayerColorMatrixLB2, zone.PlayerColorMatrixLB3);
            // scale matrices and vectors out of fixed point
            ZoneColor /= 0x100;
            PlayerZoneColor /= 0x100;
            ZoneColorInt /= 0x100;
            PlayerZoneColorInt /= 0x100;
            ZoneMatLight.Row0 /= 0x1000;
            ZoneMatLight.Row1 /= 0x1000;
            ZoneMatLight.Row2 /= 0x1000;
            PlayerZoneMatLight.Row0 /= 0x1000;
            PlayerZoneMatLight.Row1 /= 0x1000;
            PlayerZoneMatLight.Row2 /= 0x1000;
            ZoneMatColor.Row0 /= 0x1000;
            ZoneMatColor.Row1 /= 0x1000;
            ZoneMatColor.Row2 /= 0x1000;
            PlayerZoneMatColor.Row0 /= 0x1000;
            PlayerZoneMatColor.Row1 /= 0x1000;
            PlayerZoneMatColor.Row2 /= 0x1000;
            ZoneMatColor.Transpose();
            PlayerZoneMatColor.Transpose();
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

            _trans = trans;
            _scale = scale == Vector3.Zero ? Vector3.One : scale;
            _rot = MathExt.EulerToMat3_Z_XY(rot);
            _matlight = Player ? PlayerZoneMatLight : ZoneMatLight;
            _matcolor = Player ? PlayerZoneMatColor : ZoneMatColor;
            _color = Player ? PlayerZoneColor : ZoneColor;
            _colorint = Player ? PlayerZoneColorInt : ZoneColorInt;
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
                OldModelStruct str = model.Structs[polygon.TexInfo];
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
                RenderVertex(vao, frame.Vertices[polygon.VertexC / 6], polygon.NoLight, trans, scale);
                RenderVertex(vao, frame.Vertices[polygon.VertexB / 6], polygon.NoLight, trans, scale);
                RenderVertex(vao, frame.Vertices[polygon.VertexA / 6], polygon.NoLight, trans, scale);
            }

            return true;
        }

        private void RenderVertex(VAO vao, in OldFrameVertex vert, bool nolight, Vector3 trans, Vector3 scale)
        {
            int cur_vert_idx = vao.CurVert;
            vao.Verts[cur_vert_idx].trans = _rot * ((new Vector3(vert.X, vert.Y, vert.Z) + trans) * scale) * _scale + _trans;
            Rgba old_rgba = vao.Verts[cur_vert_idx].rgba;
            if (Colored)
            {
                vao.Verts[cur_vert_idx].rgba = new Rgba((byte)(old_rgba.r * 2 * vert.Red),
                                                        (byte)(old_rgba.g * 2 * vert.Green),
                                                        (byte)(old_rgba.b * 2 * vert.Blue), 255);
            }
            else
            {
                Vector3 normal = new Vector3(-vert.NormalX, vert.NormalY, vert.NormalZ) * 128;
                if (!nolight && !DisableLighting)
                {
                    // todo rot
                    float sx = MathF.Sin(0);
                    float sy = MathF.Sin(0);
                    float sz = MathF.Sin(0);
                    float cx = MathF.Cos(0);
                    float cy = MathF.Cos(0);
                    float cz = MathF.Cos(0);
                    Matrix3 yxy = new Matrix3(cx * cz - sx * sy * sz, -cy * sz, sx * cz + cx * sy * sz,
                                              cx * sz + sx * sy * cz, +cy * cz, sx * sz - cx * sy * cz,
                                              -sx * cz, sy, cx * cy);
                    yxy.Transpose();
                    Vector3 lightdir = _matlight * yxy * normal;
                    lightdir.X = Math.Clamp(lightdir.X, 0, 0x7FFF);
                    lightdir.Y = Math.Clamp(lightdir.Y, 0, 0x7FFF);
                    lightdir.Z = Math.Clamp(lightdir.Z, 0, 0x7FFF);
                    Vector3 color16 = (_matcolor * lightdir + (_color * 0x100 * _colorint));
                    color16.X = Math.Clamp(color16.X, -0x8000, 0x7FFF);
                    color16.Y = Math.Clamp(color16.Y, -0x8000, 0x7FFF);
                    color16.Z = Math.Clamp(color16.Z, -0x8000, 0x7FFF);
                    color16 = new Vector3(old_rgba.r, old_rgba.g, old_rgba.b) * color16 / 0x100;
                    color16.X = Math.Clamp(color16.X, -0x8000, 0x7FFF);
                    color16.Y = Math.Clamp(color16.Y, -0x8000, 0x7FFF);
                    color16.Z = Math.Clamp(color16.Z, -0x8000, 0x7FFF);
                    color16 /= 16;
                    color16.X = Math.Clamp(color16.X, 0, 0xFF);
                    color16.Y = Math.Clamp(color16.Y, 0, 0xFF);
                    color16.Z = Math.Clamp(color16.Z, 0, 0xFF);
                    vao.Verts[cur_vert_idx].rgba = new Rgba((byte)color16.X, (byte)color16.Y, (byte)color16.Z, old_rgba.a);
                }
                // vao.Verts[cur_vert_idx].normal = Vertex.PackNormal(new Vector3(vert.NormalX, vert.NormalY, vert.NormalZ) / 128); // is this even necessary?
            }
            vao.CurVert++;
        }
    }
}
