namespace CrashEdit.Crash
{
    public sealed class VertexGroup3(bool lerp, int num, short frames, int model, int eid) : GOOLFrameGroupWithChunk(eid)
    {
        public override short Type() => 1;

        public static VertexGroup3 Load(byte[] data, ref int index)
        {
            if (BitConv.FromInt16(data, index) != 1)
            {
                ErrorManager.SignalError("Vertex frame group version is wrong");
            }
            index += 2;

            short frames = BitConv.FromInt16(data, index);
            index += 2;

            int lerp = BitConv.FromInt32(data, index);
            index += 4;

            int num = BitConv.FromInt32(data, index);
            index += 4;

            int model = BitConv.FromInt32(data, index);
            index += 4;

            int eid = BitConv.FromInt32(data, index);
            index += 4;

            return new VertexGroup3(lerp != 0, num, frames, model, eid);
        }

        public short FrameCount { get; set; } = frames;
        public bool Interpolated { get; set; } = lerp;
        public int Number { get; set; } = num;
        public int ModelEID { get; set; } = model;

        public override byte[] Save()
        {
            byte[] data = new byte[20];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, Interpolated ? 1 : 0);
            BitConv.ToInt32(data, 8, Number);
            BitConv.ToInt32(data, 12, ModelEID);
            BitConv.ToInt32(data, 16, EID);
            return data;
        }
    }
}
