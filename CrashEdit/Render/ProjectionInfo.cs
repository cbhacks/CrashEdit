using OpenTK;

namespace CrashEdit
{
    public struct ProjectionInfo
    {
        public Vector3 Trans;
        public Vector3 Rot;
        public Vector3 Scale;
        public Vector3 Forward;

        public Matrix4 Perspective;
        public Matrix4 View;

        public const float MinDistance = GLViewer.DefaultZNear;
        public const float MaxDistance = 100;
        public const float InitialDistance = MaxDistance * 0.05f;
        public float Distance { get; set; }

        public float Width;
        public float Height;

        public float Aspect => Width / Height;

        public Matrix4 PVM => View * Perspective;
        public Vector3 RealTrans => Trans - Backward * Distance;
        public Vector3 Backward => -Forward;

        public const float Aspect4x3 = 4f / 3f;
        public const float Aspect16x9 = 16f / 9f;
    }
}
