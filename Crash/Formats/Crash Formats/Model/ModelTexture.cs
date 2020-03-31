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

            double pw = 256 << (2-ColorMode);
            int xoff = (1 << (2-ColorMode)) * 64 * Segment;
            Left = Math.Min(U1, Math.Min(U2, U3)) + xoff;
            Top = Math.Min(V1, Math.Min(V2, V3));
            Width = Math.Max(U1, Math.Max(U2, U3)) + xoff - Left;
            Height = Math.Max(V1, Math.Max(V2, V3)) - Top;
            int tx1 = U1+xoff;
            int tx2 = U2+xoff;
            int tx3 = U3+xoff;
            int tx4 = U4+xoff;
            if (tx1 > tx2 || tx1 > tx3) ++tx1;
            if (tx2 > tx1 || tx2 > tx3) ++tx2;
            if (tx3 > tx2 || tx3 > tx1) ++tx3;
            if (tx4 > tx2 || tx4 > tx3 || tx4 > tx1) ++tx4;
            X1 = tx1/pw;
            X2 = tx2/pw;
            X3 = tx3/pw;
            X4 = tx4/pw;
            int ty1 = V1;
            int ty2 = V2;
            int ty3 = V3;
            int ty4 = V4;
            if (ty1 > ty2 || ty1 > ty3) ++ty1;
            if (ty2 > ty1 || ty2 > ty3) ++ty2;
            if (ty3 > ty2 || ty3 > ty1) ++ty3;
            if (ty4 > ty2 || ty4 > ty3 || ty4 > ty1) ++ty4;
            Y1 = ty1/128.0;
            Y2 = ty2/128.0;
            Y3 = ty3/128.0;
            Y4 = ty4/128.0;
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

        public int ClutY => (ClutY2 << 2) | (ClutY1 >> 2 & 0x3);
        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }
        public double X1 { get; }
        public double X2 { get; }
        public double X3 { get; }
        public double X4 { get; }
        public double Y1 { get; }
        public double Y2 { get; }
        public double Y3 { get; }
        public double Y4 { get; }

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
