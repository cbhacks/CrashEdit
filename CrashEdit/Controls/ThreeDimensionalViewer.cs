using Crash;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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
        protected int rotx;
        protected int roty;
        private bool mouseleft;
        private bool mouseright;
        private int mousex;
        private int mousey;
        private Timer timer;

        public ThreeDimensionalViewer()
        {
            mouseleft = false;
            mouseright = false;
            timer = new Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Tick += delegate (object sender,EventArgs e)
            {
                Invalidate();
            };
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
                range -= e.Y - mousey;
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
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            const int speed = 16;
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    midz -= speed;
                    break;
                case Keys.Down:
                    midz += speed;
                    break;
                case Keys.Left:
                    midx -= speed;
                    break;
                case Keys.Right:
                    midx += speed;
                    break;
                case Keys.A:
                    midy += speed;
                    break;
                case Keys.Z:
                    midy -= speed;
                    break;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MakeCurrent();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.AlphaFunc(AlphaFunction.Greater,0);
            GL.Viewport(Location,Size);
            GL.ClearColor(Color.Black);
            GL.ClearDepth(short.MaxValue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.0001,+0.0001,-0.0001,+0.0001,0.0001,short.MaxValue);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0,0,-1);
            GL.Scale(1.0 / range,1.0 / range,1.0 / range);
            GL.Rotate(roty,1,0,0);
            GL.Rotate(rotx,0,1,0);
            GL.Translate(-midx,-midy,-midz);
            RenderObjects();
            SwapBuffers();
        }

        public void ResetCamera()
        {
            int minx = short.MaxValue;
            int miny = short.MaxValue;
            int minz = short.MaxValue;
            int maxx = short.MinValue;
            int maxy = short.MinValue;
            int maxz = short.MinValue;
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
            range += 400;
            rotx = 0;
            roty = 0;
        }

        protected override void Dispose(bool disposing)
        {
            timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
