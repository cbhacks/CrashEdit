using System;
using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public abstract class Chunk
    {
        public const int Length = 65536;
        public const short Magic = 0x1234;
        public const short CompressedMagic = 0x1235;

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

        protected static int CalculateChecksum(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != Length)
                throw new ArgumentException("Value must be 65536 bytes long.","data");
            uint checksum = 0x12345678;
            for (int i = 0;i < Length;i++)
            {
                if (i < 12 || i >= 16)
                {
                    checksum += data[i];
                }
                checksum = checksum << 3 | checksum >> 29;
            }
            return (int)checksum;
        }

        public static Chunk Load(int chunkid,byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != Length)
                throw new ArgumentException("Value must be 65536 bytes long.","data");
            short magic = BitConv.FromInt16(data,0);
            short type = BitConv.FromInt16(data,2);
            int checksum = BitConv.FromInt32(data,12);
            if (magic != Magic)
            {
                throw new LoadException();
            }
            if (checksum != CalculateChecksum(data))
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
    }
}
