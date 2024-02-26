using CrashEdit.CE.Properties;
using CrashEdit.Crash;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using System.Drawing.Imaging;

namespace CrashEdit.CE
{
    public sealed class GLViewerLoader : GLViewer
    {
        public GLViewerLoader() : base()
        {
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                yield return new Position(0, 0, 0);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (default_graphics_settings.SharedContext == null)
            {
                default_graphics_settings.SharedContext = Context as IGLFWGraphicsContext;
                bool success = default_graphics_settings.SharedContext != null;
                Console.WriteLine($"Created GLViewer loader: {success}");

                if (success)
                {
                    render.StopDisplay();
                    render.StopLogic();
                    MakeCurrent();
                    InitGL();
                }
            }
        }

        private void InitGL()
        {
            // version print
            Console.WriteLine($"OpenGL version: {GL.GetString(StringName.Version)}");

            int max_tex_size = GL.GetInteger(GetPName.MaxTextureSize);
            Console.WriteLine($"Maximum texture size: {max_tex_size}");

            Console.WriteLine("Checking OpenGL caps...");
            foreach (var cap in Enum.GetValues<EnableCap>())
            {
                Console.WriteLine($"    {cap}: {GL.IsEnabled(cap)}");
            }

            EnableGLDebug();

            // init all shaders
            shaders = new();

            // make texture
            texTpages = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texTpages);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8ui, 512, 0, 0, OpenTK.Graphics.OpenGL4.PixelFormat.RedInteger, PixelType.UnsignedByte, IntPtr.Zero);

            // make texture for sprites
            texSprites = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, texSprites);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            BitmapData data = OldResources.AllTex.LockBits(new Rectangle(Point.Empty, OldResources.AllTex.Size), ImageLockMode.ReadOnly, OldResources.AllTex.PixelFormat);
            try
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8ui, OldResources.AllTex.Width, OldResources.AllTex.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.BgraInteger, PixelType.UnsignedByte, data.Scan0);
            }
            catch
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
                Console.WriteLine("Error making texture.");
            }
            finally
            {
                OldResources.AllTex.UnlockBits(data);
            }

            // make texture for font
            fontTable = new();
            texFont = fontTable.LoadFontAndUpload(GL.GenTexture(), fontLib, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), Settings.Default.FontName), Settings.Default.FontSize);
        }
    }
}
