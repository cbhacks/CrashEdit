namespace CrashEdit.Crash
{
    public sealed class VertexGroup(short frames, int eid) : GOOLChunkFrameGroup(eid)
    {
        public override short Type() => 1;

        public static VertexGroup Load(byte[] data, ref int index)
        {
            if (BitConv.FromInt16(data, index) != 1)
            {
                ErrorManager.SignalError("Vertex frame group version is wrong");
            }
            index += 2;
            
            short frames = BitConv.FromInt16(data, index);
            index += 2;

            int eid = BitConv.FromInt32(data, index);
            index += 4;

            return new VertexGroup(frames, eid);
        }

        public short FrameCount { get; set; } = frames;

        public override byte[] Save()
        {
            byte[] data = new byte[8];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            return data;
        }
    }
}
