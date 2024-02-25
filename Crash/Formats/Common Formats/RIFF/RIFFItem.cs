namespace Crash
{
    public abstract class RIFFItem
    {
        private string name;

        public RIFFItem(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length != 4)
                throw new ArgumentException("Value must be 4 characters long.", nameof(name));
            this.name = name;
        }

        public string Name
        {
            get => name;
            set
            {
                if (name.Length != 4)
                    throw new ArgumentException("Value must be 4 characters long.", nameof(name));
                name = value;
            }
        }

        public abstract int Length { get; }

        public byte[] Save()
        {
            return Save(Endianness.LittleEndian);
        }

        public abstract byte[] Save(Endianness endianness);
    }
}
