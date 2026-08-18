namespace CrashEdit.Crash
{
    public sealed class EntityUnknownProperty : EntityProperty
    {
        private readonly EntityPropertyType type;
        private readonly byte elementsize;
        private readonly short unknown;
        private readonly bool issparse;
        private readonly bool haskeyframes;

        public EntityUnknownProperty(EntityPropertyType type, byte elementsize, short unknown, bool issparse, bool haskeyframes, byte[] data)
        {
            this.type = type;
            this.elementsize = elementsize;
            this.unknown = unknown;
            this.issparse = issparse;
            this.haskeyframes = haskeyframes;
            Data = data;
        }

        public override EntityPropertyType Type => type;
        public override byte ElementSize => elementsize;
        public override short RowCount => unknown;
        public override bool IsSparse => issparse;
        public override bool HasKeyframes => haskeyframes;
        public byte[] Data { get; }

        public override byte[] Save()
        {
            return Data;
        }
    }
}
