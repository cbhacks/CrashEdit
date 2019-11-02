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
            byte point1x = data[0];
            byte point1y = data[1];
            byte cluty1 = (byte)(data[2] >> 4);
            byte clutx = (byte)(data[2] & 0xF);
            byte cluty2 = data[3];
            byte point2x = data[4];
            byte point2y = data[5];
            bool bitflag = (data[6] >> 7 == 1);
            byte blendmode = (byte)((data[6] >> 4) & 0x7);
            byte segment = (byte)(data[6] & 0xF);
            byte textureoffset = data[7];
            byte point3x = data[8];
            byte point3y = data[9];
            short unknown = BitConv.FromInt16(data,10);
            return new ModelTexture(point1x, point1y,cluty1,clutx,cluty2, point2x, point2y,bitflag,blendmode,segment,textureoffset, point3x, point3y,unknown);
        }

        public ModelTexture(byte point1x, byte point1y, byte cluty1, byte clutx, byte cluty2, byte point2x, byte point2y, bool bitflag, byte blendmode, byte segment, byte textureoffset, byte point3x, byte point3y,short unknown)
        {
            Point1X = point1x;
            Point1Y = point1y;
            Point2X = point2x;
            Point2Y = point2y;
            Point3X = point3x;
            Point3Y = point3y;
            ClutY1 = cluty1;
            ClutX = clutx;
            ClutY2 = cluty2;
            BitFlag = bitflag;
            BlendMode = blendmode;
            Segment = segment;
            TextureOffset = textureoffset;
            Unknown = unknown;
        }

        public bool BitFlag { get; }
        public byte Point1X { get; }
        public byte Point1Y { get; }
        public byte Point2X { get; }
        public byte Point2Y { get; }
        public byte Point3X { get; }
        public byte Point3Y { get; }
        public byte ClutX { get; }
        public byte ClutY1 { get; }
        public byte ClutY2 { get; }
        public byte BlendMode { get; }
        public byte Segment { get; }
        public byte TextureOffset { get; }
        public short Unknown { get; }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelTexture cannot be saved.");
            byte[] result = new byte[12];
            result[0] = Point1X;
            result[1] = Point1Y;
            result[2] = (byte)((ClutY1  << 4) | ClutX);
            result[3] = ClutY2;
            result[4] = Point2X;
            result[5] = Point2Y;
            result[6] = (byte)((Convert.ToByte(BitFlag) << 7) | (BlendMode << 4) | Segment);
            result[7] = TextureOffset;
            result[8] = Point3X;
            result[9] = Point3Y;
            BitConv.ToInt16(result, 10, Unknown);
            return result;
        }
    }
}
