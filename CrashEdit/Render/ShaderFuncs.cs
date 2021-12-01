using OpenTK;
using System;

namespace CrashEdit
{
    public partial class ShaderInfo
    {
        public delegate void ShaderRenderFunc(Shader sh, RenderInfo ri, VAO vao);

        internal static void RenderDefault(Shader sh, RenderInfo ri, VAO vao)
        {
        }

        internal static void PreRenderDefault(Shader sh, RenderInfo ri, VAO vao)
        {
            sh.UniformMat4("projectionMatrix", ref ri.Projection.Perspective);
            sh.UniformMat4("viewMatrix", ref ri.Projection.View);
            sh.UniformVec3("viewTrans", ref ri.Projection.Trans);

            sh.UniformVec3("trans", ref vao.UserTrans);
            sh.UniformVec3("scale", ref vao.UserScale);
            sh.UniformVec4("userColor1", ref vao.UserColor1);
            sh.UniformVec4("userColor2", ref vao.UserColor2);
            sh.UniformInt("modeColor", (int)vao.ColorMode);
            sh.UniformInt("modeCull", vao.UserCullMode);
            sh.UniformFloat("scaleScalar", vao.UserScaleScalar);
            sh.UniformInt("art", (int)vao.ArtType);
            sh.UniformInt("blendmask", vao.BlendMask);

            sh.UniformBool("enableTex", ri.EnableTexture);
        }

        internal static void RenderTest(Shader sh, RenderInfo ri, VAO vao)
        {
            Matrix4 model = Matrix4.CreateScale(2) * Matrix4.CreateTranslation(0, (float)Math.Sin(ri.CurrentFrame / 60f * Math.PI / 2) * 0.25f, -5);

            sh.UniformMat4("modelMatrix", ref model);
        }

        internal static void RenderLineModel(Shader sh, RenderInfo ri, VAO vao)
        {
            Matrix4 model = Matrix4.CreateFromAxisAngle(vao.UserAxis.Xyz, vao.UserAxis.W);

            sh.UniformMat4("modelMatrix", ref model);
        }
    }
}
