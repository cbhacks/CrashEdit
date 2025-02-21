namespace CrashEdit.Crash
{
    public class GOOLDecompDomVector : List<bool>
    {
        public GOOLDecompDomVector(int size) : base(new bool[size])
        {
        }

        public void Set(int i)
        {
            this[i] = true;
        }

        public void Clear(int i)
        {
            this[i] = false;
        }

        public void SetAll()
        {
            for (int i = 0; i < Count; ++i)
            {
                this[i] = true;
            }
        }

        public void ClearAll()
        {
            for (int i = 0; i < Count; ++i)
            {
                this[i] = false;
            }
        }

        public bool Equal(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return false;
            for (int i = 0; i < Count; ++i)
            {
                if (this[i] != other[i]) return false;
            }
            return true;
        }

        public void Merge(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return;
            for (int i = 0; i < Count; ++i)
            {
                this[i] |= other[i];
            }
        }

        public void Mask(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return;
            for (int i = 0; i < Count; ++i)
            {
                this[i] &= other[i];
            }
        }

        public void Overwrite(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return;
            for (int i = 0; i < Count; ++i)
            {
                this[i] = other[i];
            }
        }
    }
}
