namespace Crash
{
    public struct OldCameraNeighbor
    {
        public OldCameraNeighbor(byte linktype,byte zoneindex,byte cameraindex,byte flag)
        {
            LinkType = linktype;
            ZoneIndex = zoneindex;
            CameraIndex = cameraindex;
            Flag = flag;
        }

        public byte LinkType { get; set; }
        public byte ZoneIndex { get; set; }
        public byte CameraIndex { get; set; }
        public byte Flag { get; set; }
    }
}
