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
            Matrix4 pvm = ri.Projection.View * ri.Projection.Perspective;
            sh.UniformMat4("PVM", ref pvm);
            var trans = ri.Projection.RealTrans;
            sh.UniformVec3("viewTrans", ref trans);

            sh.UniformVec3("trans", ref vao.UserTrans);
            sh.UniformVec3("scale", ref vao.UserScale);
            sh.UniformVec4("userColor1", ref vao.UserColor1);
            sh.UniformVec4("userColor2", ref vao.UserColor2);
            sh.UniformInt("modeColor", (int)vao.ColorMode);
            sh.UniformInt("modeCull", vao.UserCullMode);
            sh.UniformInt("blendmask", vao.BlendMask);

            sh.UniformBool("disableDepth", vao.ZBufDisableWrite);
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

        internal static void RenderSprite(Shader sh, RenderInfo ri, VAO vao)
        {
            Vector3 viewColumn0 = ri.Projection.View.Column0.Xyz;
            Vector3 viewColumn1 = ri.Projection.View.Column1.Xyz;
            sh.UniformVec3("viewColumn0", ref viewColumn0);
            sh.UniformVec3("viewColumn1", ref viewColumn1);
        }
    }
}
