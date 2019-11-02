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

        public ModelAnimatedTexture(byte offset1,byte offset2,byte animationlength,byte offset3,short unknown)
        {
            Offset1 = offset1;
            Offset2 = offset2;
            AnimationLength = animationlength;
            Offset3 = offset3;
            Unknown = unknown;
        }

        public byte Offset1 { get; }
        public byte Offset2 { get; }
        public byte AnimationLength { get; }
        public byte Offset3 { get; }
        public short Unknown { get; }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelAnimatedTexture cannot be saved.");
            byte[] result = new byte[4];
            result[0] = (byte)((Offset1 << 4) | Offset2);
            result[1] = (byte)((AnimationLength << 4) | Offset3);
            BitConv.ToInt16(result, 2, Unknown);
            return result;
        }
    }
}
