namespace CrashEdit.Crash
{
    public struct EntityNumber
    {
        public EntityNumber(byte dec, int whole)
        {
            if (whole < -8388608 || whole > 8388607)
                throw new ArgumentOutOfRangeException(nameof(whole));
            Value = dec | (whole << 8);
        }

        public EntityNumber(int value)
        {
            Value = value;
        }

        public byte Decimal => (byte)(Value);
        public int Whole => (Value & -256) >> 8;
        public int Value { get; }
    }
}
