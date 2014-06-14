using System;

namespace Crash
{
    public struct EntitySetting
    {
        private byte valuea;
        private int valueb;

        public EntitySetting(byte valuea,int valueb)
        {
            if (valueb < -8388608 || valueb > 8388607)
                throw new ArgumentOutOfRangeException("valueb");
            this.valuea = valuea;
            this.valueb = valueb;
        }

        public byte ValueA
        {
            get { return valuea; }
        }

        public int ValueB
        {
            get { return valueb; }
        }
    }
}
