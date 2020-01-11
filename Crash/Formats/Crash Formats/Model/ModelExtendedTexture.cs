using System;

namespace Crash
{
    public struct ModelExtendedTexture
    {
        public static ModelExtendedTexture Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","data");
            int rest = BitConv.FromInt32(data,0);
            return new ModelExtendedTexture(rest);
        }

        public ModelExtendedTexture(int data)
        {
            Data = data;
        }
        
        public int Data { get; }

        public int Offset => Data & 0x7FF;
        public bool IsLOD => Data < 0;

        public int LOD0 => IsLOD ? Data >> 29 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD1 => IsLOD ? Data >> 27 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD2 => IsLOD ? Data >> 25 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD3 => IsLOD ? Data >> 23 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD4 => IsLOD ? Data >> 21 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD5 => IsLOD ? Data >> 19 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD6 => IsLOD ? Data >> 17 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");
        public int LOD7 => IsLOD ? Data >> 15 & 0x3 : throw new Exception("This extended texture field is not a LOD field.");

        public bool Leap => !IsLOD ? (Data >> 11 & 0x1) == 1 : throw new Exception("This extended texture field is not a flipbook field.");
        public int Mask => !IsLOD ? Data >> 12 & 0x7F : throw new Exception("This extended texture field is not a flipbook field.");
        public int Delay => !IsLOD ? Data >> 19 & 0x7F : throw new Exception("This extended texture field is not a flipbook field.");
        public int Latency => !IsLOD ? Data >> 26 & 0x1F : throw new Exception("This extended texture field is not a flipbook field.");

        public byte[] Save()
        {
            byte[] result = new byte[4];
            BitConv.ToInt32(result, 0, Data);
            return result;
        }
    }
}
