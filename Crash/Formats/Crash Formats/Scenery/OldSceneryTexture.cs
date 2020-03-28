using System;

namespace Crash
{
    public struct OldSceneryTexture : OldModelStruct
    {
        public static OldSceneryTexture Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.", "data");
            byte r = data[0];
            byte g = data[1];
            byte b = data[2];
            byte blendmode = (byte)((data[3] >> 5) & 0x3);
            byte clutx = (byte)(data[3] & 0xF);
            uint texinfo = (uint)BitConv.FromInt32(data, 4);
            uint uvindex = ((texinfo >> 22) & 0x3FF);
            byte colormode = (byte)(texinfo >> 20 & 3);
            byte segment = (byte)(texinfo >> 18 & 3);
            byte xoffu = (byte)(texinfo >> 13 & 0x1F);
            byte cluty = (byte)(texinfo >> 6 & 0x7F);
            byte yoffu = (byte)(texinfo & 0x1F);
            return new OldSceneryTexture(uvindex,clutx,cluty,xoffu,yoffu,colormode,blendmode,segment,r,g,b);
        }
        public OldSceneryTexture(uint uvindex,byte clutx,byte cluty,byte xoffu,byte yoffu,byte colormode,byte blendmode,byte segment,byte r,byte g,byte b)
        {
            UVIndex = uvindex;
            ClutX = clutx;
            ClutY = cluty;
            XOffU = xoffu;
            YOffU = yoffu;
            Segment = segment;
            BlendMode = blendmode;
            ColorMode = colormode;
            R = r;
            G = g;
            B = b;
            
            Width = 4 << ((int)UVIndex % 5);
            Height = 4 << (((int)UVIndex / 5) % 5);
            FlipWinding = (int)UVIndex / 25;
            U1 = Width * ((0x30FF0C >> FlipWinding) & 1);
            V1 = Height * ((0xF3CC30 >> FlipWinding) & 1);
            U2 = Width * ((0x8799E1 >> FlipWinding) & 1);
            V2 = Height * ((0x9E7186 >> FlipWinding) & 1);
            U3 = Width * ((0x4B66D2 >> FlipWinding) & 1);
            V3 = Height * ((0x6DB249 >> FlipWinding) & 1);
            PageWidth = (float)(1 << (2-ColorMode)) * 256;
            XOff = ((64 << (2-ColorMode)) * Segment) + ((2 << (2-ColorMode)) * XOffU);
            YOff = YOffU * 4;
            Left = Math.Min(U1, Math.Min(U2, U3)) + XOff;
            Top = Math.Min(V1, Math.Min(V2, V3)) + YOff;

            X1 = (U1 + XOff) / PageWidth;
            X2 = (U2 + XOff) / PageWidth;
            X3 = (U3 + XOff) / PageWidth;
            Y1 = (V1 + YOff) / 128F;
            Y2 = (V2 + YOff) / 128F;
            Y3 = (V3 + YOff) / 128F;
        }
        
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public byte ColorMode { get; }
        public uint UVIndex { get; }
        public byte ClutX { get; } // 16-color (32-byte) segments
        public byte ClutY { get; }
        public byte XOffU { get; }
        public byte YOffU { get; }
        public byte BlendMode { get; }
        public byte Segment { get; }

        private float PageWidth { get; }
        private int XOff { get; }
        private int YOff { get; }
        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }
        public int FlipWinding { get; }
        public int U1 { get; }
        public int V1 { get; }
        public int U2 { get; }
        public int V2 { get; }
        public int U3 { get; }
        public int V3 { get; }

        public float X1 { get; }
        public float X2 { get; }
        public float X3 { get; }
        public float Y1 { get; }
        public float Y2 { get; }
        public float Y3 { get; }

        public byte[] Save()
        {
            byte[] result = new byte[8];
            result[0] = 0;
            result[1] = 0;
            result[2] = 0;
            result[3] = (byte)(0x80 | (BlendMode << 5) | ClutX);
            uint texinfo = (UVIndex << 22) | ((uint)ColorMode << 20) | ((uint)Segment << 18) | ((uint)XOffU << 13) | ((uint)ClutY << 6) | YOffU;
            BitConv.ToInt32(result, 4, (int)texinfo);
            return result;
        }
    }

}
