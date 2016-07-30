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

        private int structure;
        private bool footer;
        private bool colorslot;
        private bool animated;
        private byte colorflags;
        private byte colora;
        private byte colorb;
        private byte textureindex;
        private byte colorindex;
        private byte pointer;
        private byte tritype;
        private byte structtype;


        public ModelPolygon(int structure)
        {
            colorslot = ((structure & 0xFFFF0000) == 0);
            animated = ((structure >> 8) == 1);
            textureindex = (byte)structure;
            colorindex = (byte)((structure >> 9) & 0x7FFF);
            pointer = (byte)(structure >> 16);
            tritype = (byte)(structure >> 28);
            structtype = (byte)((structure >> 24) & 0xF);
            colorflags = (byte)(structure & 0x3);
            colora = (byte)((structure >> 2) & 0x7F);
            colorb = (byte)((structure >> 9) & 0x7F);
            footer = ((uint)structure == 0xFFFFFFFF);
            this.structure = structure;
        }

        public int Structure
        {
            get { return structure; }
        }

        public bool Footer
        {
            get { return footer; }
        }

        public bool ColorSlot
        {
            get { return colorslot; }
        }

        private byte ColorFlags
        {
            get { return colorflags; }
        }

        private byte ColorA
        {
            get { return colora; }
        }

        private byte ColorB
        {
            get { return colorb; }
        }

        private byte TextureIndex
        {
            get { return textureindex; }
        }

        private byte ColorIndex
        {
            get { return colorindex; }
        }

        private byte Pointer
        {
            get { return pointer; }
        }

        private byte TriType
        {
            get { return tritype; }
        }

        private byte StructType
        {
            get { return structtype; }
        }

        public byte[] Save()
        {
            //ErrorManager.SignalError("ModelPolygon cannot be saved.");
            byte[] result = new byte[4];
            BitConv.ToInt32(result, 0, structure);
            return result;
        }
    }
}
