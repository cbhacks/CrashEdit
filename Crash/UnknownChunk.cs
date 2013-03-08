using System;

namespace Crash
{
    public sealed class UnknownChunk : Chunk
    {
        private byte[] data;

        public UnknownChunk(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
        }

        public override short Type
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public override byte[] Save(int chunkid)
        {
            return data;
        }
    }
}
