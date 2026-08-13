namespace CrashEdit.Crash
{
    public sealed class FontGroup(List<FontTexture> frames, int eid) : GOOLFrameGroup<FontTexture>(frames, eid)
    {
        public override short Type() => 3;

        public static FontGroup Load(byte[] data, ref int index)
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

            List<FontTexture> frames = new();
            for (int i = 0; i < framecount; ++i)
            {
                frames.Add(new(BitConv.FromInt32(data, index), BitConv.FromInt32(data, index + 4), BitConv.FromInt16(data, index + 8), BitConv.FromInt16(data, index + 10)));
                index += 12;
            }

            return new FontGroup(frames, eid);
        }

        public override byte[] Save()
        {
            byte[] data = new byte[8 + 12 * FrameCount];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            for (int i = 0; i < FrameCount; ++i)
            {
                BitConv.ToInt32(data, 8 + i * 12, Frames[i].PackedValue1);
                BitConv.ToInt32(data, 8 + i * 12 + 4, Frames[i].PackedValue2);
                BitConv.ToInt16(data, 8 + i * 12 + 8, Frames[i].Width);
                BitConv.ToInt16(data, 8 + i * 12 + 10, Frames[i].Height);
            }
            return data;
        }
    }
}
