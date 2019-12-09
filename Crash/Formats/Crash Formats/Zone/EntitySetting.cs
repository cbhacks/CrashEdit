using System;

namespace Crash
{
    public struct EntitySetting
    {
        public EntitySetting(byte valuea,int valueb)
        {
            if (valueb < -8388608 || valueb > 8388607)
                throw new ArgumentOutOfRangeException("valueb");
            ValueA = valuea;
            ValueB = valueb;
        }

        public byte ValueA { get; }
        public int ValueB { get; }
        public int ValueInt => ValueA | (ValueB << 8);
    }
}
