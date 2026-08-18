namespace CrashEdit.Crash
{
    public sealed class EntityChunkProperty : EntityBasicProperty<int>
    {
        public EntityChunkProperty()
        {
        }

        public EntityChunkProperty(IEnumerable<EntityPropertyRow<int>> rows) : base(rows)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.Chunk;
        public override byte ElementSize => 4;

        protected override void SaveElement(byte[] data, int value)
        {
            BitConv.ToInt32(data, 0, value);
        }
    }
}
