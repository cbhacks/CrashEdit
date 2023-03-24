using Crash;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class GLViewer : GLControl
    {
        protected static readonly Vector3[] SpriteVerts = new Vector3[4] {
            new(-.5f, -.5f, 0),
            new(-.5f, +.5f, 0),
            new(+.5f, +.5f, 0),
            //new(+.5f, +.5f, 0),
            new(+.5f, -.5f, 0)
            //new(-.5f, -.5f)
        };
        protected static readonly Vector3[] AxesPos = new Vector3[6] {
            new(-.5f, 0, 0),
            new(1, 0, 0),
            new(0, -.5f, 0),
            new(0, 1, 0),
            new(0, 0, -.5f),
            new(0, 0, 1)
        };
        protected static readonly Vector3[] BoxLineVerts = new Vector3[24] {
            // sides
            new(-1, -1, -1),
            new(-1, +1, -1),

            new(+1, -1, -1),
            new(+1, +1, -1),

            new(+1, -1, +1),
            new(+1, +1, +1),

            new(-1, -1, +1),
            new(-1, +1, +1),

            // bottom
            new(-1, -1, -1),
            new(+1, -1, -1),

            new(+1, -1, -1),
            new(+1, -1, +1),

            new(+1, -1, +1),
            new(-1, -1, +1),

            new(-1, -1, +1),
            new(-1, -1, -1),

            // top
            new(-1, +1, -1),
            new(+1, +1, -1),

            new(+1, +1, -1),
            new(+1, +1, +1),

            new(+1, +1, +1),
            new(-1, +1, +1),

            new(-1, +1, +1),
            new(-1, +1, -1)
        };
        protected static readonly Vector3[] BoxTriVerts = new Vector3[36] {
            // sides
            new(-1, -1, -1),
            new(-1, +1, -1),
            new(+1, +1, -1),
            new(+1, +1, -1),
            new(+1, -1, -1),
            new(-1, -1, -1),

            new(-1, -1, +1),
            new(-1, +1, +1),
            new(+1, +1, +1),
            new(+1, +1, +1),
            new(+1, -1, +1),
            new(-1, -1, +1),

            new(-1, -1, -1),
            new(-1, +1, -1),
            new(-1, +1, +1),
            new(-1, +1, +1),
            new(-1, -1, +1),
            new(-1, -1, -1),

            new(+1, -1, -1),
            new(+1, +1, -1),
            new(+1, +1, +1),
            new(+1, +1, +1),
            new(+1, -1, +1),
            new(+1, -1, -1),

            // bottom
            new(-1, -1, -1),
            new(+1, -1, -1),
            new(+1, -1, +1),
            new(+1, -1, +1),
            new(-1, -1, +1),
            new(-1, -1, -1),

            // top
            new(-1, +1, -1),
            new(+1, +1, -1),
            new(+1, +1, +1),
            new(+1, +1, +1),
            new(-1, +1, +1),
            new(-1, +1, -1)
        };
        private static Dictionary<int, Vector3[]> SpherePosCache = new();
        private static Dictionary<int, Vector3[]> GridPosCache = new();
        private static int SpherePosLastUploaded = -1;
        private static int GridPosLastUploaded = -1;
        protected static VAO vaoSphereLine;
        protected static VAO vaoGridLine;
        protected static ShaderContext shaderContext;

        protected readonly RenderInfo render;

        protected static int tpage;
        protected static VAO vaoBoxTri;
        protected static VAO vaoBoxLine;
        protected static VAO vaoAxes;
        protected static VAO vaoSprites;
        // protected static VAO vaoText;

        private bool run = false;
        private bool loaded = false;
        private static HashSet<Type> loaded_static_types = new();

        private readonly HashSet<Keys> keysdown = new();
        private readonly HashSet<Keys> keyspressed = new();
        protected bool KDown(Keys key) => keysdown.Contains(key);
        protected bool KPress(Keys key) => keyspressed.Contains(key);
        private bool mouseright = false;
        private bool mouseleft = false;
        private int mousex = 0;
        private int mousey = 0;
        private float movespeed = 7.5f;
        private float rotspeed = 0.5f;
        private float zoomspeed = 0.75f;
        private const float PerFrame = 1 / 60f;

        protected readonly NSF nsf;

        protected abstract bool UseGrid { get; }

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
            vaoGridLine.VertCount = 0;
            for (int i = 0; i < verts.Length; ++i)
            {
                vaoSphereLine.PushAttrib(trans: verts[i]);
            }
            SpherePosLastUploaded = resolution;
        }
        protected void MakeLineGrid(int resolution)
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
            vaoGridLine.VertCount = 0;
            for (int i = 0; i < verts.Length; ++i)
            {
                vaoGridLine.PushAttrib(trans: verts[i]);
            }
            GridPosLastUploaded = resolution;
        }

        private static IGraphicsContext context;
        private static GLViewer contextWindow;

        public GLViewer() : base(GraphicsMode.Default, 4, 3, GraphicsContextFlags.Debug)
        {
            if (context == null)
            {
                context = Context;
                contextWindow = this;
            }
            render = new RenderInfo(this);
        }

        public GLViewer(NSF nsf) : base(GraphicsMode.Default, 4, 3, GraphicsContextFlags.Debug)
        {
            if (context == null)
            {
                context = Context;
                contextWindow = this;
            }
            render = new RenderInfo(this);
            this.nsf = nsf;
        }

        protected new void SwapBuffers()
        {
            ((GLControl)contextWindow).SwapBuffers();
        }

        protected new void MakeCurrent()
        {
            ((GLControl)contextWindow).Context.MakeCurrent(this.WindowInfo);
        }

        protected abstract IEnumerable<IPosition> CorePositions { get; }

        // for all instances, runs once.
        protected virtual void GLLoadStatic()
        {
            // nothing.
            Console.WriteLine($"GLLoadStatic {GetType().Name}");
        }

        // for each instance
        protected virtual void GLLoad()
        {
            if (!loaded_static_types.Contains(Program.TopLevelGLViewer.GetType()))
            {
                Program.TopLevelGLViewer.GLLoadStatic();
                loaded_static_types.Add(Program.TopLevelGLViewer.GetType());
            }
            if (!loaded_static_types.Contains(GetType()))
            {
                GLLoadStatic();
                loaded_static_types.Add(GetType());
            }

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

        protected virtual void RunLogic()
        {
            var d = movespeed * PerFrame * (render.Distance / RenderInfo.InitialDistance);
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
            if (KPress(Keys.R))
            {
                render.Reset();
                ResetCamera();
            }
            if (KPress(Keys.T)) render.EnableTexture = !render.EnableTexture;
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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            lock (render.mLock)
            {
                var olddist = render.Distance;
                float delta = (float)e.Delta / SystemInformation.MouseWheelScrollDelta * zoomspeed;
                render.Distance = Math.Max(RenderInfo.MinDistance, Math.Min(render.Distance - delta, RenderInfo.MaxDistance));
                // render.Projection.Trans -= (Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot)) * new Vector4(0, 0, render.Distance - olddist, 1)).Xyz;
            }
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!loaded)
            {
                MakeCurrent();
                GLLoad();
                loaded = true;
            }
            else if (run)
            {
                MakeCurrent();

                // mutex lock to prevent races with the renderinfo timer and using the renderinfo itself
                lock (render.mLock)
                {
                    // set up viewport clip
                    GL.Viewport(0, 0, Width, Height);
                    render.Projection.Width = Width;
                    render.Projection.Height = Height;

                    // Clear buffers
                    GL.DepthMask(true);
                    var col = Properties.Settings.Default.ClearColorRGB;
                    if ((col & 0xffffff) == 0) col = 0;
                    GL.ClearColor(Color.FromArgb(col));
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    // set up view matrices (45º FOV)
                    render.Projection.Perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), render.Projection.Aspect, 0.25f, 8192);
                    var rot_mat = Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot));
                    var test_vec = (rot_mat * new Vector4(0, 0, render.Distance, 1)).Xyz;
                    render.Projection.View = Matrix4.CreateTranslation(render.Projection.Trans - test_vec) * rot_mat;

                    // render
                    Render();
                }

                // swap the front/back buffers so what we just rendered to the back buffer is displayed in the window.
                Context.SwapBuffers();
            }
            base.OnPaint(e);
        }

        protected virtual void Render()
        {
            SetBlendMode(BlendMode.Solid);

            // vaoTest.Render(render);
            if (UseGrid && Properties.Settings.Default.DisplayAnimGrid)
            {
                RenderAxes(new Vector3(0));

                MakeLineGrid(Properties.Settings.Default.AnimGridLen);
                vaoGridLine.Render(render);
            }
        }

        private void RenderAxes(Vector3 pos)
        {
            vaoAxes.UserTrans = pos;
            vaoAxes.Render(render);
        }

        protected void RenderBox(Vector3 pos, Vector3 size, Color4 col)
        {
            vaoBoxTri.UserTrans = pos;
            vaoBoxTri.UserScale = size;
            vaoBoxTri.UserColor1 = col;
            vaoBoxTri.Render(render);
        }

        protected void RenderBoxLine(Vector3 pos, Vector3 size, Color4 col)
        {
            vaoBoxLine.UserTrans = pos;
            vaoBoxLine.UserScale = size;
            vaoBoxLine.UserColor1 = col;
            vaoBoxLine.Render(render);
        }

        protected void RenderBoxFilled(Vector3 pos, Vector3 size, Color4 col)
        {
            RenderBox(pos, size, col);
            RenderBoxLine(pos, size, col);
        }

        protected void RenderBoxAB(Vector3 a, Vector3 b, Color4 col) => RenderBox((a + b) / 2, (b - a) / 2, col);
        protected void RenderBoxLineAB(Vector3 a, Vector3 b, Color4 col) => RenderBoxLine((a + b) / 2, (b - a) / 2, col);
        protected void RenderBoxAS(Vector3 a, Vector3 b, Color4 col) => RenderBox(a + b / 2, b / 2, col);
        protected void RenderBoxLineAS(Vector3 a, Vector3 b, Color4 col) => RenderBoxLine(a + b / 2, b / 2, col);

        protected void RenderSprite(Vector3 trans, Vector2 size, Color4 col, Bitmap texture)
        {
            var texRect = OldResources.TexMap[texture];
            //Console.WriteLine($"TEXTURE: {texRect.Left},{texRect.Top}/{texRect.Right},{texRect.Bottom}");
            var uvs = new Vector2[4];
            uvs[0] = new Vector2(texRect.Left, texRect.Bottom);
            uvs[1] = new Vector2(texRect.Left, texRect.Top);
            uvs[2] = new Vector2(texRect.Right, texRect.Top);
            //uvs[3] = new Vector2(texRect.Right, texRect.Top);
            uvs[3] = new Vector2(texRect.Right, texRect.Bottom);
            //uvs[5] = new Vector2(texRect.Left, texRect.Bottom);
            vaoSprites.DiscardVerts();
            for (int i = 0; i < SpriteVerts.Length; ++i)
            {
                vaoSprites.PushAttrib(trans: SpriteVerts[i], st: uvs[i]);
            }
            vaoSprites.UserTrans = trans;
            vaoSprites.UserScale = new Vector3(size);
            vaoSprites.UserColor1 = col;
            vaoSprites.Render(render);
        }

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
            render.Distance = Math.Min(RenderInfo.MaxDistance, Math.Max(render.Distance, (float)(Math.Sqrt(Math.Pow(maxx - minx, 2) + Math.Pow(maxy - miny, 2) + Math.Pow(maxz - minz, 2)) * 1.2)));
            //render.Distance += 0;
            render.Projection.Trans.X = -midx;
            render.Projection.Trans.Y = -midy;
            render.Projection.Trans.Z = -midz;
            render.Projection.Rot.Y = 0;
            render.Projection.Rot.X = MathHelper.DegreesToRadians(15);
            Invalidate();
        }

        protected enum BlendMode { Trans = 0, Additive, Subtractive, Solid }

        protected void SetBlendMode(BlendMode bmode)
        {
            switch (bmode)
            {
                case BlendMode.Solid:
                case BlendMode.Trans:
                    GL.DepthMask(true);
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

        protected void SetupTPAGs(Dictionary<int, int> tex_eids)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tpage);

            // fill texture
            GL.GetTextureLevelParameter(tpage, 0, GetTextureParameter.TextureHeight, out int tpage_h);
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

        protected int MakeTexInfo(bool enable, int color = 0, int blend = 0, int clutx = 0, int cluty = 0, int face = 0, int page = 0)
        {
            // enable: 1, colormode: 2, blendmode: 2, clutx: 4, cluty: 7, doubleface: 1, page: X (>17 total)
            color &= 0x3;
            blend &= 0x3;
            clutx &= 0xF;
            cluty &= 0x7F;
            face &= 0x1;
            // page &= 0xFFFF;
            return (enable ? 1 : 0) | (color << 1) | (blend << 3) | (clutx << 5) | (cluty << 9) | (face << 16) | (page << 17);
        }

        protected override void Dispose(bool disposing)
        {
            if (context == Context)
            {
                context = null;
            }
            if (contextWindow == this)
            {
                contextWindow = null;
            }

            base.Dispose(disposing);
        }
    }
}
