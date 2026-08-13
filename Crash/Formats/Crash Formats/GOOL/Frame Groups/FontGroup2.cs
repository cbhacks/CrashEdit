namespace CrashEdit.Crash
{
    public sealed class FontGroup2(List<FontTexture2> frames, int eid) : GOOLFrameGroup<FontTexture2>(frames, eid)
    {
        public override short Type() => 3;

        public static FontGroup2 Load(byte[] data, ref int index)
        {
            if (BitConv.FromInt16(data, index) != 3)
            {
                ErrorManager.SignalError("Font group version is wrong");
            }
            index += 2;

            short framecount = BitConv.FromInt16(data, index);
            index += 2;

            int eid = BitConv.FromInt32(data, index);
            index += 4;

            List<FontTexture2> frames = new();
            for (int i = 0; i < framecount; ++i)
            {
                frames.Add(new(BitConv.FromInt32(data, index), BitConv.FromInt32(data, index + 4), BitConv.FromInt32(data, index + 8), BitConv.FromInt32(data, index + 12), BitConv.FromInt16(data, index + 16), BitConv.FromInt16(data, index + 18)));
                index += 20;
            }

            return new FontGroup2(frames, eid);
        }

        public override byte[] Save()
        {
            byte[] data = new byte[8 + 20 * FrameCount];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            for (int i = 0; i < FrameCount; ++i)
            {
                BitConv.ToInt32(data, 8 + i * 20, Frames[i].PackedValue1);
                BitConv.ToInt32(data, 8 + i * 20 + 4, Frames[i].PackedValue2);
                BitConv.ToInt32(data, 8 + i * 20 + 8, Frames[i].PackedValue3);
                BitConv.ToInt32(data, 8 + i * 20 + 12, Frames[i].PackedValue4);
                BitConv.ToInt16(data, 8 + i * 20 + 16, Frames[i].Width);
                BitConv.ToInt16(data, 8 + i * 20 + 18, Frames[i].Height);
            }
            return data;
        }
    }
}
