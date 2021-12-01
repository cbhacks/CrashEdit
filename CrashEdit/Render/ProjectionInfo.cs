using OpenTK;

namespace CrashEdit
{
    public struct ProjectionInfo
    {
        public Vector3 Trans;
        public Vector3 Rot;
        public Vector3 Scale;

        public Matrix4 Perspective;
        public Matrix4 View;

        public float Width;
        public float Height;

        public float Aspect => Width / Height;

        public const float Aspect4x3 = 4f / 3f;
        public const float Aspect16x9 = 16f / 9f;
    }
}
