namespace Crash
{
    public sealed class EntityUnknownProperty : EntityProperty
    {
        private byte type;
        private byte elementsize;
        private short unknown;
        private byte[] data;

        public EntityUnknownProperty(byte type,byte elementsize,short unknown,byte[] data)
        {
            this.type = type;
            this.elementsize = elementsize;
            this.unknown = unknown;
            this.data = data;
        }

        public override byte Type
        {
            get { return type; }
        }

        public override byte ElementSize
        {
            get { return elementsize; }
        }

        public override short Unknown
        {
            get { return unknown; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override byte[] Save()
        {
            return data;
        }
    }
}
