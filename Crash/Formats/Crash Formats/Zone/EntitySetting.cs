using System;

namespace Crash
{
    public struct EntitySetting
    {
        public EntitySetting(byte valuea,int valueb)
        {
            if (valueb < -8388608 || valueb > 8388607)
                throw new ArgumentOutOfRangeException("valueb");
            Value = valuea | (valueb << 8);
        }

        public EntitySetting(int value)
        {
            Value = value;
        }

        public byte ValueA => (byte)(Value);
        public int ValueB => (Value & -256) >> 8;
        public int Value { get; }
    }
}
