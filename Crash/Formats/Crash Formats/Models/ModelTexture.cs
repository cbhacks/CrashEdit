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

        private byte point1x;
        private byte point1y;
        private byte point2x;
        private byte point2y;
        private byte point3x;
        private byte point3y;
        private byte cluty1;
        private byte clutx;
        private byte cluty2;
        private bool bitflag;
        private byte blendmode;
        private byte segment;
        private byte textureoffset;
        private short unknown;

        public ModelTexture(byte point1x, byte point1y, byte cluty1, byte clutx, byte cluty2, byte point2x, byte point2y, bool bitflag, byte blendmode, byte segment, byte textureoffset, byte point3x, byte point3y,short unknown)
        {
            this.point1x = point1x;
            this.point1y = point1y;
            this.point2x = point2x;
            this.point2y = point2y;
            this.point3x = point3x;
            this.point3y = point3y;
            this.cluty1 = cluty1;
            this.clutx = clutx;
            this.cluty2 = cluty2;
            this.bitflag = bitflag;
            this.blendmode = blendmode;
            this.segment = segment;
            this.textureoffset = textureoffset;
            this.unknown = unknown;
        }

        public bool BitFlag
        {
            get { return bitflag; }
        }

        public byte Point1X
        {
            get { return point1x; }
        }

        public byte Point1Y
        {
            get { return point1y; }
        }

        public byte Point2X
        {
            get { return point2x; }
        }

        public byte Point2Y
        {
            get { return point2y; }
        }

        public byte Point3X
        {
            get { return point3x; }
        }

        public byte Point3Y
        {
            get { return point3x; }
        }

        public byte ClutX
        {
            get { return clutx; }
        }

        public byte ClutY1
        {
            get { return cluty1; }
        }

        public byte ClutY2
        {
            get { return cluty2; }
        }

        public byte BlendMode
        {
            get { return blendmode; }
        }

        public byte Segment
        {
            get { return point1x; }
        }

        public byte TextureOffset
        {
            get { return textureoffset; }
        }

        public short Unknown
        {
            get { return unknown; }
        }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelTexture cannot be saved.");
            byte[] result = new byte[12];
            result[0] = point1x;
            result[1] = point1y;
            result[2] = (byte)((cluty1  << 4) | clutx);
            result[3] = cluty2;
            result[4] = point2x;
            result[5] = point2y;
            result[6] = (byte)((Convert.ToByte(bitflag) << 7) | (blendmode << 4) | segment);
            result[7] = textureoffset;
            result[8] = point3x;
            result[9] = point3y;
            BitConv.ToInt16(result, 10, unknown);
            return result;
        }
    }
}
