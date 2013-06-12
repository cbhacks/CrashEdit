namespace Crash
{
    public sealed class EntityField
    {
        private short type;
        private byte unknown1;
        private byte elementsize;
        private short unknown2;
        private short elementcount;
        private short unknown3;
        private byte[] data;
        private byte[] extradata;

        public EntityField(short type,byte unknown1,byte elementsize,short unknown2,short elementcount,short unknown3,byte[] data)
        {
            this.type = type;
            this.unknown1 = unknown1;
            this.elementsize = elementsize;
            this.unknown2 = unknown2;
            this.elementcount = elementcount;
            this.unknown3 = unknown3;
            this.data = data;
            this.extradata = null;
        }

        public short Type
        {
            get { return type; }
        }

        public byte Unknown1
        {
            get { return unknown1; }
        }

        public byte ElementSize
        {
            get { return elementsize; }
        }

        public short Unknown2
        {
            get { return unknown2; }
        }

        public short ElementCount
        {
            get { return elementcount; }
        }

        public short Unknown3
        {
            get { return unknown3; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public byte[] ExtraData
        {
            get { return extradata; }
            set { extradata = value; }
        }
    }
}
