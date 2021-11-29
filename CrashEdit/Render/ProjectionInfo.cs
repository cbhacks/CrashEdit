using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace CrashEdit
{
    public struct ProjectionInfo
    {
        public Vector3 Trans;
        public Vector3 Rot;
        public Vector3 Scale;

        public Matrix4 Perspective;
        public Matrix4 View;

        // these can be whatever you want
        public Vector3 UserTrans;
        public Vector3 UserRot;
        public Vector3 UserScale;
        public Quaternion UserQuat;
        public Vector4 UserAxis;
        public Color4 UserColor1;
        public Color4 UserColor2;
        public Matrix3 UserMat3;
        public Vector3 UserColorAmb;
        public Vector3 UserColorDiff;
        public int UserInt1;
        public bool UserBool1;

        public enum ColorModeEnum { Default = 0, GradientY = 1, Solid = 2 };
        public ColorModeEnum ColorMode;
        public Stack<ColorModeEnum> ColorModeStack;
        public void PushColorMode(ColorModeEnum mode)
        {
            ColorModeStack.Push(ColorMode);
            ColorMode = mode;
        }
        public void PopColorMode()
        {
            ColorMode = ColorModeStack.Pop();
        }

        public float Width;
        public float Height;

        public float Aspect => Width / Height;

        public const float Aspect4x3 = 4f / 3f;
        public const float Aspect16x9 = 16f / 9f;
    }
}
