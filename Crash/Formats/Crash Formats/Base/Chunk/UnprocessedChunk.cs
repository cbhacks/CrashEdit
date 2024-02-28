namespace CrashEdit.Crash
{
    public sealed class UnprocessedChunk : Chunk
    {
        public UnprocessedChunk(byte[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (data.Length != Length)
                throw new ArgumentException("Data must be 65536 bytes long.");
            Data = data;
        }

        public override string Title => $"Unprocessed Chunk T{Type} ({ID:X08})";
        public override string ImageKey => "JournalOrange";

        public override short Type => BitConv.FromInt16(Data, 2);
        public int ID => BitConv.FromInt32(Data, 4);

        public byte[] Data { get; }

        public Chunk Process()
        {
            if (loaders.ContainsKey(Type))
            {
                return loaders[Type].Load(Data);
            }
            else
            {
                ErrorManager.SignalError("UnprocessedChunk: Unknown chunk type");
                return null;
            }
        }

        public override UnprocessedChunk Unprocess()
        {
            return this;
        }

        public override byte[] Save()
        {
            return Data;
        }
    }
}
