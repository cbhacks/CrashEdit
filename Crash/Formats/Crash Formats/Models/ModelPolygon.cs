using System;

namespace Crash
{
    public struct ModelPolygon
    {
        public static ModelPolygon Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.","data");
            int structure = BitConv.FromInt32(data,0);
            return new ModelPolygon(structure);
        }
        
        public ModelPolygon(int structure)
        {
            ColorSlot = (structure & 0xFFFF0000) == 0;
            Animated = (structure >> 8) == 1;
            TextureIndex = (byte)structure;
            ColorIndex = (byte)((structure >> 9) & 0x7F);
            Pointer = (byte)(structure >> 16);
            TriType = (byte)((structure >> 28) & 0xF); //AA, BB, CC
            StructType = (byte)((structure >> 24) & 0xF); //Original or duplicate
            //Colorslot
            ColorFlags = (byte)(structure & 0x3);
            ColorA = (byte)((structure >> 2) & 0x7F);
            ColorB = (byte)((structure >> 9) & 0x7F);
            //Footer
            Footer = (uint)structure == 0xFFFFFFFF;
            Structure = structure;
        }

        public int Structure { get; }
        public bool Footer { get; }
        public bool ColorSlot { get; }
        public bool Animated { get; }
        public byte ColorFlags { get; }
        public byte ColorA { get; }
        public byte ColorB { get; }
        public byte TextureIndex { get; }
        public byte ColorIndex { get; }
        public byte Pointer { get; }
        public byte TriType { get; }
        public byte StructType { get; }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelPolygon cannot be saved.");
            byte[] result = new byte[4];
            BitConv.ToInt32(result, 0, Structure);
            return result;
        }
    }
}
