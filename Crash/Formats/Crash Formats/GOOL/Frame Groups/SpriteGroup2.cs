namespace CrashEdit.Crash
{
    public sealed class SpriteGroup2(List<SpriteTexture2> frames, int eid) : GOOLFrameGroup<SpriteTexture2>(frames, eid)
    {
        public override short Type() => 2;

        public static SpriteGroup2 Load(byte[] data, ref int index)
        {
            if (BitConv.FromInt16(data, index) != 2)
            {
                ErrorManager.SignalError("Sprite frame group version is wrong");
            }
            index += 2;
            
            short framecount = BitConv.FromInt16(data, index);
            index += 2;

            int eid = BitConv.FromInt32(data, index);
            index += 4;

            List<SpriteTexture2> frames = new();
            for (int i = 0; i < framecount; ++i)
            {
                frames.Add(new(BitConv.FromInt32(data, index), BitConv.FromInt32(data, index + 4), BitConv.FromInt32(data, index + 8), BitConv.FromInt32(data, index + 12)));
                index += 16;
            }

            return new SpriteGroup2(frames, eid);
        }

        public override byte[] Save()
        {
            byte[] data = new byte[8 + 16 * FrameCount];
            BitConv.ToInt16(data, 0, Type());
            BitConv.ToInt16(data, 2, FrameCount);
            BitConv.ToInt32(data, 4, EID);
            for (int i = 0; i < FrameCount; ++i)
            {
                BitConv.ToInt32(data, 8 + i * 16 + 0, Frames[i].PackedValue1);
                BitConv.ToInt32(data, 8 + i * 16 + 4, Frames[i].PackedValue2);
                BitConv.ToInt32(data, 8 + i * 16 + 8, Frames[i].PackedValue3);
                BitConv.ToInt32(data, 8 + i * 16 + 12, Frames[i].PackedValue4);
            }
            return data;
        }
    }
}
