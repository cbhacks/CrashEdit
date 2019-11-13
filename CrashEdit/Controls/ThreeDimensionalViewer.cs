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
        protected int[] textures;

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

        protected virtual int CameraRangeMargin => 0;

        protected abstract IEnumerable<IPosition> CorePositions
        {
            get;
        }

        protected abstract void RenderObjects();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.AlphaFunc(AlphaFunction.Greater, 0);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
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
            GL.Viewport(Location,Size);
            GL.ClearColor(0.05f,0.05f,0.05f,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            var proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3,(float)Width/Height,1/25f,ushort.MaxValue);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0,0,-1);
            GL.Scale(1F / range,1F / range,1F / range);
            GL.Rotate(roty,1,0,0);
            GL.Rotate(rotx,0,1,0);
            GL.Translate(-midx,-midy,-midz);
            RenderObjects();
            SwapBuffers();
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
            range = Math.Max(range,maxx - minx);
            range = Math.Max(range,maxy - miny);
            range = Math.Max(range,maxz - minz);
            range += CameraRangeMargin;
            rotx = 0;
            roty = 0;
            fullrange = range;
            Invalidate();
        }

        private long GenerateTextureHash(ModelTexture tex) // lol
        {
            return (long)tex.ClutY
                | (long)tex.ClutX << 7
                | (long)tex.TextureOffset / 4 << 11
                | (long)tex.Left << 14
                | (long)tex.Top << 24
                | (long)tex.Width << 31
                | (long)tex.Height << 41
                | (tex.BitFlag ? 1L : 0L) << 48;
        }

        public void ConvertTexturesToGL(TextureChunk[] texturechunks, IList<ModelTexture> modeltextures, byte[] eid_list, int eid_off)
        {
            MakeCurrent();
            //bitmaps = new Bitmap[model.Textures.Count];
            //Dictionary<long, Bitmap> bitmapbucket = new Dictionary<long, Bitmap>();
            textures = new int[modeltextures.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < textures.Length; ++i)
            {
                ModelTexture tex = modeltextures[i];
                int w = tex.Width + 1;
                int h = tex.Height + 1;
                TextureChunk texturechunk = null;
                int eid = BitConv.FromInt32(eid_list, eid_off + tex.TextureOffset);
                for (int t = 0; t < texturechunks.Length; ++t)
                {
                    if (eid == texturechunks[t].EID)
                    {
                        texturechunk = texturechunks[t];
                        break;
                    }
                }
                if (texturechunk == null) throw new Exception("ConvertTexturesToGL: Texture chunk not found");
                int[] pixels = new int[w * h]; // using indexed colors in GL would be dumb so we convert them to 32-bit
                if (tex.BitFlag) // 8-bit
                {
                    int[] palette = new int[256];
                    for (int j = 0; j < 256; ++j) // copy palette
                    {
                        palette[j] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturechunk.Data, tex.ClutX * 32 + tex.ClutY * 512 + j * 2));
                    }
                    for (int y = 0; y < h; ++y) // copy pixel data
                    {
                        for (int x = 0; x < w; ++x)
                        {
                            pixels[x + w * y] = palette[texturechunk.Data[(tex.Left + x) + (tex.Top + y) * 512]];
                        }
                    }
                }
                else // 4-bit
                {
                    int[] palette = new int[16];
                    for (int j = 0; j < 16; ++j) // copy palette
                    {
                        palette[j] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturechunk.Data, tex.ClutX * 32 + tex.ClutY * 512 + j * 2));
                    }
                    for (int y = 0; y < h; ++y) // copy pixels
                    {
                        for (int x = 0; x < w / 2; ++x) // 2 pixels per byte
                        {
                            pixels[x * 2 + w * y] = palette[texturechunk.Data[(tex.Left / 2 + x) + (tex.Top + y) * 512] & 0xF];
                            pixels[x * 2 + w * y + 1] = palette[texturechunk.Data[(tex.Left / 2 + x) + (tex.Top + y) * 512] >> 4 & 0xF];
                        }
                    }
                }
                long hash = GenerateTextureHash(tex);
                if (texturebucket.ContainsKey(hash))
                {
                    textures[i] = texturebucket[hash];
                }
                else
                {
                    //Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //unsafe
                    //{
                    //    for (int j = 0; j < w * h; ++j)
                    //    {
                    //        *((int*)data.Scan0.ToPointer() + j) = pixels[j];
                    //    }
                    //}
                    GL.GenTextures(1, out textures[i]);
                    GL.BindTexture(TextureTarget.Texture2D, textures[i]);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, w, h, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    //bmp.UnlockBits(data);
                    texturebucket[hash] = textures[i];
                }
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        protected override void Dispose(bool disposing)
        {
            inputtimer.Dispose();
            refreshtimer.Dispose();
            base.Dispose(disposing);
        }
    }
}
