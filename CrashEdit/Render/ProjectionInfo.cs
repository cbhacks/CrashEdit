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
        public const float MaxInitialDistance = 100;
        public const float MaxDistance = MaxInitialDistance * 2;
        public const float InitialDistance = 5;
        public float Distance { get; set; }

        public float Width;
        public float Height;

        public readonly float Aspect => Width / Height;

        public readonly Matrix4 PVM => View * Perspective;
        public readonly Vector3 RealTrans => Trans - Backward * Distance;
        public readonly Vector3 Backward => -Forward;

        public const float Aspect4x3 = 4f / 3f;
        public const float Aspect16x9 = 16f / 9f;
    }
}
