using OpenTK;

namespace CrashEdit
{
    public struct ProjectionInfo
    {
        public Vector3 Trans;
        public Quaternion Rot;
        public Vector3 Scale;
        public Vector3 Forward;
        public Matrix4 RotMat;

        public Vector3 CamTrans;
        public Vector3 CamRot;

        public Matrix4 Perspective;
        public Matrix4 View;

        public const float MinDistance = GLViewer.DefaultZNear;
        public const float MaxInitialDistance = 100;
        public const float MaxDistance = MaxInitialDistance * 2;
        public const float InitialDistance = 5;
        public const float Aspect4x3 = 4f / 3f;
        public const float Aspect8x5 = 8f / 5f;
        public const float Aspect16x9 = 16f / 9f;

        public float Distance { get; set; }

        public float Width;
        public float Height;

        public readonly float Aspect => Width / Height;

        public readonly Vector3 Backward => -Forward;

        public readonly Matrix4 GetPVM() => View * Perspective;

        public void SetPerspective(float fov, float aspect, float znear, float zfar)
        {
            Perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspect, znear, zfar);
        }

        public void SetRotation(Vector3 euler)
        {
            Rot = new Quaternion(euler);
            RotMat = Matrix4.CreateFromQuaternion(Rot);
            Forward = -(RotMat * new Vector4(0, 0, 1, 1)).Xyz;
            View = Matrix4.CreateTranslation(Trans) * RotMat;
        }

        public void SetTrans(Vector3 trans)
        {
            Trans = -trans;
            View = Matrix4.CreateTranslation(Trans) * RotMat;
        }
    }
}
