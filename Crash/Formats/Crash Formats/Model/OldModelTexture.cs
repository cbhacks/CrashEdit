using System;

namespace Crash
{
    public struct OldModelTexture : OldModelStruct
    {
        public static OldModelTexture Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 12)
                throw new ArgumentException("Value must be 12 bytes long.", "data");
            byte r = data[0];
            byte g = data[1];
            byte b = data[2];
            byte blendmode = (byte)((data[3] >> 5) & 0x3);
            bool n = (data[3] & 0x10) != 0;
            byte clutx = (byte)(data[3] & 0xF);
            int eid = BitConv.FromInt32(data,4);
            int texinfo = BitConv.FromInt32(data,8);
            int uvindex = (texinfo >> 22) & 0x3FF;
            byte colormode = (byte)(texinfo >> 20 & 3);
            byte segment = (byte)(texinfo >> 18 & 3);
            byte xoffu = (byte)(texinfo >> 13 & 0x1F);
            byte cluty = (byte)(texinfo >> 6 & 0x7F);
            byte yoffu = (byte)(texinfo & 0x1F);
            return new OldModelTexture(uvindex,clutx,cluty,xoffu,yoffu,colormode,blendmode,segment,r,g,b,n,eid);
        }
        public OldModelTexture(int uvindex,byte clutx,byte cluty,byte xoffu,byte yoffu,byte colormode,byte blendmode,byte segment,byte r,byte g,byte b,bool n,int eid)
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
            N = n;
            EID = eid;
            
            int w = 4 << (UVIndex % 5);
            int h = 4 << ((UVIndex / 5) % 5);
            int xoff = ((64 << (2 - ColorMode)) * Segment) + ((2 << (2 - ColorMode)) * XOffU);
            int yoff = YOffU * 4;
            int winding = UVIndex / 25;
            U1 = w * ((0x30FF0C >> winding) & 1) + xoff;
            U2 = w * ((0x8799E1 >> winding) & 1) + xoff;
            U3 = w * ((0x4B66D2 >> winding) & 1) + xoff;
            V1 = h * ((0xF3CC30 >> winding) & 1) + yoff;
            V2 = h * ((0x9E7186 >> winding) & 1) + yoff;
            V3 = h * ((0x6DB249 >> winding) & 1) + yoff;
        }
        
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public bool N { get; }

        public int EID { get; }

        public byte ColorMode { get; }
        public int UVIndex { get; }
        public byte ClutX { get; } // 16-color (32-byte) segments
        public byte ClutY { get; }
        public byte XOffU { get; }
        public byte YOffU { get; }
        public byte BlendMode { get; }
        public byte Segment { get; }
        public int U1 { get; }
        public int U2 { get; }
        public int U3 { get; }
        public int V1 { get; }
        public int V2 { get; }
        public int V3 { get; }

        public byte[] Save()
        {
            byte[] result = new byte[8];
            result[0] = R;
            result[1] = G;
            result[2] = B;
            result[3] = (byte)(0x80 | (BlendMode << 5) | (Convert.ToByte(N) << 4) | ClutX);
            uint texinfo = ((uint)UVIndex << 22) | ((uint)ColorMode << 20) | ((uint)Segment << 18) | ((uint)XOffU << 13) | ((uint)ClutY << 6) | YOffU;
            BitConv.ToInt32(result, 4, (int)texinfo);
            return result;
        }
    }

}
