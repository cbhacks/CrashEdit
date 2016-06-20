namespace Crash
{
    public struct OldCameraPosition : IPosition
    {
        private short x;
        private short y;
        private short z;
        private short xrot;
        private short yrot;
        private short zrot;

        public OldCameraPosition(short x,short y,short z,short xrot,short yrot,short zrot)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.xrot = xrot;
            this.yrot = yrot;
            this.zrot = zrot;
        }

        public short X
        {
            get { return x; }
        }

        public short Y
        {
            get { return y; }
        }

        public short Z
        {
            get { return z; }
        }

        public short XRot
        {
            get { return xrot; }
        }

        public short YRot
        {
            get { return yrot; }
        }

        public short ZRot
        {
            get { return zrot; }
        }

        double IPosition.X
        {
            get { return x; }
        }

        double IPosition.Y
        {
            get { return y; }
        }

        double IPosition.Z
        {
            get { return z; }
        }
    }
}
