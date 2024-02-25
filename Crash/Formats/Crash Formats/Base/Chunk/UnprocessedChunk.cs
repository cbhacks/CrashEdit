namespace Crash
{
    public sealed class UnprocessedChunk : Chunk
    {
        public UnprocessedChunk(byte[] data, NSF nsf) : base(nsf)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length != Length)
                throw new ArgumentException("Data must be 65536 bytes long.");
            Data = data;
        }

        public override short Type => BitConv.FromInt16(Data, 2);
        public int ID => BitConv.FromInt32(Data, 4);

        public byte[] Data { get; }

        public Chunk Process(int chunkid)
        {
            if (loaders.ContainsKey(Type))
            {
                return loaders[Type].Load(chunkid, Data, NSF);
            }
            else
            {
                ErrorManager.SignalError("UnprocessedChunk: Unknown chunk type");
                return null;
            }
        }

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            return this;
        }

        public override byte[] Save(int chunkid)
        {
            return Data;
        }
    }
}
