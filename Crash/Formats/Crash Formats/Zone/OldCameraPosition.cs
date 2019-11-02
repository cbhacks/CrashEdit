namespace Crash
{
    public struct OldCameraPosition : IPosition
    {
        public OldCameraPosition(short x,short y,short z,short xrot,short yrot,short zrot)
        {
            X = x;
            Y = y;
            Z = z;
            XRot = xrot;
            YRot = yrot;
            ZRot = zrot;
        }

        public short X { get; }
        public short Y { get; }
        public short Z { get; }
        public short XRot { get; }
        public short YRot { get; }
        public short ZRot { get; }
        double IPosition.X => X;
        double IPosition.Y => Y;
        double IPosition.Z => Z;
    }
}
