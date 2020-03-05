using System;

namespace Crash
{
    public struct ModelTexture
    {
        public static ModelTexture Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 12)
                throw new ArgumentException("Value must be 12 bytes long.","data");
            byte u1 = data[0];
            byte v1 = data[1];
            byte clutx = (byte)(data[2] & 0xF);
            byte cluty1 = (byte)(data[2] >> 4 & 0xF);
            byte cluty2 = data[3];
            byte u2 = data[4];
            byte v2 = data[5];
            byte segment = (byte)(data[6] & 0xF);
            byte blendmode = (byte)((data[6] >> 5) & 0x3);
            byte colormode = (byte)((data[6] >> 7 | data[7] << 1 & 2) & 0x3);
            byte textureoffset = (byte)(data[7] >> 2 & 0x3F);
            byte u3 = data[8];
            byte v3 = data[9];
            byte u4 = data[10];
            byte v4 = data[11];
            return new ModelTexture(u1,v1,cluty1,clutx,cluty2,u2,v2,colormode,blendmode,segment,textureoffset,u3,v3,u4,v4);
        }

        public ModelTexture(byte u1,byte v1,byte cluty1,byte clutx,byte cluty2,byte u2,byte v2,byte colormode,byte blendmode,byte segment,byte textureoffset,byte u3,byte v3,byte u4,byte v4)
        {
            U1 = u1;
            V1 = v1;
            U2 = u2;
            V2 = v2;
            U3 = u3;
            V3 = v3;
            U4 = u4;
            V4 = v4;
            ClutX = clutx;
            ClutY1 = cluty1;
            ClutY2 = cluty2;
            Segment = segment;
            BlendMode = blendmode;
            ColorMode = colormode;
            Page = textureoffset;
        }

        public byte ColorMode { get; set; }
        public byte U1 { get; }
        public byte V1 { get; }
        public byte U2 { get; }
        public byte V2 { get; }
        public byte U3 { get; }
        public byte V3 { get; }
        public byte U4 { get; }
        public byte V4 { get; }
        public byte ClutX { get; set; } // 16-color (32-byte) segments
        public byte ClutY1 { get; set; }
        public byte ClutY2 { get; set; }
        public byte BlendMode { get; set; }
        public byte Segment { get; }
        public byte Page { get; }

        public float PageWidth => (float)Math.Pow(2,2-ColorMode) * 256;
        private int XOff => (1 << (2-ColorMode)) * 64 * Segment;
        public int Left => Math.Min(U1, Math.Min(U2, U3)) + XOff;
        private int Right => Math.Max(U1, Math.Max(U2, U3)) + XOff;
        public int Top => Math.Min(V1, Math.Min(V2, V3));
        private int Bottom => Math.Max(V1, Math.Max(V2, V3));
        public int Width => Right - Left;
        public int Height => Bottom - Top;
        public int ClutY => (ClutY2 << 2) | (ClutY1 >> 2 & 0x3);
        public float X1 => ((U1 + XOff) - Left) / (float)(Right - Left) * ((Width+1)/PageWidth) + Left/PageWidth;
        public float X2 => ((U2 + XOff) - Left) / (float)(Right - Left) * ((Width+1)/PageWidth) + Left/PageWidth;
        public float X3 => ((U3 + XOff) - Left) / (float)(Right - Left) * ((Width+1)/PageWidth) + Left/PageWidth;
        public float X4 => ((U4 + XOff) - Left) / (float)(Right - Left) * ((Width+1)/PageWidth) + Left/PageWidth;
        public float Y1 => (V1 - Top) / (float)(Bottom - Top) * ((Height+1)/128F) + Top/128F;
        public float Y2 => (V2 - Top) / (float)(Bottom - Top) * ((Height+1)/128F) + Top/128F;
        public float Y3 => (V3 - Top) / (float)(Bottom - Top) * ((Height+1)/128F) + Top/128F;
        public float Y4 => (V4 - Top) / (float)(Bottom - Top) * ((Height+1)/128F) + Top/128F;

        public byte[] Save()
        {
            byte[] result = new byte[12];
            result[0] = U1;
            result[1] = V1;
            result[2] = (byte)((ClutY1  << 4) | ClutX);
            result[3] = ClutY2;
            result[4] = U2;
            result[5] = V2;
            result[6] = (byte)((ColorMode << 7) | (BlendMode << 5) | Segment);
            result[7] = (byte)((Page << 2) | (ColorMode >> 1));
            result[8] = U3;
            result[9] = V3;
            result[10] = U4;
            result[11] = V4;
            return result;
        }
    }
}
