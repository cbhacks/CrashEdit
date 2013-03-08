using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class Chunk
    {
        public const int Length = 65536;
        public const short Magic = 0x1234;

        private static Dictionary<short,ChunkLoader> loaders;

        static Chunk()
        {
            loaders = new Dictionary<short,ChunkLoader>();
        }

        public static Chunk Load(int chunkid,byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != Length)
                throw new ArgumentException("Data must be 65536 bytes long.");
            short magic = BitConv.FromHalf(data,0);
            short type = BitConv.FromHalf(data,2);
            if (magic != Magic)
            {
                throw new LoadException();
            }
            if (loaders.ContainsKey(type))
            {
                return loaders[type].Load(chunkid,data);
            }
            else
            {
                return new UnknownChunk(data);
            }
        }

        public static void AddLoader(short type,ChunkLoader loader)
        {
            if (loader == null)
                throw new ArgumentNullException("loader");
            loaders.Add(type,loader);
        }

        public abstract short Type
        {
            get;
        }

        public abstract byte[] Save(int chunkid);
    }
}
