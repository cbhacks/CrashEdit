using System;

namespace Crash
{
    public sealed class TextureChunk : Chunk,IEntry
    {
        public TextureChunk(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException("data");
        }

        public override short Type
        {
            get { return 1; }
        }

        public int EID
        {
            get { return BitConv.FromInt32(Data,4); }
        }

        public byte[] Data { get; }

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            return new UnprocessedChunk(Data);
        }
    }
}
