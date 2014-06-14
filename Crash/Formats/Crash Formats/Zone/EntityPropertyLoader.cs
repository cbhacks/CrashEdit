namespace Crash
{
    public abstract class EntityPropertyLoader
    {
        public abstract EntityProperty Load(byte elementsize,short unknown,bool issparse,bool hasmetavalues,byte[] data);
    }
}
