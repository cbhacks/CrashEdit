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
        public int EID => BitConv.FromInt32(Data, 4);
        public string EName => Entry.EIDToEName(EID);
        public byte[] Data { get; }

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            return new UnprocessedChunk(Data);
        }
    }
}
