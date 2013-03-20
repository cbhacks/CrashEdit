using System;
using System.Reflection;
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
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (ChunkTypeAttribute attribute in type.GetCustomAttributes(typeof(ChunkTypeAttribute),false))
                {
                    ChunkLoader loader = (ChunkLoader)Activator.CreateInstance(type);
                    loaders.Add(attribute.Type,loader);
                }
            }
        }

        public static Chunk Load(int chunkid,byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != Length)
                throw new ArgumentException("Value must be 65536 bytes long.","data");
            short magic = BitConv.FromInt16(data,0);
            short type = BitConv.FromInt16(data,2);
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

        public abstract short Type
        {
            get;
        }

        public abstract byte[] Save(int chunkid);

        protected void CalculateChecksum(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != Length)
                throw new ArgumentException("Value must be 65536 bytes long.","data");
            BitConv.ToInt32(data,12,0);
            uint checksum = 0x12345678;
            for (int i = 0;i < Length;i++)
            {
                checksum += data[i];
                checksum = checksum << 3 | checksum >> 29;
            }
            BitConv.ToInt32(data,12,(int)checksum);
        }
    }
}
