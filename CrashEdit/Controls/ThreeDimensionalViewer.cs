using Crash;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class ThreeDimensionalViewer : GLControl
    {
        protected static void LoadTexture(Bitmap image)
        {
            BitmapData data = image.LockBits(new Rectangle(Point.Empty,image.Size),ImageLockMode.ReadOnly,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            try
            {
                GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,image.Width,image.Height,0,OpenTK.Graphics.OpenGL.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);
                GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Nearest);
            }
            finally
            {
                image.UnlockBits(data);
            }
        }

        private int midx;
        private int midy;
        private int midz;
        private int range;
        private int fullrange;
        protected int rotx;
        protected int roty;
        private bool mouseleft;
        private bool mouseright;
        private bool keyup;
        private bool keydown;
        private bool keyleft;
        private bool keyright;
        private bool keya;
        private bool keyz;
        private int mousex;
        private int mousey;
        private Timer inputtimer;
        private Timer refreshtimer;

        public ThreeDimensionalViewer()
        {
            mouseleft = false;
            mouseright = false;
            keyup = false;
            keydown = false;
            keyleft = false;
            keyright = false;
            keya = false;
            keyz = false;
            inputtimer = new Timer();
            inputtimer.Interval = 15;
            inputtimer.Enabled = true;
            inputtimer.Tick += delegate (object sender,EventArgs e)
            {
                int speed = range / 100;
                int changex = 0;
                int changey = 0;
                int changez = 0;
                if (keyup)
                    changez--;
                if (keydown)
                    changez++;
                if (keyleft)
                    changex--;
                if (keyright)
                    changex++;
                if (keya)
                    changey++;
                if (keyz)
                    changey--;
                midx += changex * speed;
                midy += changey * speed;
                midz += changez * speed;
                if ((changex | changey | changez) != 0)
                {
                    Invalidate();
                }
            };
            refreshtimer = new Timer();
            refreshtimer.Interval = 100;
            refreshtimer.Enabled = true;
            refreshtimer.Tick += delegate (object sender,EventArgs e)
            {
                Invalidate();
            };
        }

        protected virtual int CameraRangeMargin
        {
            get { return 400; }
        }

        protected abstract IEnumerable<IPosition> CorePositions
        {
            get;
        }

        protected abstract void RenderObjects();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ResetCamera();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseleft = true;
                    break;
                case MouseButtons.Right:
                    mouseright = true;
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseleft = false;
                    break;
                case MouseButtons.Right:
                    mouseright = false;
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mouseleft)
            {
                rotx += e.X - mousex;
                roty += e.Y - mousey;
                rotx %= 360;
                if (roty > 90)
                    roty = 90;
                if (roty < -90)
                    roty = -90;
                Invalidate();
            }
            else if (mouseright)
            {
                range -= (e.Y - mousey) * fullrange / 500;
                if (range < 1)
                    range = 1;
                Invalidate();
            }
            mousex = e.X;
            mousey = e.Y;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.A:
                case Keys.Z:
                case Keys.R:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    keyup = true;
                    break;
                case Keys.Down:
                    keydown = true;
                    break;
                case Keys.Left:
                    keyleft = true;
                    break;
                case Keys.Right:
                    keyright = true;
                    break;
                case Keys.A:
                    keya = true;
                    break;
                case Keys.Z:
                    keyz = true;
                    break;
                case Keys.R:
                    ResetCamera();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    keyup = false;
                    break;
                case Keys.Down:
                    keydown = false;
                    break;
                case Keys.Left:
                    keyleft = false;
                    break;
                case Keys.Right:
                    keyright = false;
                    break;
                case Keys.A:
                    keya = false;
                    break;
                case Keys.Z:
                    keyz = false;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MakeCurrent();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.AlphaFunc(AlphaFunction.Greater,0);
            GL.Viewport(Location,Size);
            GL.ClearColor(0.05f,0.05f,0.05f,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.01,+0.01,-0.01,+0.01,0.01,ushort.MaxValue);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0,0,-1);
            GL.Scale(1.0 / range,1.0 / range,1.0 / range);
            GL.Rotate(roty,1,0,0);
            GL.Rotate(rotx,0,1,0);
            GL.Translate(-midx,-midy,-midz);
            RenderObjects();
            SwapBuffers();
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.AlphaTest);
        }

        public void ResetCamera()
        {
            int minx = int.MaxValue;
            int miny = int.MaxValue;
            int minz = int.MaxValue;
            int maxx = int.MinValue;
            int maxy = int.MinValue;
            int maxz = int.MinValue;
            foreach (IPosition position in CorePositions)
            {
                minx = Math.Min(minx,(int)position.X);
                miny = Math.Min(miny,(int)position.Y);
                minz = Math.Min(minz,(int)position.Z);
                maxx = Math.Max(maxx,(int)position.X);
                maxy = Math.Max(maxy,(int)position.Y);
                maxz = Math.Max(maxz,(int)position.Z);
            }
            midx = (maxx + minx) / 2;
            midy = (maxy + miny) / 2;
            midz = (maxz + minz) / 2;
            range = 1;
            range = Math.Max(range,midx - minx);
            range = Math.Max(range,midy - miny);
            range = Math.Max(range,midz - minz);
            range += CameraRangeMargin;
            rotx = 0;
            roty = 0;
            fullrange = range;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            inputtimer.Dispose();
            refreshtimer.Dispose();
            base.Dispose(disposing);
        }
    }
}
