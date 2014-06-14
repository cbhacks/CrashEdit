namespace Crash
{
    public sealed class EntityUnknownProperty : EntityProperty
    {
        private byte type;
        private byte elementsize;
        private short unknown;
        private bool issparse;
        private bool hasmetavalues;
        private byte[] data;

        public EntityUnknownProperty(byte type,byte elementsize,short unknown,bool issparse,bool hasmetavalues,byte[] data)
        {
            this.type = type;
            this.elementsize = elementsize;
            this.unknown = unknown;
            this.issparse = issparse;
            this.hasmetavalues = hasmetavalues;
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

        public override bool IsSparse
        {
            get { return issparse; }
        }

        public override bool HasMetaValues
        {
            get { return hasmetavalues; }
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
