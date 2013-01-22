using System;

namespace Crash.Graphics
{
    public sealed class TextureChunk : Chunk
    {
        private byte[] data;

        public TextureChunk(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null.");
            this.data = data;
        }

        public override short Type
        {
            get { return 1; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override byte[] Save()
        {
            return data;
        }
    }
}
