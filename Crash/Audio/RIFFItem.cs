using System;

namespace Crash.Audio
{
    public abstract class RIFFItem
    {
        private string name;

        public RIFFItem(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length != 4)
                throw new ArgumentException("Name must be 4 characters long.");
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name.Length != 4)
                    throw new ArgumentException("Name must be 4 characters long.");
                name = value;
            }
        }

        public abstract byte[] Save();
    }
}
