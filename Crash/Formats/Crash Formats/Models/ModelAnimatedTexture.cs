using System;

namespace Crash
{
    public struct ModelAnimatedTexture
    {
        public static ModelAnimatedTexture Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","data");
            byte offset1 = (byte)(data[0] >> 4);
            byte offset2 = (byte)(data[0] & 0xF);
            byte animationlength = (byte)(data[1] >> 4);
            byte offset3 = (byte)(data[1] & 0xF);
            short unknown = BitConv.FromInt16(data,2);
            return new ModelAnimatedTexture(offset1,offset2,animationlength,offset3,unknown);
        }
        private byte offset1;
        private byte offset2;
        private byte animationlength;
        private byte offset3;
        private short unknown;

        public ModelAnimatedTexture(byte offset1,byte offset2,byte animationlength,byte offset3,short unknown)
        {
            this.offset1 = offset1;
            this.offset2 = offset2;
            this.animationlength = animationlength;
            this.offset3 = offset3;
            this.unknown = unknown;
        }

        public byte Offset1
        {
            get { return offset1; }
        }

        public byte Offset2
        {
            get { return offset2; }
        }

        public byte AnimationLength
        {
            get { return animationlength; }
        }

        public byte Offset3
        {
            get { return offset3; }
        }

        public short Unknown
        {
            get { return unknown; }
        }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelAnimatedTexture cannot be saved.");
            byte[] result = new byte[4];
            result[0] = (byte)((offset1 << 4) | offset2);
            result[1] = (byte)((animationlength << 4) | offset3);
            BitConv.ToInt16(result, 2, unknown);
            return result;
        }
    }
}
