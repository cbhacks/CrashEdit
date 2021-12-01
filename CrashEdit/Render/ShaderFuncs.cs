using OpenTK;
using System;

namespace CrashEdit
{
    public partial class ShaderInfo
    {
        internal static void RenderDefault(Shader sh, RenderInfo ri)
        {
        }

        internal static void PreRenderDefault(Shader sh, RenderInfo ri)
        {
            sh.UniformMat4("projectionMatrix", ref ri.Projection.Perspective);
            sh.UniformMat4("viewMatrix", ref ri.Projection.View);
            sh.UniformVec3("viewTrans", ref ri.Projection.Trans);

            sh.UniformVec3("trans", ref ri.Projection.UserTrans);
            sh.UniformVec3("scale", ref ri.Projection.UserScale);
            sh.UniformVec4("userColor1", ref ri.Projection.UserColor1);
            sh.UniformVec4("userColor2", ref ri.Projection.UserColor2);
            sh.UniformInt("colorMode", (int)ri.Projection.ColorMode);

            sh.UniformBool("enableTex", ri.EnableTexture);
            sh.UniformBool("blendmask", ri.BlendMask);
        }

        internal static void RenderTest(Shader sh, RenderInfo ri)
        {
            Matrix4 model = Matrix4.CreateScale(2) * Matrix4.CreateTranslation(0, (float)Math.Sin(ri.CurrentFrame / 60f * Math.PI / 2) * 0.25f, -5);

            sh.UniformMat4("modelMatrix", ref model);
        }

        internal static void RenderLineModel(Shader sh, RenderInfo ri)
        {
            Matrix4 model = Matrix4.CreateFromAxisAngle(ri.Projection.UserAxis.Xyz, ri.Projection.UserAxis.W);

            sh.UniformMat4("modelMatrix", ref model);
        }

        internal static void RenderC1Anim(Shader sh, RenderInfo ri)
        {
            ri.Projection.UserScale /= 3200;
            ri.Projection.UserScale /= GameScales.AnimC1;
            sh.UniformVec3("modelScale", ref ri.Projection.UserScale);
            sh.UniformInt("cullmode", ri.Projection.UserInt1);
        }
    }
}
