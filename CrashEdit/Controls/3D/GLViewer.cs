using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    [Flags]
    public enum TextRenderFlags
    {
        Shadow = 1 << 0,
        Unscaled = 1 << 1,
        AutoScale = 1 << 2,
        Left = 1 << 3,
        Center = 1 << 4,
        Right = 1 << 5,
        Top = 1 << 6,
        Middle = 1 << 7,
        Bottom = 1 << 8,

        Default = Shadow | AutoScale | Left | Top
    }

    public abstract class GLViewer : GLControl
    {
        #region Static data.
        protected static readonly Vector3[] AxesPos = new Vector3[6] {
            new(-0.5f, 0, 0),
            new(+1.0f, 0, 0),
            new(0, -0.5f, 0),
            new(0, +1.0f, 0),
            new(0, 0, -0.5f),
            new(0, 0, +1.0f)
        };

        protected static readonly Vector2[] SpriteVerts = new Vector2[4] {
            new(-.5f, -.5f),
            new(-.5f, +.5f),
            new(+.5f, +.5f),
            new(+.5f, -.5f)
        };

        protected static readonly Vector3[] BoxVerts = new Vector3[8] {
            new(-1, -1, -1),
            new(-1, -1, +1),
            new(+1, -1, -1),
            new(+1, -1, +1),
            new(-1, +1, -1),
            new(-1, +1, +1),
            new(+1, +1, -1),
            new(+1, +1, +1),
        };

        // index into SpriteVerts for rendering sprites using triangles
        protected static readonly int[] SpriteTriIndices = new int[6] { 0, 1, 2, 2, 3, 0 };
        // index into BoxVerts for rendering a box using lines
        protected static readonly int[] BoxLineIndices = new int[24] {
            // sides
            0, 0+4,
            1, 1+4,
            2, 2+4,
            3, 3+4,

            // bottom
            0, 2,
            2, 3,
            3, 1,
            1, 0,

            // top
            0+4, 2+4,
            2+4, 3+4,
            3+4, 1+4,
            1+4, 0+4,
        };
        // index into BoxVerts for rendering a box using triangles
        protected static readonly int[] BoxTriIndices = new int[36]
        {
            // sides
            1, 5, 7, 7, 3, 1,
            3, 7, 6, 6, 2, 3,
            0, 4, 5, 5, 1, 0,
            2, 6, 4, 4, 0, 2,
            // bottom
            0, 1, 3, 3, 2, 0,
            // top
            1+4, 0+4, 2+4, 2+4, 3+4, 1+4,
        };
        #endregion

        #region Static fields for OpenGL renderer.
        private static readonly Dictionary<int, Vector3[]> SpherePosCache = new();
        private static readonly Dictionary<int, Vector3[]> GridPosCache = new();
        private static int SpherePosLastUploaded = -1;
        private static int GridPosLastUploaded = -1;
        protected static VAO vaoSphereLine;
        protected static VAO vaoGridLine;
        protected static ShaderContext shaderContext;
        protected static FontTable fontTable;
        protected static Library fontLib = new();

        protected static int texTpages;
        protected static int texSprites;
        protected static int texFont;
        protected static VAO vaoAxes;
        protected static VAO vaoTris;
        protected static VAO vaoLines;
        protected static VAO vaoLinesThick;
        protected static VAO vaoSprites;
        protected static VAO vaoText;
        protected static VAO vaoOctree;
        // note: there's multiple buffers because of blending
        protected const int ANIM_BUF_MAX = 2;
        protected static VAO[] vaoListCrash1 = new VAO[ANIM_BUF_MAX];
        // these shouldn't be used
        protected static VAO vaoDebugBoxTri;
        protected static VAO vaoDebugBoxLine;
        protected static VAO vaoDebugSprite;
        public static List<string> dbgContextDir = new();

        private static IGraphicsContext global_context;
        private static GLControl global_context_window;
        private static readonly GraphicsMode default_graphics_settings = new(32, 24, 8);
        #endregion

        protected readonly RenderInfo render;

        protected string con_debug;
        protected string con_help;
        // debug timers
        private double dbg_run_ms;

        #region Internal fields for input status and handling.
        private bool run = false;
        private bool loaded = false;
        private static readonly HashSet<Type> loaded_static_types = new();

        private readonly HashSet<Keys> keysdown = new();
        private readonly HashSet<Keys> keyspressed = new();
        private bool mouseright = false;
        private bool mouseleft = false;
        private int mousex = 0;
        private int mousey = 0;
        private readonly float movespeed = 10f;
        private readonly float rotspeed = 0.5f;
        private readonly float zoomspeed = 1f;
        private const float PerFrame = 1 / 60f;
        public const float DefaultZNear = 0.2f;
        public const float DefaultZFar = DefaultZNear * 0x4000;
        #endregion

        public bool KDown(Keys key) => keysdown.Contains(key);
        public bool KPress(Keys key) => keyspressed.Contains(key);
        public bool KDown(ControlsKeyboardInfo control) => KDown(control.Key);
        public bool KPress(ControlsKeyboardInfo control) => KPress(control.Key);
        private float GetMoveSpeed() => movespeed * 0.2f + movespeed * 0.8f * (render.Projection.Distance / (ProjectionInfo.MaxInitialDistance * 0.2f));

        #region Default controls.
        public static class KeyboardControls
        {
            public static readonly ControlsKeyboardInfo ResetCamera = new(Keys.R, Resources.ViewerControls_ResetCamera);
            public static readonly ControlsKeyboardInfo ToggleTextures = new(Keys.T, Resources.ViewerControls_ToggleTextures);
            public static readonly ControlsKeyboardInfo ToggleZoneOctree = new(Keys.C, Resources.ViewerControls_ToggleZoneOctree);
            public static readonly ControlsKeyboardInfo ToggleZoneOctreeOutline = new(Keys.V, Resources.ViewerControls_ToggleZoneOctreeOutline);
            public static readonly ControlsKeyboardInfo ToggleZoneOctreeNeighbors = new(Keys.Z, Resources.ViewerControls_ToggleZoneOctreeNeighbors);
            public static readonly ControlsKeyboardInfo OpenOctreeWindow = new(Keys.B, Resources.ViewerControls_OpenOctreeWindow);
            public static readonly ControlsKeyboardInfo ToggleZoneOctreeFlip = new(Keys.O, Resources.ViewerControls_ToggleZoneOctreeFlip);
            public static readonly ControlsKeyboardInfo ToggleTimeTrial = new(Keys.Y, Resources.ViewerControls_ToggleTimeTrial);
            public static readonly ControlsKeyboardInfo ToggleCollisionAnim = new(Keys.C, Resources.ViewerControls_ToggleCollisionAnim);
            public static readonly ControlsKeyboardInfo ToggleLerp = new(Keys.I, Resources.ViewerControls_ToggleLerp);
            public static readonly ControlsKeyboardInfo ToggleNormals = new(Keys.N, Resources.ViewerControls_ToggleNormals);
            public static readonly ControlsKeyboardInfo ChangeCullMode = new(Keys.U, Resources.ViewerControls_ChangeCullMode);
            public static readonly ControlsKeyboardInfo ToggleHelp = new(Keys.H, Resources.ViewerControls_ToggleHelp);
        }
        #endregion

        protected readonly NSF nsf;
        private bool showHelp = Settings.Default.ViewerShowHelp;

        #region Functions for generating static data for some helper renderers.
        protected static void MakeLineSphere(int resolution)
        {
            if (resolution < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), "Sphere resolution cannot be less than 0.");
            if (SpherePosLastUploaded == resolution) return;
            if (!SpherePosCache.ContainsKey(resolution))
            {
                int long_amt = resolution * 4;
                int lat_amt = resolution;
                int pt_nb = 1 + long_amt * (2 + 2 * lat_amt) + (1 + long_amt) * (1 + 2 * lat_amt);
                var pos = new Vector3[pt_nb];
                int i = 1;
                pos[0] = new Vector3(0, 0, 1);
                bool even = true;
                for (int ii = 0; ii < long_amt; ++ii)
                {
                    var rotmat = Matrix3.CreateRotationZ((float)ii / long_amt * MathHelper.TwoPi);
                    if (ii % 2 == 0)
                    {
                        for (int iii = 0, l_m = 2 + lat_amt * 2; iii < l_m; ++iii)
                        {
                            pos[i++] = pos[0] * Matrix3.CreateRotationX((float)(iii + 1) / l_m * MathHelper.Pi) * rotmat;
                        }
                        even = true;
                    }
                    else
                    {
                        for (int iii = 0, l_m = 2 + lat_amt * 2; iii < l_m; ++iii)
                        {
                            pos[i++] = pos[0] * Matrix3.CreateRotationX((float)(l_m - iii - 1) / l_m * MathHelper.Pi) * rotmat;
                        }
                        even = false;
                    }
                }
                for (int ii = 1, l_m = lat_amt * 2 + 2; ii < l_m; ++ii)
                {
                    Matrix3 rotmat;
                    if (!even)
                    {
                        rotmat = Matrix3.CreateRotationX((float)ii / l_m * MathHelper.Pi);
                    }
                    else
                    {
                        rotmat = Matrix3.CreateRotationX((float)(l_m - ii) / l_m * MathHelper.Pi);
                    }
                    for (int iii = 0; iii <= long_amt; ++iii)
                    {
                        pos[i++] = pos[0] * (!even ? Matrix3.CreateRotationX((float)ii / l_m * MathHelper.Pi) : Matrix3.CreateRotationX((float)(l_m - ii) / l_m * MathHelper.Pi))
                                          * Matrix3.CreateRotationZ((float)iii / long_amt * MathHelper.TwoPi);
                    }
                }
                SpherePosCache.Add(resolution, pos);
            }
            var verts = SpherePosCache[resolution];
            vaoGridLine.vert_count = 0;
            for (int i = 0; i < verts.Length; ++i)
            {
                vaoSphereLine.PushAttrib(trans: verts[i]);
            }
            SpherePosLastUploaded = resolution;
        }
        protected static void MakeLineGrid(int resolution)
        {
            if (resolution < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), "Grid resolution cannot be less than 0.");
            if (GridPosLastUploaded == resolution) return;

            if (!GridPosCache.ContainsKey(resolution))
            {
                var pos = new Vector3[4 * resolution * 2];
                var border = resolution * 1f - 0.5f;

                var pi = 0;
                for (int i = 0; i < resolution * 2; ++i)
                {
                    pos[pi++] = new Vector3(-border + i, 0, -border);
                    pos[pi++] = new Vector3(-border + i, 0, +border);
                    pos[pi++] = new Vector3(-border, 0, -border + i);
                    pos[pi++] = new Vector3(+border, 0, -border + i);
                }

                GridPosCache.Add(resolution, pos);
            }

            var verts = GridPosCache[resolution];
            vaoGridLine.vert_count = 0;
            for (int i = 0; i < verts.Length; ++i)
            {
                vaoGridLine.PushAttrib(trans: verts[i]);
            }
            GridPosLastUploaded = resolution;
        }
        #endregion

        public GLViewer(NSF nsf = null) : base(default_graphics_settings, 4, 3, GraphicsContextFlags.Debug | GraphicsContextFlags.ForwardCompatible)
        {
            if (global_context == null)
            {
                global_context = Context;
                global_context_window = this;
            }
            render = new RenderInfo(this);
            this.nsf = nsf;
        }

        protected new void SwapBuffers()
        {
            global_context_window.SwapBuffers();
        }

        protected new void MakeCurrent()
        {
            global_context.MakeCurrent(WindowInfo);
        }

        protected abstract IEnumerable<IPosition> CorePositions { get; }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            lock (render.mLock)
            {
                var olddist = render.Projection.Distance;
                float delta = (float)e.Delta / SystemInformation.MouseWheelScrollDelta * zoomspeed;
                delta *= 0.3f + 1.7f * (render.Projection.Distance / ProjectionInfo.MaxInitialDistance);
                render.Projection.Distance = Math.Max(ProjectionInfo.MinDistance, Math.Min(render.Projection.Distance - delta, ProjectionInfo.MaxDistance));
                // render.Projection.Trans -= (Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot)) * new Vector4(0, 0, render.Distance - olddist, 1)).Xyz;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.Button)
            {
                case MouseButtons.Left: mouseleft = false; break;
                case MouseButtons.Right: mouseright = false; break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            lock (render.mLock)
            {
                if (mouseleft)
                {
                    float rotx = render.Projection.Rot.X;
                    float rotz = render.Projection.Rot.Y;
                    rotz += MathHelper.DegreesToRadians(e.X - mousex) * rotspeed;
                    rotx += MathHelper.DegreesToRadians(e.Y - mousey) * rotspeed;
                    if (rotx > RenderInfo.MaxRot)
                        rotx = RenderInfo.MaxRot;
                    if (rotx < RenderInfo.MinRot)
                        rotx = RenderInfo.MinRot;
                    render.Projection.Rot.X = rotx;
                    render.Projection.Rot.Y = rotz;
                }
                else if (mouseright)
                {
                }
                mousex = e.X;
                mousey = e.Y;
            }
        }

        // for each instance
        protected virtual void GLLoad()
        {
            void run_load_static(Type t)
            {
                if (!loaded_static_types.Contains(t))
                {
                    if (t.BaseType != null && t.BaseType != typeof(object))
                    {
                        run_load_static(t.BaseType);
                    }
                    Console.WriteLine($"GLLoadStatic {t.Name}");
                    var func = t.GetMethod("GLLoadStatic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                    func?.Invoke(this, new object[] { });
                    loaded_static_types.Add(t);
                }
            }
            run_load_static(Program.TopLevelGLViewer.GetType());
            // will run this on parent types first
            run_load_static(GetType());

            ResetCamera();

            // enable logic
            run = true;
        }

        public void BeginRunLogic()
        {
            if (!run) return;
            RunLogic();
            keyspressed.Clear();
        }

        public static string OnOffName(bool enable)
        {
            return enable ? "on" : "off";
        }

        public static string CullModeName(int mode)
        {
            switch (mode)
            {
                case 0: return "frontface culling";
                case 1: return "backface culling";
                case 2:
                default: return "no culling";
            }
        }

        protected virtual void PrintDebug()
        {
            con_debug += $"Zoom: {render.Projection.Distance}\nMove Speed: {GetMoveSpeed()}\n";
            con_debug += string.Format("Render time: {0:F2}ms\nTotal time: {1:F2}ms\n", render.DebugRenderMs, dbg_run_ms);
        }

        protected virtual void PrintHelp()
        {
            con_help += Resources.ViewerControls_Move + '\n';
            con_help += Resources.ViewerControls_MoveAligned + '\n';
            con_help += Resources.ViewerControls_AimAndZoom + '\n';
            con_help += KeyboardControls.ToggleHelp.Print(OnOffName(showHelp));
            con_help += KeyboardControls.ResetCamera.Print();
            con_help += KeyboardControls.ToggleTextures.Print(OnOffName(render.EnableTexture));
        }

        protected virtual void RunLogic()
        {
            var d = GetMoveSpeed() * PerFrame;
            if (KDown(Keys.ControlKey))
            {
                if (KDown(Keys.W)) render.Projection.Trans.Z += d;
                if (KDown(Keys.S)) render.Projection.Trans.Z -= d;
                if (KDown(Keys.A)) render.Projection.Trans.X += d;
                if (KDown(Keys.D)) render.Projection.Trans.X -= d;
                if (KDown(Keys.E)) render.Projection.Trans.Y += d;
                if (KDown(Keys.Q)) render.Projection.Trans.Y -= d;
            }
            else
            {
                var r = Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot));
                if (KDown(Keys.W)) render.Projection.Trans += (r * new Vector4(0, 0, d, 1)).Xyz;
                if (KDown(Keys.S)) render.Projection.Trans -= (r * new Vector4(0, 0, d, 1)).Xyz;
                if (KDown(Keys.A)) render.Projection.Trans += (r * new Vector4(d, 0, 0, 1)).Xyz;
                if (KDown(Keys.D)) render.Projection.Trans -= (r * new Vector4(d, 0, 0, 1)).Xyz;
                if (KDown(Keys.E)) render.Projection.Trans += (r * new Vector4(0, d, 0, 1)).Xyz;
                if (KDown(Keys.Q)) render.Projection.Trans -= (r * new Vector4(0, d, 0, 1)).Xyz;
            }
            if (KPress(KeyboardControls.ResetCamera))
            {
                render.Reset();
                ResetCamera();
            }
            if (KPress(KeyboardControls.ToggleTextures)) render.EnableTexture = !render.EnableTexture;
            if (KPress(KeyboardControls.ToggleHelp)) showHelp = !showHelp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            dbgContextDir.Clear();
            if (!loaded)
            {
                dbgContextDir.Add("load");
                MakeCurrent();
                GLLoad();
                loaded = true;
                dbgContextDir.RemoveLast();
            }
            else if (run)
            {
                Stopwatch watchRun = Stopwatch.StartNew();

                dbgContextDir.Add("render");

                render.DebugRenderMs = 0;

                MakeCurrent();

                // mutex lock to prevent races with the renderinfo timer and using the renderinfo itself
                lock (render.mLock)
                {
                    dbgContextDir.Add("setup");

                    // update font
                    if (fontTable.Size != Settings.Default.FontSize || fontTable.FileName != Settings.Default.FontName)
                    {
                        fontTable.LoadFontAndLoadToGL(texFont, fontLib, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), Settings.Default.FontName), Settings.Default.FontSize);
                    }

                    // set up viewport clip
                    GL.Viewport(0, 0, Width, Height);
                    render.Projection.Width = Width;
                    render.Projection.Height = Height;

                    // Clear buffers
                    GL.DepthMask(true);
                    var col = Settings.Default.ClearColorRGB;
                    if ((col & 0xffffff) == 0) col = 0;
                    GL.ClearColor(Color.FromArgb(col));
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    // set up view matrices (45deg FOV)
                    render.Projection.Perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), render.Projection.Aspect, DefaultZNear, DefaultZFar);
                    var rot_mat = Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot));
                    render.Projection.Forward = -(rot_mat * new Vector4(0, 0, 1, 1)).Xyz;
                    render.Projection.View = Matrix4.CreateTranslation(render.Projection.RealTrans) * rot_mat;
                    // con_debug += $"trans: {-render.Projection.Trans} -> real_trans: {-render.Projection.RealTrans}\n";
                    
                    dbgContextDir.RemoveLast();
                    dbgContextDir.Add(GetType().ToString());

                    // render
                    dbgContextDir.Add("render");
                    Render();
                    dbgContextDir.RemoveLast();

                    // post render
                    dbgContextDir.Add("post-render");
                    PostRender();
                    dbgContextDir.RemoveLast();

                    PrintHelp();
                    PrintDebug();
                    if (showHelp)
                        AddText(con_help, Width, Height - 8, (Rgba)Color4.White, size: 0.85f, flags: TextRenderFlags.Right | TextRenderFlags.Bottom | TextRenderFlags.Default);
                    if (Settings.Default.Font2DEnable)
                        AddText(con_debug, 0, 0, (Rgba)Color4.White, size: 0.85f);
                    vaoText.UserScale = new Vector3(Width, Height, 1);
                    vaoText.RenderAndDiscard(render);
                    con_debug = string.Empty;
                    con_help = string.Empty;

                    dbgContextDir.RemoveLast();
                }

                dbg_run_ms = watchRun.ElapsedMillisecondsFull();

                // swap the front/back buffers so what we just rendered to the back buffer is displayed in the window.
                dbgContextDir.RemoveLast();
                dbgContextDir.Add("swap-buffers");
                SwapBuffers();
                dbgContextDir.RemoveLast();
            }
            base.OnPaint(e);
        }

        protected virtual void Render()
        {
            SetBlendMode(BlendMode.Solid);

            if (Settings.Default.DisplayAnimGrid)
            {
                RenderAxes(new Vector3(0));

                MakeLineGrid(Settings.Default.AnimGridLen);
                vaoGridLine.Render(render);
            }
        }

        protected void PostRender()
        {
            SetBlendMode(BlendMode.Solid);

            vaoLines.RenderAndDiscard(render);
            vaoLinesThick.RenderAndDiscard(render);
            vaoTris.RenderAndDiscard(render);
            vaoSprites.RenderAndDiscard(render);
        }

        private void RenderAxes(Vector3 pos)
        {
            vaoAxes.UserTrans = pos;
            vaoAxes.Render(render);
        }

        protected void DebugRenderBox(Vector3 pos, Vector3 size, Color4 col)
        {
            vaoDebugBoxTri.UserTrans = pos;
            vaoDebugBoxTri.UserScale = size;
            vaoDebugBoxTri.UserColor1 = col;
            vaoDebugBoxTri.Render(render);
        }

        protected void DebugRenderBoxLine(Vector3 pos, Vector3 size, Color4 col)
        {
            vaoDebugBoxLine.UserTrans = pos;
            vaoDebugBoxLine.UserScale = size;
            vaoDebugBoxLine.UserColor1 = col;
            vaoDebugBoxLine.Render(render);
        }

        protected void DebugRenderBoxLineFilled(Vector3 pos, Vector3 size, Color4 col_line, Color4 col_fill)
        {
            DebugRenderBoxLine(pos, size, col_line);
            DebugRenderBox(pos, size, col_fill);
        }

        public Vector2 AddText3D(string text, Vector3 pos, Rgba col, float size = 1, TextRenderFlags flags = TextRenderFlags.Default, float ofs_x = 0, float ofs_y = 0)
        {
            var screen_pos = new Vector4(pos, 1) * render.Projection.PVM;
            screen_pos /= screen_pos.W;
            if (screen_pos.Z >= 1 || screen_pos.Z <= -1)
                return new Vector2(0);
            screen_pos.Y = -screen_pos.Y;
            if ((flags & TextRenderFlags.AutoScale) != 0)
            {
                flags &= ~TextRenderFlags.Unscaled;
                if (!Settings.Default.Font3DAutoscale)
                {
                    flags |= TextRenderFlags.Unscaled;
                }
            }
            if ((flags & TextRenderFlags.Unscaled) == 0)
            {
                var text_dist_vec = render.Projection.RealTrans + pos;
                var cam_to_text_factor = 1 / Vector3.Dot(text_dist_vec, render.Projection.Forward);
                // con_debug += $"text_dist: {text_dist_vec.Length} dot: {cam_to_text_factor}\n";
                size *= cam_to_text_factor * render.Projection.Height / 50;
            }
            return AddText(text, (screen_pos.Xy + new Vector2(1)) * new Vector2(Width, Height) / 2 + new Vector2(ofs_x, ofs_y), col, size, flags);
        }

        public Vector2 AddText(string text, float x, float y, Rgba col, float size = 1, TextRenderFlags flags = TextRenderFlags.Default) => AddText(text, new Vector2(x, y), col, new Vector2(size), flags);
        public Vector2 AddText(string text, Vector2 ofs, Rgba col, float size = 1, TextRenderFlags flags = TextRenderFlags.Default) => AddText(text, ofs, col, new Vector2(size), flags);
        public Vector2 AddText(string text, Vector2 ofs, Rgba col, Vector2 size, TextRenderFlags flags = TextRenderFlags.Default)
        {
            if (fontTable == null || text.Length == 0)
                return new Vector2(0);

            var face = fontTable.Face;
            var text_size = GetTextSize(text, size, flags);
            string[] text_lines = null;
            float[] text_line_sizes = null;
            int line = 0;

            var start_ofs = ofs;
            if ((flags & TextRenderFlags.Middle) != 0)
            {
                start_ofs.Y -= text_size.Y / 2;
            }
            else if ((flags & TextRenderFlags.Bottom) != 0)
            {
                start_ofs.Y -= text_size.Y;
            }
            if ((flags & (TextRenderFlags.Center | TextRenderFlags.Right)) != 0)
            {
                text_lines = text.Split('\n');
                text_line_sizes = new float[text_lines.Length];
                for (int i = 0; i < text_lines.Length; ++i)
                {
                    text_line_sizes[i] = GetTextSize(text_lines[i], new Vector2(1), flags).X * size.X;
                }
                start_ofs.X -= text_line_sizes[line++] / (((flags & TextRenderFlags.Center) != 0) ? 2 : 1);
            }

            // correct size
            size *= 16;
            size.X /= fontTable.Width;
            size.Y /= fontTable.Height;

            var cur_ofs = start_ofs;
            cur_ofs.Y += fontTable.LineHeight * size.Y;
            int start_idx = vaoText.vert_count;
            for (int i = 0; i < text.Length; ++i)
            {
                var c = text[i];
                if (c == '\n')
                {
                    if ((flags & TextRenderFlags.Center) != 0)
                    {
                        cur_ofs.X = ofs.X - text_line_sizes[line++] / 2;
                    }
                    else if ((flags & TextRenderFlags.Right) != 0)
                    {
                        cur_ofs.X = ofs.X - text_line_sizes[line++];
                    }
                    else
                    {
                        cur_ofs.X = start_ofs.X;
                    }
                    cur_ofs.Y += fontTable.LineHeight * size.Y;
                    continue;
                }
                if (!fontTable.ContainsKey(c))
                {
                    cur_ofs.X += fontTable.Width * size.X;
                    continue;
                }

                var glyph = fontTable[c];

                float kBearingX = (float)glyph.BearingX * size.X;
                float kBearingY = (float)glyph.BearingY * size.Y;
                float kAdvanceX = (float)glyph.AdvanceX * size.X;
                var kSize = new Vector2(glyph.Width, glyph.Height) * size;

                int idx = vaoText.vert_count;
                var char_ofs = cur_ofs + new Vector2(kBearingX, -kBearingY);
                vaoText.PushAttrib(trans: new Vector3(char_ofs + kSize * new Vector2(0, 0)), st: new Vector2(glyph.Left, glyph.Top), rgba: col);
                vaoText.PushAttrib(trans: new Vector3(char_ofs + kSize * new Vector2(1, 0)), st: new Vector2(glyph.Right, glyph.Top), rgba: col);
                vaoText.PushAttrib(trans: new Vector3(char_ofs + kSize * new Vector2(1, 1)), st: new Vector2(glyph.Right, glyph.Bottom), rgba: col);
                vaoText.CopyAttrib(idx + 2);
                vaoText.PushAttrib(trans: new Vector3(char_ofs + kSize * new Vector2(0, 1)), st: new Vector2(glyph.Left, glyph.Bottom), rgba: col);
                vaoText.CopyAttrib(idx + 0);

                if (face.HasKerning && i < text.Length - 1)
                {
                    char cNext = text[i + 1];
                    float kern = (float)face.GetKerning(glyph.GlyphID, face.GetCharIndex(cNext), KerningMode.Default).X * size.X;
                    if (kern > kAdvanceX * 5 || kern < -(kAdvanceX * 5))
                        kern = 0;
                    cur_ofs.X += kern;
                }

                cur_ofs.X += kAdvanceX;
            }
            int end_idx = vaoText.vert_count;
            if ((flags & TextRenderFlags.Shadow) != 0)
            {
                for (int i = 0; i < end_idx - start_idx; ++i)
                {
                    vaoText.CopyAttrib(start_idx + i);
                    vaoText.Verts[start_idx + i].trans += new Vector3(new Vector2(2, 2) * size);
                    vaoText.Verts[start_idx + i].rgba = new Rgba(0, 0, 0, col.a);
                }
            }

            return text_size;
        }

        public Vector2 GetTextSize(string text, Vector2 size, TextRenderFlags flags = TextRenderFlags.Default)
        {
            if (fontTable == null || text.Length == 0)
                return new Vector2(0);

            var face = fontTable.Face;
            size *= 16;
            size.X /= fontTable.Width;
            size.Y /= fontTable.Height;

            float text_w = 0;
            var start_ofs = new Vector2(0);
            var cur_ofs = new Vector2(0, fontTable.LineHeight * size.Y);
            for (int i = 0; i < text.Length; ++i)
            {
                var c = text[i];
                if (c == '\n')
                {
                    text_w = Math.Max(text_w, cur_ofs.X - start_ofs.X);

                    cur_ofs.X = start_ofs.X;
                    if (i < text.Length - 1)
                        cur_ofs.Y += fontTable.LineHeight * size.Y;
                    continue;
                }
                if (!fontTable.ContainsKey(c))
                {
                    cur_ofs.X += fontTable.Width * size.X;
                    continue;
                }

                var glyph = fontTable[c];

                float kAdvanceX = (float)glyph.AdvanceX * size.X;

                if (face.HasKerning && i < text.Length - 1)
                {
                    char cNext = text[i + 1];
                    float kern = (float)face.GetKerning(glyph.GlyphID, face.GetCharIndex(cNext), KerningMode.Default).X * size.X;
                    if (kern > kAdvanceX * 5 || kern < -(kAdvanceX * 5))
                        kern = 0;
                    cur_ofs.X += kern;
                }

                cur_ofs.X += kAdvanceX;
            }

            return new Vector2(Math.Max(text_w, cur_ofs.X - start_ofs.X), cur_ofs.Y - start_ofs.Y);
        }

        public void AddBox(Vector3 ofs, Vector3 sz, Rgba col, bool outline)
        {
            if (outline)
            {
                for (int i = 0; i < BoxLineIndices.Length; ++i)
                {
                    int v_idx = BoxLineIndices[i];
                    vaoLines.PushAttrib(trans: (BoxVerts[v_idx] + new Vector3(1)) / 2 * sz + ofs, rgba: col);
                }
            }
            else
            {
                for (int i = 0; i < BoxTriIndices.Length; ++i)
                {
                    int v_idx = BoxTriIndices[i];
                    vaoTris.PushAttrib(trans: (BoxVerts[v_idx] + new Vector3(1)) / 2 * sz + ofs, rgba: col, st: new Vector2(-1));
                }
            }
        }

        public void AddBox(Vector3 ofs, Vector3 sz, Rgba[] colors, bool outline)
        {
            if (outline)
            {
                for (int i = 0; i < BoxLineIndices.Length; ++i)
                {
                    int v_idx = BoxLineIndices[i];
                    vaoLines.PushAttrib(trans: (BoxVerts[v_idx] + new Vector3(1)) / 2 * sz + ofs, rgba: colors.Length == BoxLineIndices.Length ? colors[i] : colors[v_idx]);
                }
            }
            else
            {
                for (int i = 0; i < BoxTriIndices.Length; ++i)
                {
                    int v_idx = BoxTriIndices[i];
                    vaoTris.PushAttrib(trans: (BoxVerts[v_idx] + new Vector3(1)) / 2 * sz + ofs, rgba: colors.Length == BoxTriIndices.Length ? colors[i] : colors[v_idx], st: new Vector2(-1));
                }
            }
        }

        public void AddBoxAB(Vector3 a, Vector3 b, Rgba col, bool outline) => AddBox(a, b - a, col, outline);
        public void AddBoxAB(Vector3 a, Vector3 b, Rgba[] col, bool outline) => AddBox(a, b - a, col, outline);

        protected void DebugRenderSprite(Vector3 trans, Vector2 size, Color4 col, Bitmap texture)
        {
            var texRect = OldResources.TexMap[texture];
            //Console.WriteLine($"TEXTURE: {texRect.Left},{texRect.Top}/{texRect.Right},{texRect.Bottom}");
            Span<Vector2> uvs = stackalloc Vector2[4] {
                new Vector2(texRect.Left, texRect.Bottom),
                new Vector2(texRect.Left, texRect.Top),
                new Vector2(texRect.Right, texRect.Top),
                // new Vector2(texRect.Right, texRect.Top),
                new Vector2(texRect.Right, texRect.Bottom),
                // new Vector2(texRect.Left, texRect.Bottom)
            };
            vaoDebugSprite.DiscardVerts();
            for (int i = 0; i < SpriteVerts.Length; ++i)
            {
                vaoDebugSprite.PushAttrib(trans: new Vector3(SpriteVerts[i]), st: uvs[i]);
            }
            vaoDebugSprite.UserTrans = trans;
            vaoDebugSprite.UserScale = new Vector3(size);
            vaoDebugSprite.UserColor1 = col;
            vaoDebugSprite.Render(render);
        }

        protected void AddSprite(Vector3 trans, Vector2 size, Rgba col, Bitmap texture)
        {
            var texRect = OldResources.TexMap[texture];
            //Console.WriteLine($"TEXTURE: {texRect.Left},{texRect.Top}/{texRect.Right},{texRect.Bottom}");
            Span<Vector2> uvs = stackalloc Vector2[6] {
                 new Vector2(texRect.Left, texRect.Bottom),
                 new Vector2(texRect.Left, texRect.Top),
                 new Vector2(texRect.Right, texRect.Top),
                 new Vector2(texRect.Right, texRect.Top),
                 new Vector2(texRect.Right, texRect.Bottom),
                 new Vector2(texRect.Left, texRect.Bottom)
            };
            for (int i = 0; i < SpriteTriIndices.Length; ++i)
            {
                vaoSprites.PushAttrib(trans: trans, rgba: col, st: uvs[i], misc: new Vector4(SpriteVerts[SpriteTriIndices[i]] * size));
            }
        }

        public void AddOctreeX(Vector3 trans, Vector3 trans_size, int node, Vector3w nodes_size)
        {
            vaoOctree.PushAttrib(trans: trans, misc: new Vector3w(node, 0, 0).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, trans_size.Y, 0), misc: new Vector3w(node, nodes_size.Y, 0).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, 0, trans_size.Z), misc: new Vector3w(node, 0, nodes_size.Z).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, 0, trans_size.Z), misc: new Vector3w(node, 0, nodes_size.Z).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, trans_size.Y, 0), misc: new Vector3w(node, nodes_size.Y, 0).ToVec4());
            vaoOctree.PushAttrib(trans: trans + trans_size, misc: new Vector3w(node, nodes_size.Y, nodes_size.Z).ToVec4());
        }

        public void AddOctreeY(Vector3 trans, Vector3 trans_size, int node, Vector3w nodes_size)
        {
            vaoOctree.PushAttrib(trans: trans, misc: new Vector3w(0, node, 0).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(trans_size.X, 0, 0), misc: new Vector3w(nodes_size.X, node, 0).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, 0, trans_size.Z), misc: new Vector3w(0, node, nodes_size.Z).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, 0, trans_size.Z), misc: new Vector3w(0, node, nodes_size.Z).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(trans_size.X, 0, 0), misc: new Vector3w(nodes_size.X, node, 0).ToVec4());
            vaoOctree.PushAttrib(trans: trans + trans_size, misc: new Vector3w(nodes_size.X, node, nodes_size.Z).ToVec4());
        }

        public void AddOctreeZ(Vector3 trans, Vector3 trans_size, int node, Vector3w nodes_size)
        {
            vaoOctree.PushAttrib(trans: trans, misc: new Vector3w(0, 0, node).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(trans_size.X, 0, 0), misc: new Vector3w(nodes_size.X, 0, node).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, trans_size.Y, 0), misc: new Vector3w(0, nodes_size.Y, node).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(0, trans_size.Y, 0), misc: new Vector3w(0, nodes_size.Y, node).ToVec4());
            vaoOctree.PushAttrib(trans: trans + new Vector3(trans_size.X, 0, 0), misc: new Vector3w(nodes_size.X, 0, node).ToVec4());
            vaoOctree.PushAttrib(trans: trans + trans_size, misc: new Vector3w(nodes_size.X, nodes_size.Y, node).ToVec4());
        }

        public void AddOctreeLine(Vector3 trans, Vector3 line_size, Vector3w node, Vector3w node_size)
        {
            vaoOctree.PushAttrib(trans: trans, misc: node.ToVec4());
            vaoOctree.PushAttrib(trans: trans + line_size, misc: (node + node_size).ToVec4());
        }

        public void OctreeSetNodeShadeMax(float amt)
        {
            vaoOctree.UserFloat = amt;
        }

        public void OctreeSetNodeAlpha(float alpha)
        {
            vaoOctree.UserFloat2 = alpha;
        }

        public void OctreeSetOutline(bool outline)
        {
            if (outline)
                vaoOctree.Primitive = PrimitiveType.Lines;
            else
                vaoOctree.Primitive = PrimitiveType.Triangles;
        }

        public void RenderOctree()
        {
            SetBlendMode(BlendMode.Solid);
            vaoOctree.RenderAndDiscard(render);
        }

        protected float RelativeX(float pos) => Width * pos;
        protected float RelativeY(float pos) => Height * pos;

        public void ResetCamera()
        {
            float minx = float.MaxValue;
            float miny = float.MaxValue;
            float minz = float.MaxValue;
            float maxx = float.MinValue;
            float maxy = float.MinValue;
            float maxz = float.MinValue;
            foreach (IPosition position in CorePositions)
            {
                minx = (float)Math.Min(minx, position.X);
                miny = (float)Math.Min(miny, position.Y);
                minz = (float)Math.Min(minz, position.Z);
                maxx = (float)Math.Max(maxx, position.X);
                maxy = (float)Math.Max(maxy, position.Y);
                maxz = (float)Math.Max(maxz, position.Z);
            }
            float midx = (maxx + minx) / 2;
            float midy = (maxy + miny) / 2;
            float midz = (maxz + minz) / 2;
            render.Projection.Distance = Math.Min(ProjectionInfo.MaxInitialDistance, Math.Max(render.Projection.Distance, (float)(Math.Sqrt(Math.Pow(maxx - minx, 2) + Math.Pow(maxy - miny, 2) + Math.Pow(maxz - minz, 2)) * 1.2)));
            //render.Projection.Distance += 0;
            render.Projection.Trans.X = -midx;
            render.Projection.Trans.Y = -midy;
            render.Projection.Trans.Z = -midz;
            render.Projection.Rot.Y = 0;
            render.Projection.Rot.X = MathHelper.DegreesToRadians(15);
            Invalidate();
        }

        [Flags]
        public enum BlendMode { None = 0, Trans = 1, Additive = 2, Subtractive = 4, Solid = 8, All = Trans | Additive | Subtractive | Solid }
        public static int BlendModeIndex(BlendMode blend) => MathExt.Log2((int)blend);

        public static void SetBlendMode(BlendMode bmode)
        {
            switch (bmode)
            {
                case BlendMode.Solid:
                    GL.DepthMask(true);
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;
                case BlendMode.Trans:
                    GL.DepthMask(false);
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;
                case BlendMode.Additive:
                    GL.DepthMask(false);
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    break;
                case BlendMode.Subtractive:
                    GL.DepthMask(false);
                    GL.BlendEquation(BlendEquationMode.FuncReverseSubtract);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    break;
            }
        }

        protected void SetupTPAGs(Dictionary<int, short> tex_eids)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texTpages);

            // fill texture
            GL.GetTextureLevelParameter(texTpages, 0, GetTextureParameter.TextureHeight, out int tpage_h);
            if (tpage_h < tex_eids.Count * 128)
            {
                // realloc if not enough texture mem
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8ui, 512, tex_eids.Count * 128, 0, OpenTK.Graphics.OpenGL4.PixelFormat.RedInteger, PixelType.UnsignedByte, IntPtr.Zero);
            }
            foreach (var kvp in tex_eids)
            {
                var tpag = nsf.GetEntry<TextureChunk>(kvp.Key);
                if (tpag != null)
                {
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, kvp.Value * 128, 512, 128, OpenTK.Graphics.OpenGL4.PixelFormat.RedInteger, PixelType.UnsignedByte, tpag.Data);
                }
            }
        }

        protected bool ProcessTextureInfoC2(int in_tex_id, bool animated, IList<ModelTexture> textures, IList<ModelExtendedTexture> animated_textures, out ModelTexture tex)
        {
            if (in_tex_id != 0 || animated)
            {
                int tex_id = in_tex_id - 1;
                if (animated)
                {
                    if (++tex_id >= animated_textures.Count)
                    {
                        tex = default;
                        return false;
                    }
                    var anim = animated_textures[tex_id];
                    // check if it's an untextured polygon
                    if (anim.Offset != 0)
                    {
                        tex_id = anim.Offset - 1;
                        if (anim.IsLOD)
                        {
                            tex_id += anim.LOD0; // we only render closest LOD for now
                        }
                        else
                        {
                            tex_id += (int)((render.RealCurrentFrame / 2 / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                            if (anim.Leap)
                            {
                                anim = animated_textures[++tex_id];
                                tex_id = anim.Offset - 1 + anim.LOD0;
                            }
                        }
                        if (tex_id >= textures.Count)
                        {
                            tex = default;
                            return false;
                        }
                        tex = textures[tex_id];
                    }
                    else
                    {
                        tex = default;
                    }
                }
                else
                {
                    if (tex_id >= textures.Count)
                    {
                        tex = default;
                        return false;
                    }
                    tex = textures[tex_id];
                }
                return true;
            }
            tex = default;
            return true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (KDown(e.KeyCode)) return;
            keysdown.Add(e.KeyCode);
            keyspressed.Add(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            keysdown.Remove(e.KeyCode);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            keysdown.Clear(); // release all keys on unfocus
            mouseleft = false;
            mouseright = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (e.Button)
            {
                case MouseButtons.Left: mouseleft = true; /*mousex = e.X; mousey = e.Y;*/ break;
                case MouseButtons.Right: mouseright = true; break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            render.Dispose();

            if (global_context_window == this)
            {
                global_context_window = null;
                global_context = null;
            }
            Context?.Dispose();

            base.Dispose(disposing);
        }
    }
}
