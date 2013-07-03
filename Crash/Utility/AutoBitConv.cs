using System;

namespace Crash
{
    public static class AutoBitConv
    {
        public static short FromInt16(Endianness endianness,byte[] str,int offset)
        {
            switch (endianness)
            {
                case Endianness.LittleEndian:
                    return BitConv.FromInt16(str,offset);
                case Endianness.BigEndian:
                    return BEBitConv.FromInt16(str,offset);
                default:
                    throw new ArgumentException("Endianness is invalid.");
            }
        }

        public static int FromInt32(Endianness endianness,byte[] str,int offset)
        {
            switch (endianness)
            {
                case Endianness.LittleEndian:
                    return BitConv.FromInt32(str,offset);
                case Endianness.BigEndian:
                    return BEBitConv.FromInt32(str,offset);
                default:
                    throw new ArgumentException("Endianness is invalid.");
            }
        }

        public static void ToInt16(Endianness endianness,byte[] str,int offset,short value)
        {
            switch (endianness)
            {
                case Endianness.LittleEndian:
                    BitConv.ToInt16(str,offset,value);
                    break;
                case Endianness.BigEndian:
                    BEBitConv.ToInt16(str,offset,value);
                    break;
                default:
                    throw new ArgumentException("Endianness is invalid.");
            }
        }

        public static void ToInt32(Endianness endianness,byte[] str,int offset,int value)
        {
            switch (endianness)
            {
                case Endianness.LittleEndian:
                    BitConv.ToInt32(str,offset,value);
                    break;
                case Endianness.BigEndian:
                    BEBitConv.ToInt32(str,offset,value);
                    break;
                default:
                    throw new ArgumentException("Endianness is invalid.");
            }
        }
    }
}
