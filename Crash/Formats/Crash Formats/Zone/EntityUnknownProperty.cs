namespace Crash
{
    public sealed class EntityUnknownProperty : EntityProperty
    {
        private byte type;
        private byte elementsize;
        private short unknown;
        private bool issparse;
        private bool hasmetavalues;

        public EntityUnknownProperty(byte type,byte elementsize,short unknown,bool issparse,bool hasmetavalues,byte[] data)
        {
            this.type = type;
            this.elementsize = elementsize;
            this.unknown = unknown;
            this.issparse = issparse;
            this.hasmetavalues = hasmetavalues;
            Data = data;
        }

        public override byte Type => type;
        public override byte ElementSize => elementsize;
        public override short RowCount => unknown;
        public override bool IsSparse => issparse;
        public override bool HasMetaValues => hasmetavalues;
        public byte[] Data { get; }

        public override byte[] Save()
        {
            return Data;
        }
    }
}
