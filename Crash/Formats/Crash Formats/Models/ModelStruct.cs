namespace Crash
{
    public interface ModelStruct
    {

    }

    public struct ModelTriangle : ModelStruct
    {
        public const byte NullPtr = 0x57;

        public enum IndexType : byte
        {
            Original = 0,
            Duplicate = 4
        }

        public static ModelTriangle Load(uint structure)
        {
            byte texture = (byte)structure;
            bool animated = (structure >> 8 & 0x1) != 0;
            byte color = (byte)(structure >> 9 & 0x7F);
            byte key = (byte)(structure >> 16);
            byte type = (byte)(structure >> 24 & 0x07);
            byte tritype = (byte)(structure >> 28);
            return new ModelTriangle(texture,animated,color,key,type,tritype);
        }

        public ModelTriangle(byte texture,bool animated,byte color,byte key,byte type,byte tritype)
        {
            TextureIndex = texture;
            Animated = animated;
            ColorIndex = color;
            PositionKey = key;
            Flag = (type & 0x8) == 1;
            Type = (IndexType)type;
            TriangleSubtype = (byte)(tritype & 0x3);
            TriangleType = (byte)(tritype >> 2 & 0x3);
        }

        public byte TextureIndex { get; }
        public byte ColorIndex { get; }
        public bool Animated { get; }
        public byte PositionKey { get; }
        public byte TriangleType { get; }
        public byte TriangleSubtype { get; }
        public bool Flag { get; }
        public IndexType Type { get; }
    }

    public struct ModelColor : ModelStruct
    {
        public static ModelColor Load(uint structure)
        {
            byte color1 = (byte)(structure >> 2 & 0x7F);
            byte color2 = (byte)(structure >> 9 & 0x7F);
            return new ModelColor(color1,color2);
        }

        public ModelColor(byte color1,byte color2)
        {
            Color1 = color1;
            Color2 = color2;
        }

        public byte Color1 { get; }
        public byte Color2 { get; }
    }
}
