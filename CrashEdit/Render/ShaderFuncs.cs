using OpenTK.Mathematics;

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
            Matrix4 pvm = ri.Projection.GetPVM();
            sh.UniformMat4("PVM", ref pvm);
            sh.UniformVec3("viewTrans", ref ri.Projection.Trans);

            sh.UniformVec3("trans", ref vao.UserTrans);
            sh.UniformVec3("scale", ref vao.UserScale);
            sh.UniformInt("modeCull", vao.UserCullMode);
            sh.UniformInt("blendmask", vao.BlendMask);

            sh.UniformBool("disableDepth", vao.ZBufDisableWrite);
            sh.UniformBool("enableTex", ri.EnableTexture);
        }

        internal static void RenderOctree(Shader sh, RenderInfo ri, VAO vao)
        {
            sh.UniformFloat("uNodeShadeMax", vao.UserFloat);
            sh.UniformFloat("uNodeAlpha", vao.UserFloat2);
        }
    }
}
