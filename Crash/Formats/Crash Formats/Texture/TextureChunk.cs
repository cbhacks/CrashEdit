using System;

namespace Crash
{
    public sealed class TextureChunk : Chunk,IEntry
    {
        public TextureChunk(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException("data");
        }

        public override short Type => 1;
        public int EID
        {
            get => BitConv.FromInt32(Data,4);
            set => BitConv.ToInt32(Data,4,value);
        }
        public string EName => Entry.EIDToEName(EID);
        public int HashKey => EID >> 15 & 0xFF;
        public byte[] Data { get; }

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            return new UnprocessedChunk(Data);
        }
    }
}
