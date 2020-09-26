using Crash;
using CrashEdit.Properties;
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
                GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,image.Width,image.Height,0,OpenTK.Graphics.OpenGL.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);
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

        protected static long textureframe; // CrashEdit would have to run for so long for this to overflow
        private static Timer texturetimer;

        static ThreeDimensionalViewer()
        {
            textureframe = 0;
            texturetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate(),
                Enabled = true
            };
            texturetimer.Tick += delegate (object sender, EventArgs e)
            {
                ++textureframe;
                texturetimer.Interval = 1000 / OldMainForm.GetRate();
            };
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
        private int[][] textureIDs;
        private int[][] textures; // maps modeltexture offsets to an offset in textureIDs
        private int boundtex;
        private int blend;

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
            boundtex = 0;
            inputtimer = new Timer
            {
                Interval = 16,
                Enabled = true
            };
            inputtimer.Tick += delegate (object sender,EventArgs e)
            {
                int speed = 1 + range / 66;
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
            refreshtimer = new Timer
            {
                Interval = 33,
                Enabled = true
            };
            refreshtimer.Tick += delegate (object sender,EventArgs e)
            {
                Invalidate();
            };
        }

        protected virtual float NearPlane => 100;
        protected virtual float FarPlane => 400*800;
        protected virtual int CameraRangeMargin => 0;
        protected virtual int CameraRangeMinimum => 5;

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
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.AlphaFunc(AlphaFunction.Greater, 0);
            // Lighting settings. Lighting must be enabled for them to take effect, logically
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0, 0, 0, 1 });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1 }); // set some minimum light parameters so less shading doesn't make things too dark
            GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 1.25F); // reduce direct light intensity
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            SetBlendMode(3);
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
                range -= (int)((e.Y - mousey) * fullrange / 256 * (range / (fullrange*8 * 0.67F + 0.33F)));
                if (range < 5)
                    range = 5;
                else if (range > fullrange * 8)
                    range = fullrange * 8;
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
            lastimage = null;
            MakeCurrent();
            GL.Viewport(Location,Size);
            GL.ClearColor(Color.FromArgb(Settings.Default.ClearColorRGB));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            var proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3,(float)Width/Height,NearPlane,FarPlane);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0,0,-range);
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
                minx = (int)Math.Min(minx,position.X);
                miny = (int)Math.Min(miny,position.Y);
                minz = (int)Math.Min(minz,position.Z);
                maxx = (int)Math.Max(maxx,position.X);
                maxy = (int)Math.Max(maxy,position.Y);
                maxz = (int)Math.Max(maxz,position.Z);
            }
            midx = (maxx + minx) / 2;
            midy = (maxy + miny) / 2;
            midz = (maxz + minz) / 2;
            range = Math.Max(CameraRangeMinimum, (int)(Math.Sqrt(Math.Pow(maxx-midx, 2) + Math.Pow(maxy-midy, 2) + Math.Pow(maxz-midz, 2))*1.15));
            range += CameraRangeMargin;
            rotx = 0;
            roty = 15;
            fullrange = range;
            Invalidate();
        }

        protected void SetBlendMode(int blendmode)
        {
            if (blend == blendmode) return;
            switch (blendmode)
            {
                case 0: GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); break; // translucent
                case 1: GL.BlendFunc(BlendingFactor.One, BlendingFactor.One); break; // additive
                case 2: GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); break; // subtractive - not supported
                case 3: GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); break; // normal
            }
            blend = blendmode;
        }

        protected void InitTextures(int count)
        {
            textureIDs = new int[count][];
            textures = new int[count][];
        }

        internal void ConvertTextureDataTo32Bit(int w,int h,int l,int t,int cx,int cy,byte colormode,byte blendmode,byte[] texturedata,ref int[] pixeldata)
        {
            if (colormode == 2) // 16-bit
            {
                for (int y = 0; y < h; ++y) // copy pixel data
                {
                    for (int x = 0; x < w; ++x)
                    {
                        pixeldata[l+x+(t+y)*256] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturedata,(l+x)*2 + (t+y) * 512),blendmode);
                    }
                }
            }
            else if (colormode == 1) // 8-bit
            {
                int[] palette = new int[256];
                for (int j = 0; j < 256; ++j) // copy palette
                {
                    palette[j] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturedata,cx*32+cy*512+j*2),blendmode);
                }
                for (int y = 0; y < h; ++y) // copy pixel data
                {
                    for (int x = 0; x < w; ++x)
                    {
                        pixeldata[l+x+(t+y)*512] = palette[texturedata[(l+x) + (t+y) * 512]];
                    }
                }
            }
            else if (colormode == 0) // 4-bit
            {
                int[] palette = new int[16];
                for (int j = 0; j < 16; ++j) // copy palette
                {
                    palette[j] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturedata,cx*32+cy*512+j*2),blendmode);
                }
                for (int y = 0; y < h; ++y) // copy pixels
                {
                    for (int x = 0; x < w / 2; ++x) // 2 pixels per byte
                    {
                        pixeldata[l+x*2+(t+y)*1024] = palette[texturedata[(l/2+x) + (t+y)*512] & 0xF];
                        pixeldata[l+x*2+(t+y)*1024+1] = palette[texturedata[(l/2+x) + (t+y)*512] >> 4 & 0xF];
                    }
                }
            }
        }

        internal void MakeGLTextures(int list,Dictionary<long,int> texturebucket,ref int[][] texturepages)
        {
            // get rid of unused textures
            HashSet<int> usedids = new HashSet<int>();
            foreach (int id in texturebucket.Values)
            {
                usedids.Add(id);
            }
            for (int i = 0; i < texturepages.Length; ++i)
            {
                if (!usedids.Contains(i))
                    continue;
                textureIDs[list][i] = GL.GenTexture();
                //Bitmap bmp = new Bitmap(texturepages[i].Length / 128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //unsafe
                //{
                //    for (int j = 0; j < texturepages[i].Length; ++j)
                //    {
                //        *((int*)data.Scan0.ToPointer() + j) = texturepages[i][j];
                //    }
                //}
                GL.BindTexture(TextureTarget.Texture2D, textureIDs[list][i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, texturepages[i].Length/128, 128, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texturepages[i]);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                //string filename = $"tex_{i}_{texturepages[i].Length / 128}x128.png";
                //bmp.Save(/*YOURPATHHERE*/ + filename, ImageFormat.Png);
                //bmp.UnlockBits(data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        internal long GenerateTextureHash(ref ModelTexture tex) // compresses a model texture's relevant texture info into a standard type that can be quickly looked up
        {
            return (long)tex.ClutY
                | (long)tex.ClutX << 7
                | (long)tex.Page << 11
                | (long)tex.Left << 14
                | (long)tex.Top << 24
                | (long)tex.Width << 31
                | (long)tex.Height << 41
                | (long)tex.ColorMode << 48
                | (long)tex.BlendMode << 50; // 52 bits total
        }

        protected void ConvertTexturesToGL(int list, TextureChunk[] texturechunks, IList<ModelTexture> modeltextures)
        {
            textureIDs[list] = new int[texturechunks.Length*6];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturepages[i*6+0] = new int[1024*128]; // 4bpp
                texturepages[i*6+1] = new int[512*128]; // 8bpp
                texturepages[i*6+2] = new int[256*128]; // 16bpp
                // transparency
                texturepages[i*6+3] = new int[1024*128]; // 4bpp
                texturepages[i*6+4] = new int[512*128]; // 8bpp
                texturepages[i*6+5] = new int[256*128]; // 16bpp
            }
            textures[list] = new int[modeltextures.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < modeltextures.Count; ++i)
            {
                ModelTexture tex = modeltextures[i];
                long hash = GenerateTextureHash(ref tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    TextureChunk texturechunk = texturechunks[tex.Page];
                    int page = tex.Page*6 + tex.ColorMode + (tex.BlendMode == 0 ? 3 : 0);
                    ConvertTextureDataTo32Bit(tex.Width+1,tex.Height+1,tex.Left,tex.Top,tex.ClutX,tex.ClutY,tex.ColorMode,tex.BlendMode,texturechunk.Data,ref texturepages[page]);
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            MakeGLTextures(list,texturebucket,ref texturepages);
        }

        internal long GenerateTextureHash(int tpag, ref OldSceneryTexture tex) // compresses a model texture's relevant texture info into a standard type that can be quickly looked up
        {
            return (long)tex.ClutY
                | (long)tex.ClutX << 7
                | (long)tpag << 11
                | (long)tex.Left << 14
                | (long)tex.Top << 24
                | (long)(tex.UVIndex % 5) << 31
                | (long)(tex.UVIndex / 5 % 5) << 34
                | (long)tex.ColorMode << 37
                | (long)tex.BlendMode << 39; // 41 bits total
        }

        protected void ConvertTexturesToGL(int list, TextureChunk[] texturechunks, IList<OldSceneryPolygon> oldscenerypolygons, IList<OldModelStruct> oldmodelstructs)
        {
            textureIDs[list] = new int[texturechunks.Length*6];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturepages[i*6+0] = new int[1024*128]; // 4bpp
                texturepages[i*6+1] = new int[512*128]; // 8bpp
                texturepages[i*6+2] = new int[256*128]; // 16bpp
                // transparency
                texturepages[i*6+3] = new int[1024*128]; // 4bpp
                texturepages[i*6+4] = new int[512*128]; // 8bpp
                texturepages[i*6+5] = new int[256*128]; // 16bpp
            }
            textures[list] = new int[oldscenerypolygons.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < oldscenerypolygons.Count; ++i)
            {
                OldSceneryPolygon poly = oldscenerypolygons[i];
                OldModelStruct modelstruct = oldmodelstructs[poly.ModelStruct];
                if (modelstruct is OldSceneryColor || modelstruct == null)
                    continue;
                OldSceneryTexture tex = (OldSceneryTexture)modelstruct;
                long hash = GenerateTextureHash(poly.Page,ref tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    TextureChunk texturechunk = texturechunks[poly.Page];
                    int page = poly.Page*6 + tex.ColorMode + (tex.BlendMode == 0 ? 3 : 0);
                    ConvertTextureDataTo32Bit(tex.Width,tex.Height,tex.Left,tex.Top,tex.ClutX,tex.ClutY,tex.ColorMode,tex.BlendMode,texturechunk.Data,ref texturepages[page]);
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            MakeGLTextures(list,texturebucket,ref texturepages);
        }
        
        protected void ConvertTexturesToGL(int list, TextureChunk[] texturechunks, IList<ProtoSceneryPolygon> protoscenerypolygons, IList<OldModelStruct> oldmodelstructs)
        {
            textureIDs[list] = new int[texturechunks.Length*6];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturepages[i*6+0] = new int[1024*128]; // 4bpp
                texturepages[i*6+1] = new int[512*128]; // 8bpp
                texturepages[i*6+2] = new int[256*128]; // 16bpp
                // transparency
                texturepages[i*6+3] = new int[1024*128]; // 4bpp
                texturepages[i*6+4] = new int[512*128]; // 8bpp
                texturepages[i*6+5] = new int[256*128]; // 16bpp
            }
            textures[list] = new int[protoscenerypolygons.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < protoscenerypolygons.Count; ++i)
            {
                ProtoSceneryPolygon poly = protoscenerypolygons[i];
                OldModelStruct modelstruct = oldmodelstructs[poly.Texture];
                if (modelstruct is OldSceneryColor || modelstruct == null)
                    continue;
                OldSceneryTexture tex = (OldSceneryTexture)modelstruct;
                long hash = GenerateTextureHash(poly.Page,ref tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    TextureChunk texturechunk = texturechunks[poly.Page];
                    int page = poly.Page*6 + tex.ColorMode + (tex.BlendMode == 0 ? 3 : 0);
                    ConvertTextureDataTo32Bit(tex.Width,tex.Height,tex.Left,tex.Top,tex.ClutX,tex.ClutY,tex.ColorMode,tex.BlendMode,texturechunk.Data,ref texturepages[page]);
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            MakeGLTextures(list,texturebucket,ref texturepages);
        }

        internal long GenerateTextureHash(int tpag, ref OldModelTexture tex) // compresses a model texture's relevant texture info into a standard type that can be quickly looked up
        {
            return (long)tex.ClutY
                | (long)tex.ClutX << 7
                | (long)tpag << 11
                | (long)tex.Left << 14
                | (long)tex.Top << 24
                | (long)(tex.UVIndex % 5) << 31
                | (long)(tex.UVIndex / 5 % 5) << 34
                | (long)tex.ColorMode << 37
                | (long)tex.BlendMode << 39; // 41 bits total
        }

        protected void ConvertTexturesToGL(int list, Dictionary<int,TextureChunk> texturechunks, IList<OldModelStruct> modeltextures)
        {
            textureIDs[list] = new int[texturechunks.Count*6];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Count; ++i)
            {
                texturepages[i*6+0] = new int[1024*128]; // 4bpp
                texturepages[i*6+1] = new int[512*128]; // 8bpp
                texturepages[i*6+2] = new int[256*128]; // 16bpp
                // transparency
                texturepages[i*6+3] = new int[1024*128]; // 4bpp
                texturepages[i*6+4] = new int[512*128]; // 8bpp
                texturepages[i*6+5] = new int[256*128]; // 16bpp
            }
            List<int> textureeids = new List<int>(texturechunks.Keys);
            textures[list] = new int[modeltextures.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < modeltextures.Count; ++i)
            {
                if (!(modeltextures[i] is OldModelTexture tex))
                    continue;
                long hash = GenerateTextureHash(textureeids.IndexOf(tex.EID),ref tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    int page = textureeids.IndexOf(tex.EID)*6 + tex.ColorMode + (tex.BlendMode == 0 ? 3 : 0);
                    ConvertTextureDataTo32Bit(tex.Width,tex.Height,tex.Left,tex.Top,tex.ClutX,tex.ClutY,tex.ColorMode,tex.BlendMode,texturechunks[tex.EID].Data,ref texturepages[page]);
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            MakeGLTextures(list,texturebucket,ref texturepages);
        }

        protected void UnbindTexture()
        {
            if (boundtex == 0) return;
            GL.BindTexture(TextureTarget.Texture2D, 0);
            boundtex = 0;
        }

        protected void BindTexture(int list, int i)
        {
            int tex = textureIDs[list][textures[list][i]];
            if (tex == boundtex) return;
            GL.BindTexture(TextureTarget.Texture2D, tex);
            boundtex = tex;
        }

        protected override void Dispose(bool disposing)
        {
            inputtimer.Dispose();
            refreshtimer.Dispose();
            if (textureIDs != null)
            {
                for (int i = 0; i < textureIDs.Length; ++i)
                {
                    if (textureIDs[i] != null)
                    {
                        GL.DeleteTextures(textureIDs[i].Length, textureIDs[i]);
                        textureIDs[i] = null;
                    }
                }
                textureIDs = null;
            }
            base.Dispose(disposing);
        }
    }
}
