namespace CrashEdit.Crash
{
    public sealed class ImageGroup(List<List<ImageTexture>> frames, int eid) : GOOLFrameGroup<List<ImageTexture>>(frames, eid)
    {
        public override short Type() => 5;

        public static ImageGroup Load(byte[] data, ref int index)
        {
            if (BitConv.FromInt16(data, index) != 5)
            {
                ErrorManager.SignalError("Image frame group version is wrong");
            }
            index += 2;

            short framecount = BitConv.FromInt16(data, index);
            index += 2;

            int eid = BitConv.FromInt32(data, index);
            index += 4;

            int partcount = BitConv.FromInt32(data, index);
            index += 4;

            List<List<ImageTexture>> frames = new();
            for (int i = 0; i < framecount; ++i)
            {
                List<ImageTexture> parts = new();
                frames.Add(parts);
                for (int j = 0; j < partcount; ++j)
                {
                    parts.Add(new(BitConv.FromInt32(data, index), BitConv.FromInt32(data, index + 4), BitConv.FromInt16(data, index + 8), BitConv.FromInt16(data, index + 10), BitConv.FromInt16(data, index + 12), BitConv.FromInt16(data, index + 14)));
                    index += 16;
                }
            }

            return new ImageGroup(frames, eid);
        }

        public override byte[] Save()
        {
            int parts = FrameCount > 0 ? Frames[0].Count : 0;
            int framesize = 16 * parts;
            byte[] data = new byte[12 + 16 * FrameCount * parts];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            BitConv.ToInt32(data, 8, parts);
            for (int i = 0; i < FrameCount; ++i)
            {
                var frame = Frames[i];
                for (int j = 0; j < parts; ++j)
                {
                    BitConv.ToInt32(data, 12 + framesize * i + j * 16, frame[j].PackedValue1);
                    BitConv.ToInt32(data, 12 + framesize * i + j * 16 + 4, frame[j].PackedValue2);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 16 + 8, frame[j].X1);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 16 + 10, frame[j].Y1);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 16 + 12, frame[j].X2);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 16 + 14, frame[j].Y2);
                }
            }
            return data;
        }
    }
}
