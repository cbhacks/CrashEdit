using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Runtime.InteropServices;

namespace CrashEdit
{
    public delegate void ApplyVAOFunc(VAO vao);

    public class MultiPassVAO : IDisposable
    {
        private VAO[] vaoPasses = new VAO[4];
        private GLViewer.BlendMode blendMask;

        private VAO GetVAO(int i)
        {
            return vaoPasses[GLViewer.BlendModeIndex(BlendPassOrder[i])];
        }

        internal readonly GLViewer.BlendMode[] BlendPassOrder = {
            GLViewer.BlendMode.Solid,
            GLViewer.BlendMode.Trans,
            GLViewer.BlendMode.Subtractive,
            GLViewer.BlendMode.Additive
        };

        public MultiPassVAO(GLViewer.BlendMode mask, ShaderContext shaders, string shadername, PrimitiveType prim, int vert_count = 10000)
        {
            for (int i = 0; i < vaoPasses.Length; ++i)
            {
                if (((1 << i) & (int)mask) != 0)
                {
                    vaoPasses[i] = new VAO(shaders, shadername, prim, vert_count);
                    vaoPasses[i].BlendMask = i;
                }
            }

            blendMask = mask;
        }

        public void PushAttrib(GLViewer.BlendMode mask, Vector3? trans = null, Vector3? normal = null, Vector2? st = null, Rgba? rgba = null, TexInfoUnpacked? tex = null)
        {
            mask &= blendMask;
            for (int i = 0; i < vaoPasses.Length; ++i)
            {
                if (((1 << i) & (int)mask) != 0)
                {
                    vaoPasses[i].PushAttrib(trans, normal, st, rgba, tex);
                }
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < vaoPasses.Length; ++i)
            {
                VAO vao = GetVAO(i);
                vao?.Dispose();
            }
        }

        public void Render(RenderInfo ri)
        {
            for (int i = 0; i < vaoPasses.Length; ++i)
            {
                VAO vao = GetVAO(i);
                if (vao != null)
                {
                    GLViewer.SetBlendMode((GLViewer.BlendMode)(1 << vao.BlendMask));
                    vao.Render(ri);
                }
            }
        }

        public void DiscardVerts()
        {
            for (int i = 0; i < vaoPasses.Length; ++i)
            {
                VAO vao = GetVAO(i);
                vao?.DiscardVerts();
            }
        }

        public void RenderAndDiscard(RenderInfo ri)
        {
            Render(ri);
            DiscardVerts();
        }
        public void ForEachVAO(ApplyVAOFunc f)
        {

            for (int i = 0; i < vaoPasses.Length; ++i)
            {
                if (vaoPasses[i] != null)
                {
                    f(vaoPasses[i]);
                }
            }
        }
    }
}
