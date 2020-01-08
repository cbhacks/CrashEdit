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
                int speed = 50 + range / 66;
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

        protected virtual float ScaleFactor => 1;
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
                range -= (int)((e.Y - mousey) * fullrange / 256 * (range / (float)(fullrange * 8) * 0.8F + 0.2F));
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
            GL.ClearColor(0.025f,0.025f,0.025f,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            var proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3,(float)Width/Height,300,2000000);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0,0,-range);
            GL.Rotate(roty,1,0,0);
            GL.Rotate(rotx,0,1,0);
            GL.Translate(-midx,-midy,-midz);
            GL.Scale(ScaleFactor,ScaleFactor,ScaleFactor);
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
                minx = (int)Math.Min(minx,position.X * ScaleFactor);
                miny = (int)Math.Min(miny,position.Y * ScaleFactor);
                minz = (int)Math.Min(minz,position.Z * ScaleFactor);
                maxx = (int)Math.Max(maxx,position.X * ScaleFactor);
                maxy = (int)Math.Max(maxy,position.Y * ScaleFactor);
                maxz = (int)Math.Max(maxz,position.Z * ScaleFactor);
            }
            midx = (maxx + minx) / 2;
            midy = (maxy + miny) / 2;
            midz = (maxz + minz) / 2;
            range = CameraRangeMinimum;
            range = Math.Max(range,maxx - minx);
            range = Math.Max(range,maxy - miny);
            range = Math.Max(range,maxz - minz);
            range += (int)(CameraRangeMargin * ScaleFactor);
            rotx = 0;
            roty = 0;
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

        private long GenerateTextureHash(ModelTexture tex) // compresses a model texture's relevant texture info into a standard type that can be quickly looked up
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

        protected void ConvertTexturesToGL(int list, TextureChunk[] texturechunks, IList<ModelTexture> modeltextures, byte[] eid_list, int eid_off)
        {
            textureIDs[list] = new int[texturechunks.Length*2];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturepages[i*2+0] = new int[1024*128]; // 4bpp
                texturepages[i*2+1] = new int[512*128]; // 8bpp
            }
            textures[list] = new int[modeltextures.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < modeltextures.Count; ++i)
            {
                ModelTexture tex = modeltextures[i];
                long hash = GenerateTextureHash(tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    if (tex.TextureOffset / 4 >= texturechunks.Length) throw new Exception("ConvertTexturesToGL: Texture chunk index out of bounds");
                    if (tex.TextureOffset % 4 != 0) throw new Exception("ConvertTexturesToGL: Texture chunk index is unaligned");
                    TextureChunk texturechunk = null;
                    foreach (TextureChunk chunk in texturechunks)
                    {
                        if (chunk.EID == BitConv.FromInt32(eid_list,eid_off + tex.TextureOffset))
                        {
                            texturechunk = chunk;
                            break;
                        }
                    }
                    if (texturechunk == null) throw new Exception("ConvertTexturesToGL: Texture chunk not found");
                    int w = tex.Width + 1;
                    int h = tex.Height + 1;
                    int page = tex.TextureOffset / 2 + Convert.ToInt32(tex.BitFlag);
                    if (tex.BitFlag) // 8-bit
                    {
                        int[] palette = new int[256];
                        for (int j = 0; j < 256; ++j) // copy palette
                        {
                            palette[j] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturechunk.Data,tex.ClutX*32 + tex.ClutY*512 + j*2), tex.BlendMode);
                        }
                        for (int y = 0; y < h; ++y) // copy pixel data
                        {
                            for (int x = 0; x < w; ++x)
                            {
                                texturepages[page][tex.Left + tex.Top*512 + x + 512*y] = palette[texturechunk.Data[(tex.Left + x) + (tex.Top + y) * 512]];
                            }
                        }
                    }
                    else // 4-bit
                    {
                        int[] palette = new int[16];
                        for (int j = 0; j < 16; ++j) // copy palette
                        {
                            palette[j] = PixelConv.Convert5551_8888(BitConv.FromInt16(texturechunk.Data,tex.ClutX*32 + tex.ClutY*512 + j*2), tex.BlendMode);
                        }
                        for (int y = 0; y < h; ++y) // copy pixels
                        {
                            for (int x = 0; x < w / 2; ++x) // 2 pixels per byte
                            {
                                texturepages[page][tex.Left + tex.Top*1024 + x*2 + 1024*y] = palette[texturechunk.Data[(tex.Left/2 + x) + (tex.Top + y)*512] & 0xF];
                                texturepages[page][tex.Left + tex.Top*1024 + x*2 + 1024*y+1] = palette[texturechunk.Data[(tex.Left/2 + x) + (tex.Top + y)*512] >> 4 & 0xF];
                            }
                        }
                    }
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            // get rid of unused textures
            HashSet<int> usedids = new HashSet<int>();
            foreach (int id in texturebucket.Values)
            {
                usedids.Add(id);
            }
            for (int i = 0; i < texturepages.Length; ++i)
            {
                if (!usedids.Contains(i))
                {
                    GL.DeleteTexture(textureIDs[list][i]);
                    textureIDs[list][i] = 0;
                    continue;
                }
                //Bitmap bmp = new Bitmap(texturepages[i].Length / 128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //unsafe
                //{
                //    for (int j = 0; j < texturepages[i].Length / 128 * 128; ++j)
                //    {
                //        *((int*)data.Scan0.ToPointer() + j) = texturepages[i][j];
                //    }
                //}
                GL.BindTexture(TextureTarget.Texture2D, textureIDs[list][i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, texturepages[i].Length/128, 128, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texturepages[i]);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                //string filename = $"tex_{texturechunks[i/2].EName}_{(i%2 == 0 ?"4":"8")}bpp_{textureIDs[list][i]}.png";
                //bmp.Save(/*YOURPATHHERE*/ + filename, ImageFormat.Png);
                //bmp.UnlockBits(data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private long GenerateTextureHash(int tpag, OldModelTexture tex) // compresses a model texture's relevant texture info into a standard type that can be quickly looked up
        {
            return (long)tex.ClutY
                | (long)tex.ClutX << 7
                | (long)tpag << 11
                | (long)tex.Left << 14
                | (long)tex.Top << 24
                | (long)tex.Width << 31
                | (long)tex.Height << 41
                | (tex.BitFlag ? 1L : 0L) << 48;
        }

        protected void ConvertTexturesToGL(int list, TextureChunk[] texturechunks, IList<OldSceneryPolygon> oldscenerypolygons, IList<OldModelStruct> oldmodelstructs, byte[] eid_list, int eid_off)
        {
            textureIDs[list] = new int[texturechunks.Length * 2];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturepages[i * 2 + 0] = new int[1024 * 128]; // 4bpp
                texturepages[i * 2 + 1] = new int[512 * 128]; // 8bpp
            }
            textures[list] = new int[oldscenerypolygons.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < oldscenerypolygons.Count; ++i)
            {
                OldSceneryPolygon poly = oldscenerypolygons[i];
                OldModelStruct modelstruct = oldmodelstructs[poly.ModelStruct];
                if (modelstruct is OldModelColor || modelstruct == null)
                    continue;
                OldModelTexture tex = (OldModelTexture)modelstruct;
                long hash = GenerateTextureHash(poly.Page,tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    TextureChunk texturechunk = null;
                    foreach (TextureChunk chunk in texturechunks)
                    {
                        if (chunk.EID == BitConv.FromInt32(eid_list, eid_off + poly.Page*4))
                        {
                            texturechunk = chunk;
                            break;
                        }
                    }
                    if (texturechunk == null) throw new Exception("ConvertTexturesToGL: Texture chunk not found");
                    int w = tex.Width + 1;
                    int h = tex.Height + 1;
                    int page = poly.Page * 2 + Convert.ToInt32(tex.BitFlag);
                    if (tex.BitFlag) // 8-bit
                    {
                        int[] palette = new int[256];
                        for (int j = 0; j < 256; ++j) // copy palette
                        {
                            palette[j] = PixelConv.Convert5551_8888_Old(BitConv.FromInt16(texturechunk.Data,tex.ClutX*32+tex.ClutY*512+j*2),tex.BlendMode);
                        }
                        for (int y = 0; y < h; ++y) // copy pixel data
                        {
                            for (int x = 0; x < w; ++x)
                            {
                                texturepages[page][tex.Left+tex.Top*512+x+512*y] = palette[texturechunk.Data[(tex.Left+x) + (tex.Top+y) * 512]];
                            }
                        }
                    }
                    else // 4-bit
                    {
                        int[] palette = new int[16];
                        for (int j = 0; j < 16; ++j) // copy palette
                        {
                            palette[j] = PixelConv.Convert5551_8888_Old(BitConv.FromInt16(texturechunk.Data,tex.ClutX*32+tex.ClutY*512+j*2),tex.BlendMode);
                        }
                        for (int y = 0; y < h; ++y) // copy pixels
                        {
                            for (int x = 0; x < w / 2; ++x) // 2 pixels per byte
                            {
                                texturepages[page][tex.Left+tex.Top*1024+x*2+1024*y] = palette[texturechunk.Data[(tex.Left/2+x) + (tex.Top+y)*512] & 0xF];
                                texturepages[page][tex.Left+tex.Top*1024+x*2+1024*y+1] = palette[texturechunk.Data[(tex.Left/2+x) + (tex.Top+y)*512] >> 4 & 0xF];
                            }
                        }
                    }
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            // get rid of unused textures
            HashSet<int> usedids = new HashSet<int>();
            foreach (int id in texturebucket.Values)
            {
                usedids.Add(id);
            }
            for (int i = 0; i < texturepages.Length; ++i)
            {
                if (!usedids.Contains(i))
                {
                    GL.DeleteTexture(textureIDs[list][i]);
                    textureIDs[list][i] = 0;
                    continue;
                }
                //Bitmap bmp = new Bitmap(texturepages[i].Length / 128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //unsafe
                //{
                //    for (int j = 0; j < texturepages[i].Length / 128 * 128; ++j)
                //    {
                //        *((int*)data.Scan0.ToPointer() + j) = texturepages[i][j];
                //    }
                //}
                GL.BindTexture(TextureTarget.Texture2D, textureIDs[list][i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, texturepages[i].Length/128, 128, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texturepages[i]);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                //string filename = $"tex_{texturechunks[i/2].EName}_{(i%2 == 0 ?"4":"8")}bpp_{textureIDs[list][i]}.png";
                //bmp.Save(/*YOURPATHHERE*/ + filename, ImageFormat.Png);
                //bmp.UnlockBits(data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        
        protected void ConvertTexturesToGL(int list, TextureChunk[] texturechunks, IList<ProtoSceneryPolygon> protoscenerypolygons, IList<OldModelStruct> oldmodelstructs, byte[] eid_list, int eid_off)
        {
            textureIDs[list] = new int[texturechunks.Length * 2];
            GL.GenTextures(textureIDs[list].Length, textureIDs[list]);
            int[][] texturepages = new int[textureIDs[list].Length][]; // using indexed colors in GL would be dumb so we will convert each texture chunk into two 32-bit pages
            for (int i = 0; i < texturechunks.Length; ++i)
            {
                texturepages[i * 2 + 0] = new int[1024 * 128]; // 4bpp
                texturepages[i * 2 + 1] = new int[512 * 128]; // 8bpp
            }
            textures[list] = new int[protoscenerypolygons.Count];
            Dictionary<long, int> texturebucket = new Dictionary<long, int>();
            for (int i = 0; i < protoscenerypolygons.Count; ++i)
            {
                ProtoSceneryPolygon poly = protoscenerypolygons[i];
                OldModelStruct modelstruct = oldmodelstructs[poly.Texture];
                if (modelstruct is OldModelColor || modelstruct == null)
                    continue;
                OldModelTexture tex = (OldModelTexture)modelstruct;
                long hash = GenerateTextureHash(poly.Page,tex);
                if (!texturebucket.ContainsKey(hash))
                {
                    TextureChunk texturechunk = null;
                    foreach (TextureChunk chunk in texturechunks)
                    {
                        if (chunk.EID == BitConv.FromInt32(eid_list, eid_off + poly.Page*4))
                        {
                            texturechunk = chunk;
                            break;
                        }
                    }
                    if (texturechunk == null) throw new Exception("ConvertTexturesToGL: Texture chunk not found");
                    int w = tex.Width + 1;
                    int h = tex.Height + 1;
                    int page = poly.Page * 2 + Convert.ToInt32(tex.BitFlag);
                    if (tex.BitFlag) // 8-bit
                    {
                        int[] palette = new int[256];
                        for (int j = 0; j < 256; ++j) // copy palette
                        {
                            palette[j] = PixelConv.Convert5551_8888_Old(BitConv.FromInt16(texturechunk.Data,tex.ClutX*32+tex.ClutY*512+j*2),tex.BlendMode);
                        }
                        for (int y = 0; y < h; ++y) // copy pixel data
                        {
                            for (int x = 0; x < w; ++x)
                            {
                                texturepages[page][tex.Left+tex.Top*512+x+512*y] = palette[texturechunk.Data[(tex.Left+x) + (tex.Top+y) * 512]];
                            }
                        }
                    }
                    else // 4-bit
                    {
                        int[] palette = new int[16];
                        for (int j = 0; j < 16; ++j) // copy palette
                        {
                            palette[j] = PixelConv.Convert5551_8888_Old(BitConv.FromInt16(texturechunk.Data,tex.ClutX*32+tex.ClutY*512+j*2),tex.BlendMode);
                        }
                        for (int y = 0; y < h; ++y) // copy pixels
                        {
                            for (int x = 0; x < w / 2; ++x) // 2 pixels per byte
                            {
                                texturepages[page][tex.Left+tex.Top*1024+x*2+1024*y] = palette[texturechunk.Data[(tex.Left/2+x) + (tex.Top+y)*512] & 0xF];
                                texturepages[page][tex.Left+tex.Top*1024+x*2+1024*y+1] = palette[texturechunk.Data[(tex.Left/2+x) + (tex.Top+y)*512] >> 4 & 0xF];
                            }
                        }
                    }
                    texturebucket[hash] = page;
                }
                textures[list][i] = texturebucket[hash];
            }
            // get rid of unused textures
            HashSet<int> usedids = new HashSet<int>();
            foreach (int id in texturebucket.Values)
            {
                usedids.Add(id);
            }
            for (int i = 0; i < texturepages.Length; ++i)
            {
                if (!usedids.Contains(i))
                {
                    GL.DeleteTexture(textureIDs[list][i]);
                    textureIDs[list][i] = 0;
                    continue;
                }
                //Bitmap bmp = new Bitmap(texturepages[i].Length / 128, 128, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //unsafe
                //{
                //    for (int j = 0; j < texturepages[i].Length / 128 * 128; ++j)
                //    {
                //        *((int*)data.Scan0.ToPointer() + j) = texturepages[i][j];
                //    }
                //}
                GL.BindTexture(TextureTarget.Texture2D, textureIDs[list][i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, texturepages[i].Length/128, 128, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texturepages[i]);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                //string filename = $"tex_{texturechunks[i/2].EName}_{(i%2 == 0 ?"4":"8")}bpp_{textureIDs[list][i]}.png";
                //bmp.Save(/*YOURPATHHERE*/ + filename, ImageFormat.Png);
                //bmp.UnlockBits(data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
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
