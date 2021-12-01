using Crash;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class GLViewer : GLControl, IGLDisposable
    {
        protected static readonly CullFaceMode[] cullFaceModes = { CullFaceMode.Back, CullFaceMode.Front, CullFaceMode.FrontAndBack };

        private static Bitmap lastimage = null;
        private static int defaulttexture = 0;

        protected static void LoadTexture(Bitmap image)
        {
            if (defaulttexture == 0) defaulttexture = GL.GenTexture();
            if (image == lastimage) return; // no reload
            BitmapData data = image.LockBits(new Rectangle(Point.Empty,image.Size),ImageLockMode.ReadOnly,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                GL.BindTexture(TextureTarget.Texture2D, defaulttexture);
                GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,image.Width,image.Height,0,OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);
                GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Nearest);
            }
            catch
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            finally
            {
                image.UnlockBits(data);
                lastimage = image;
            }
        }

        static readonly Vector3[] AxesPos = new Vector3[6] {
            new(-.5f, 0, 0),
            new(1, 0, 0),
            new(0, -.5f, 0),
            new(0, 1, 0),
            new(0, 0, -.5f),
            new(0, 0, 1)
        };
        static readonly Vector3[] BoxLineVerts = new Vector3[24] {
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
        static readonly Vector3[] BoxTriVerts = new Vector3[36] {
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
        private readonly Dictionary<int, Vector4[]> SpherePosCache = new();
        private int SpherePosLastUploaded = -1;
        protected VAO vaoSphereLine;
        private readonly Dictionary<int, Vector4[]> GridPosCache = new();
        private int GridPosLastUploaded = -1;
        protected VAO vaoGridLine;

        protected readonly RenderInfo render;

        private int tpage;
        private VAO vaoBox;
        private VAO vaoAxes;
        // private VAO vaoText;

        private bool run = false;

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

        protected abstract bool UseGrid { get; }

        protected void MakeLineSphere(int resolution)
        {
            if (resolution < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), "Sphere resolution cannot be less than 0.");
            if (SpherePosLastUploaded == resolution) return;
            if (!SpherePosCache.ContainsKey(resolution))
            {
                int long_amt = resolution * 4;
                int lat_amt = resolution;
                int pt_nb = 1 + long_amt * (2 + 2 * lat_amt) + (1 + long_amt) * (1 + 2 * lat_amt);
                var pos = new Vector4[pt_nb];
                int i = 1;
                pos[0] = new Vector4(0, 0, 1, 1);
                bool even = true;
                for (int ii = 0; ii < long_amt; ++ii)
                {
                    var rotmat = Matrix4.CreateRotationZ((float)ii / long_amt * MathHelper.TwoPi);
                    if (ii % 2 == 0)
                    {
                        for (int iii = 0, l_m = 2 + lat_amt * 2; iii < l_m; ++iii)
                        {
                            pos[i++] = pos[0] * Matrix4.CreateRotationX((float)(iii + 1) / l_m * MathHelper.Pi) * rotmat;
                        }
                        even = true;
                    }
                    else
                    {
                        for (int iii = 0, l_m = 2 + lat_amt * 2; iii < l_m; ++iii)
                        {
                            pos[i++] = pos[0] * Matrix4.CreateRotationX((float)(l_m - iii - 1) / l_m * MathHelper.Pi) * rotmat;
                        }
                        even = false;
                    }
                }
                for (int ii = 1, l_m = lat_amt * 2 + 2; ii < l_m; ++ii)
                {
                    Matrix4 rotmat;
                    if (!even)
                    {
                        rotmat = Matrix4.CreateRotationX((float)ii / l_m * MathHelper.Pi);
                    }
                    else
                    {
                        rotmat = Matrix4.CreateRotationX((float)(l_m - ii) / l_m * MathHelper.Pi);
                    }
                    for (int iii = 0; iii <= long_amt; ++iii)
                    {
                        pos[i++] = pos[0] * rotmat * Matrix4.CreateRotationZ((float)iii / long_amt * MathHelper.TwoPi);
                    }
                }
                SpherePosCache.Add(resolution, pos);
            }
            vaoSphereLine.UpdatePositions(SpherePosCache[resolution]);
            vaoSphereLine.VertCount = SpherePosCache[resolution].Length;
            SpherePosLastUploaded = resolution;
        }
        protected void MakeLineGrid(int resolution)
        {
            if (resolution < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution), "Grid resolution cannot be less than 0.");
            if (GridPosLastUploaded == resolution) return;

            if (!GridPosCache.ContainsKey(resolution))
            {
                var pos = new Vector4[4 * resolution * 2];
                var border = resolution * 1f - 0.5f;

                var pi = 0;
                for (int i = 0; i < resolution * 2; ++i)
                {
                    pos[pi++] = new Vector4(-border + i, 0, -border, 1);
                    pos[pi++] = new Vector4(-border + i, 0, +border, 1);
                    pos[pi++] = new Vector4(-border, 0, -border + i, 1);
                    pos[pi++] = new Vector4(+border, 0, -border + i, 1);
                }

                GridPosCache.Add(resolution, pos);
            }

            vaoGridLine.UpdatePositions(GridPosCache[resolution]);
            vaoGridLine.VertCount = GridPosCache[resolution].Length;
            GridPosLastUploaded = resolution;

        }

        public GLViewer() : base(GraphicsMode.Default, 4, 3, GraphicsContextFlags.Debug)
        {
            render = new RenderInfo(this);
        }

        protected abstract IEnumerable<IPosition> CorePositions { get; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MakeCurrent();

            // version print
            Console.WriteLine($"OpenGL version: {GL.GetString(StringName.Version)}");

            int flags = GL.GetInteger(GetPName.ContextFlags);
            // Console.WriteLine($"flags: {flags}");
            if ((flags & (int)ContextFlagMask.ContextFlagDebugBit) != 0)
            {
                // Console.WriteLine("GL debug enabled.");
                // Enable debug callbacks.
                GL.Enable(EnableCap.DebugOutput);
                GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
                {
                    string msg = Marshal.PtrToStringAnsi(message);
                    switch (severity)
                    {
                        case DebugSeverity.DebugSeverityHigh:
                            Console.WriteLine("OpenGL ERROR: " + msg);
                            break;
                        case DebugSeverity.DebugSeverityMedium:
                            Console.WriteLine("OpenGL WARN: " + msg);
                            break;
                        case DebugSeverity.DebugSeverityLow:
                            Console.WriteLine("OpenGL INFO: " + msg);
                            break;
                        default:
                            Console.WriteLine("OpenGL OTHER: " + msg);
                            break;
                    }
                }, IntPtr.Zero);
            }

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // init all shaders
            render.ShaderContext.InitShaders();

            // init axes vao
            vaoAxes = new VAO(render.ShaderContext, "axes", PrimitiveType.Lines);
            vaoAxes.UpdatePositions(AxesPos);

            vaoSphereLine = new VAO(render.ShaderContext, "line-model", PrimitiveType.LineStrip);
            vaoGridLine = new VAO(render.ShaderContext, "line-usercolor", PrimitiveType.Lines);
            vaoBox = new VAO(render.ShaderContext, "box-model", PrimitiveType.Triangles);

            // make texture
            tpage = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, tpage);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8ui, 512, 0, 0, OpenTK.Graphics.OpenGL4.PixelFormat.RedInteger, PixelType.UnsignedByte, IntPtr.Zero);

            // enable logic
            run = true;

            ResetCamera();
        }

        public void RunLogic()
        {
            if (!run) return;
            ActualRunLogic();
            keyspressed.Clear();
        }

        protected virtual void ActualRunLogic()
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
            if (render.Started)
            {
                MakeCurrent();

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
                render.Projection.Perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), render.Projection.Aspect, 0.1f, 16384);
                var rot_mat = Matrix4.CreateFromQuaternion(new Quaternion(render.Projection.Rot));
                var test_vec = (rot_mat * new Vector4(0, 0, render.Distance, 1)).Xyz;
                render.Projection.View = Matrix4.CreateTranslation(render.Projection.Trans - test_vec) * rot_mat;

                // mutex lock to prevent races with the renderinfo timer and using the renderinfo itself
                lock (render.mLock)
                {
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
            SetBlendForRenderPass(RenderPass.Solid);

            // vaoTest.Render(render);
            if (UseGrid && Properties.Settings.Default.DisplayAnimGrid)
            {
                RenderAxes(new Vector3(0));

                MakeLineGrid(Properties.Settings.Default.AnimGridLen);
                render.Projection.UserColor1 = Color4.Gray;
                vaoGridLine.Render(render);
            }
        }

        private void RenderAxes(Vector3 pos)
        {
            render.Projection.UserTrans = pos;
            vaoAxes.Render(render, vertcount: 6);
        }

        protected void RenderBox(Vector3 pos, Vector3 size, Color4 col)
        {
            render.Projection.UserTrans = pos;
            render.Projection.UserScale = size;
            render.Projection.UserColor1 = col;
            vaoBox.Primitive = PrimitiveType.Triangles;
            vaoBox.UpdatePositions(BoxTriVerts);
            vaoBox.Render(render, vertcount: BoxTriVerts.Length);
        }

        protected void RenderBoxLine(Vector3 pos, Vector3 size, Color4 col)
        {
            render.Projection.UserTrans = pos;
            render.Projection.UserScale = size;
            render.Projection.UserColor1 = col;
            vaoBox.Primitive = PrimitiveType.Lines;
            vaoBox.UpdatePositions(BoxLineVerts);
            vaoBox.Render(render, vertcount: BoxLineVerts.Length);
        }

        protected void RenderBoxFilled(Vector3 pos, Vector3 size, Color4 col)
        {
            RenderBox(pos, size, col);
            RenderBoxLine(pos, size, col);
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
                minx = (float)Math.Min(minx,position.X);
                miny = (float)Math.Min(miny,position.Y);
                minz = (float)Math.Min(minz,position.Z);
                maxx = (float)Math.Max(maxx,position.X);
                maxy = (float)Math.Max(maxy,position.Y);
                maxz = (float)Math.Max(maxz,position.Z);
            }
            float midx = (maxx + minx) / 2;
            float midy = (maxy + miny) / 2;
            float midz = (maxz + minz) / 2;
            render.Distance = Math.Max(render.Distance, (float)(Math.Sqrt(Math.Pow(maxx-minx, 2) + Math.Pow(maxy-miny, 2) + Math.Pow(maxz-minz, 2))*1.2));
            //render.Distance += 0;
            render.Projection.Trans.X = -midx;
            render.Projection.Trans.Y = -midy;
            render.Projection.Trans.Z = -midz;
            render.Projection.Rot.Y = 0;
            render.Projection.Rot.X = MathHelper.DegreesToRadians(15);
            Invalidate();
        }

        protected enum RenderPass { Solid, Trans, Additive, Subtractive }

        protected void SetBlendForRenderPass(RenderPass pass)
        {
            switch (pass)
            {
                default:
                case RenderPass.Solid:
                case RenderPass.Trans:
                    GL.DepthMask(true);
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;
                case RenderPass.Additive:
                    GL.DepthMask(false);
                    GL.BlendEquation(BlendEquationMode.FuncAdd);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    break;
                case RenderPass.Subtractive:
                    GL.DepthMask(false);
                    GL.BlendEquation(BlendEquationMode.FuncReverseSubtract);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    break;
            }
        }

        protected void SetupTPAGs(NSF nsf, Dictionary<int, int> tex_eids)
        {
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
        
        protected override void Dispose(bool disposing)
        {
            GLDispose();
            base.Dispose(disposing);
        }

        public void GLDispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteTexture(tpage);
            render.ShaderContext.KillShaders();

            vaoAxes?.GLDispose();
            vaoSphereLine?.GLDispose();
            vaoBox?.GLDispose();
        }
    }
}
