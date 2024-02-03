using Crash;
using CrashEdit.Properties;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace CrashEdit
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

        private static void GLLoadStatic()
        {
            // version print
            Console.WriteLine($"OpenGL version: {GL.GetString(StringName.Version)}");

            int max_tex_size = GL.GetInteger(GetPName.MaxTextureSize);
            Console.WriteLine($"Maximum texture size: {max_tex_size}");

            int flags = GL.GetInteger(GetPName.ContextFlags);
            // Console.WriteLine($"flags: {flags}");
            if ((flags & (int)ContextFlagMask.ContextFlagDebugBit) != 0)
            {
                Console.WriteLine("GL debug enabled.");
                // Enable debug callbacks.
                GL.Enable(EnableCap.DebugOutput);
                GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
                {
                    string msg = Marshal.PtrToStringAnsi(message);
                    string level = "OTHER";
                    switch (severity)
                    {
                        case DebugSeverity.DebugSeverityHigh:
                            level = "ERROR";
                            break;
                        case DebugSeverity.DebugSeverityMedium:
                            level = "WARN";
                            break;
                        case DebugSeverity.DebugSeverityLow:
                            level = "INFO";
                            break;
                    }
                    Console.WriteLine($"[{dbgContextString}] OpenGL {level}: {msg}");
                }, IntPtr.Zero);
            }

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // init all shaders
            shaderContext = new();

            // init vertex array objects
            vaoAxes = new VAO(shaderContext, "axes", PrimitiveType.Lines, vert_count: AxesPos.Length);
            vaoSphereLine = new VAO(shaderContext, "line-debug", PrimitiveType.LineStrip);
            vaoGridLine = new VAO(shaderContext, "line-debug-usercolor", PrimitiveType.Lines);
            vaoDebugBoxTri = new VAO(shaderContext, "box-debug", PrimitiveType.Triangles, vert_count: BoxTriIndices.Length);
            vaoDebugBoxLine = new VAO(shaderContext, "box-debug", PrimitiveType.Lines, vert_count: BoxLineIndices.Length);
            vaoDebugSprite = new VAO(shaderContext, "sprite-debug", PrimitiveType.TriangleFan, vert_count: SpriteVerts.Length);
            vaoSprites = new VAO(shaderContext, "sprite", PrimitiveType.Triangles);
            vaoLines = new VAO(shaderContext, "line", PrimitiveType.Lines);
            vaoLinesThick = new VAO(shaderContext, "line", PrimitiveType.Lines);
            vaoTris = new VAO(shaderContext, "generic", PrimitiveType.Triangles);
            vaoText = new VAO(shaderContext, "screen", PrimitiveType.Triangles);
            vaoOctree = new VAO(shaderContext, "octree", PrimitiveType.Triangles);
            for (int i = 0; i < ANIM_BUF_MAX; ++i)
            {
                vaoListCrash1[i] = new(shaderContext, "crash1", PrimitiveType.Triangles);
            }

            vaoGridLine.UserColor1 = Color4.Gray;
            vaoText.ZBufDisable = true;
            vaoLinesThick.LineWidth = 5;

            for (int i = 0; i < AxesPos.Length; ++i)
            {
                vaoAxes.PushAttrib(trans: AxesPos[i]);
            }
            for (int i = 0; i < BoxTriIndices.Length; ++i)
            {
                vaoDebugBoxTri.PushAttrib(trans: BoxVerts[BoxTriIndices[i]]);
            }
            for (int i = 0; i < BoxLineIndices.Length; ++i)
            {
                vaoDebugBoxLine.PushAttrib(trans: BoxVerts[BoxLineIndices[i]]);
            }

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
            texFont = fontTable.LoadFontAndLoadToGL(GL.GenTexture(), fontLib, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), Settings.Default.FontName), Settings.Default.FontSize);
        }

        protected override void GLLoad()
        {
            base.GLLoad();

            render.StopDisplay();
            render.StopLogic();
        }
    }
}
