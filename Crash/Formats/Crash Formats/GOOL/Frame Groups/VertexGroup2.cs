namespace CrashEdit.Crash
{
    public sealed class VertexGroup2(bool lerp, short frames, int eid) : GOOLChunkFrameGroup(eid)
    {
        public override short Type() => 1;

        public static VertexGroup2 Load(byte[] data, ref int index)
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

            int lerp = BitConv.FromInt32(data, index);
            index += 4;

            return new VertexGroup2(lerp != 0, frames, eid);
        }

        public short FrameCount { get; set; } = frames;
        public bool Interpolated { get; set; } = lerp;

        public override byte[] Save()
        {
            byte[] data = new byte[12];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            BitConv.ToInt32(data, 8, Interpolated ? 1 : 0);
            return data;
        }
    }
}
