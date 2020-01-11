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
            Duplicate = 1
        }

        public static ModelTriangle Load(uint structure)
        {
            // LE format: YYSSFTLL PPPPPPPP CCCCCCCA XXXXXXXX
            // TTL is probably TLL or maybe even TPP?
            byte texture = (byte)structure; // X
            bool animated = (structure >> 8 & 0x1) != 0; // A
            byte color = (byte)(structure >> 9 & 0x7F); // C
            byte key = (byte)(structure >> 16); // P
            byte unknown = (byte)(structure >> 24 & 0x3); // L
            byte type = (byte)(structure >> 26 & 0x01); // T
            bool flag = (structure >> 27 & 0x1) != 0; // F
            byte tritype = (byte)(structure >> 28); // Y/S
            return new ModelTriangle(texture,animated,color,key,unknown,type,flag,tritype);
        }

        public ModelTriangle(byte texture,bool animated,byte color,byte key,byte unknown,byte type,bool flag,byte tritype)
        {
            TextureIndex = texture;
            Animated = animated;
            ColorIndex = color;
            PositionKey = key;
            Unknown = unknown;
            Type = (IndexType)type;
            Flag = flag;
            TriangleSubtype = (byte)(tritype & 0x3);
            TriangleType = (byte)(tritype >> 2 & 0x3);
        }

        public byte TextureIndex { get; }
        public byte ColorIndex { get; }
        public bool Animated { get; }
        public byte PositionKey { get; }
        public byte TriangleType { get; }
        public byte TriangleSubtype { get; }
        public byte Unknown { get; }
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
