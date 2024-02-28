using System.Reflection;

namespace CrashEdit.Crash
{
    public abstract class Chunk : IResource
    {
        public const int Length = 65536;
        public const short Magic = 0x1234;
        public const short CompressedMagic = 0x1235;

        internal static Dictionary<short, ChunkLoader> loaders;

        static Chunk()
        {
            loaders = new Dictionary<short, ChunkLoader>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (ChunkTypeAttribute attribute in type.GetCustomAttributes(typeof(ChunkTypeAttribute), false))
                {
                    ChunkLoader loader = (ChunkLoader)Activator.CreateInstance(type);
                    loaders.Add(attribute.Type, loader);
                }
            }
        }

        public static int CalculateChecksum(byte[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (data.Length != Length)
                throw new ArgumentException("Value must be 65536 bytes long.", nameof(data));
            uint checksum = 0x12345678;
            for (int i = 0; i < Length; i++)
            {
                if (i < 12 || i >= 16)
                {
                    checksum += data[i];
                }
                checksum = checksum << 3 | checksum >> 29;
            }
            return (int)checksum;
        }

        public static UnprocessedChunk Load(byte[] data)
        {
            short magic = BitConv.FromInt16(data, 0);
            if (magic != Magic)
            {
                ErrorManager.SignalIgnorableError("Chunk: Magic number is wrong");
            }
            return new UnprocessedChunk(data);
        }

        public abstract string Title { get; }
        public abstract string ImageKey { get; }

        public abstract short Type { get; }

        public abstract UnprocessedChunk Unprocess();

        public virtual byte[] Save()
        {
            return Unprocess().Save();
        }
    }
}
