using System;

namespace Crash
{
    public struct OldModelTexture : OldModelStruct
    {
        public static OldModelTexture Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.", "data");
            byte blendmode = (byte)((data[3] >> 5) & 0x3);
            byte clutx = (byte)(data[3] & 0xF);
            uint texinfo = (uint)BitConv.FromInt32(data, 4);
            uint uvindex = ((texinfo >> 22) & 0x3FF);
            bool bitflag = ((texinfo >> 20 & 1) == 1);
            byte segment = (byte)(texinfo >> 18 & 3);
            byte xoffu = (byte)(texinfo >> 13 & 0x1F);
            byte cluty = (byte)(texinfo >> 6 & 0x7F);
            byte yoffu = (byte)(texinfo & 0x1F);
            return new OldModelTexture(uvindex, clutx, cluty, xoffu, yoffu, bitflag, blendmode, segment);
        }
        public OldModelTexture(uint uvindex, byte clutx, byte cluty, byte xoffu, byte yoffu, bool bitflag, byte blendmode, byte segment)
        {
            UVIndex = uvindex;
            ClutX = clutx;
            ClutY = cluty;
            XOffU = xoffu;
            YOffU = yoffu;
            Segment = segment;
            BlendMode = blendmode;
            BitFlag = bitflag;
        }
        public bool BitFlag { get; }
        public uint UVIndex { get; }
        public byte ClutX { get; } // 16-color (32-byte) segments
        public byte ClutY { get; }
        public byte XOffU { get; }
        public byte YOffU { get; }
        public byte BlendMode { get; }
        public byte Segment { get; }
        private float PageWidth => BitFlag ? 512F : 1024F;
        private int XOff => ((BitFlag ? 128 : 256) * Segment) + ((BitFlag ? 4 : 8) * XOffU);
        private int YOff => YOffU * 4;
        public int Left => Math.Min(U1, Math.Min(U2, U3)) + XOff;
        private int Right => Math.Max(U1, Math.Max(U2, U3)) + XOff;
        public int Top => Math.Min(V1, Math.Min(V2, V3)) + YOff;
        private int Bottom => Math.Max(V1, Math.Max(V2, V3)) + YOff;
        public int Width => (4 << ((int)UVIndex % 5)) - 1;
        public int Height => (4 << (((int)UVIndex / 5) % 5)) - 1;
        public int FlipWinding => (int)UVIndex / 25;
        public int U1 => Width * ((0x30FF0C >> FlipWinding) & 1);
        public int V1 => Height * ((0xF3CC30 >> FlipWinding) & 1);
        public int U2 => Width * ((0x8799E1 >> FlipWinding) & 1);
        public int V2 => Height * ((0x9E7186 >> FlipWinding) & 1);
        public int U3 => Width * ((0x4B66D2 >> FlipWinding) & 1);
        public int V3 => Height * ((0x6DB249 >> FlipWinding) & 1);
        public int U4 => Width * ((0x30FF0C >> FlipWinding) & 1); // TODO: correct
        public int V4 => Height * ((0xF3CC30 >> FlipWinding) & 1); // TODO: correct

        public float X1 => (U1 + XOff) / PageWidth;
        public float X2 => (U2 + XOff) / PageWidth;
        public float X3 => (U3 + XOff) / PageWidth;
        public float X4 => (U4 + XOff) / PageWidth;
        public float Y1 => (V1 + YOff) / 128F;
        public float Y2 => (V2 + YOff) / 128F;
        public float Y3 => (V3 + YOff) / 128F;
        public float Y4 => (V4 + YOff) / 128F;

        public byte[] Save()
        {
            byte[] result = new byte[8];
            result[0] = 0;
            result[1] = 0;
            result[2] = 0;
            result[3] = (byte)(0x80 | (BlendMode << 5) | ClutX);
            uint texinfo = (UVIndex << 22) | (Convert.ToUInt32(BitFlag) << 20) | ((uint)Segment << 18) | ((uint)XOffU << 13) | ((uint)ClutY << 6) | YOffU;
            BitConv.ToInt32(result, 4, (int)texinfo);
            return result;
        }
    }

}
