using System;

namespace Crash
{
    public sealed class TextureChunk : Chunk,IEntry
    {
        private byte[] data;

        public TextureChunk(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
        }

        public override short Type
        {
            get { return 1; }
        }

        public int EID
        {
            get { return BitConv.FromInt32(data,4); }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            return new UnprocessedChunk(data);
        }
    }
}
