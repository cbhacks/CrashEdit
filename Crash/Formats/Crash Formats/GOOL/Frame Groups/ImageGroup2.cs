namespace CrashEdit.Crash
{
    public sealed class ImageGroup2(List<List<ImageTexture2>> frames, int eid) : GOOLFrameGroup<List<ImageTexture2>>(frames, eid)
    {
        public override short Type() => 5;

        public static ImageGroup2 Load(byte[] data, ref int index)
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

            List<List<ImageTexture2>> frames = new();
            for (int i = 0; i < framecount; ++i)
            {
                List<ImageTexture2> parts = new();
                frames.Add(parts);
                for (int j = 0; j < partcount; ++j)
                {
                    parts.Add(new(BitConv.FromInt32(data, index), BitConv.FromInt32(data, index + 4), BitConv.FromInt32(data, index + 8), BitConv.FromInt32(data, index + 12), BitConv.FromInt16(data, index + 16), BitConv.FromInt16(data, index + 18), BitConv.FromInt16(data, index + 20), BitConv.FromInt16(data, index + 22)));
                    index += 24;
                }
            }

            return new ImageGroup2(frames, eid);
        }

        public override byte[] Save()
        {
            int parts = FrameCount > 0 ? Frames[0].Count : 0;
            int framesize = 24 * parts;
            byte[] data = new byte[12 + 24 * FrameCount * parts];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            BitConv.ToInt32(data, 8, parts);
            for (int i = 0; i < FrameCount; ++i)
            {
                var frame = Frames[i];
                for (int j = 0; j < parts; ++j)
                {
                    BitConv.ToInt32(data, 12 + framesize * i + j * 24, frame[j].PackedValue1);
                    BitConv.ToInt32(data, 12 + framesize * i + j * 24 + 4, frame[j].PackedValue2);
                    BitConv.ToInt32(data, 12 + framesize * i + j * 24 + 8, frame[j].PackedValue3);
                    BitConv.ToInt32(data, 12 + framesize * i + j * 24 + 12, frame[j].PackedValue4);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 24 + 16, frame[j].X1);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 24 + 18, frame[j].Y1);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 24 + 20, frame[j].X2);
                    BitConv.ToInt16(data, 12 + framesize * i + j * 24 + 22, frame[j].Y2);
                }
            }
            return data;
        }
    }
}
