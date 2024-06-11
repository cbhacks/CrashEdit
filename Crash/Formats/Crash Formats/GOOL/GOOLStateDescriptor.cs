namespace CrashEdit.Crash
{
    public struct GOOLStateDescriptor
    {
        public GOOLStateDescriptor(int stateflags, int cflags, short goolid, short epc, short tpc, short cpc)
        {
            StateFlags = stateflags;
            BlockFlags = cflags;
            GOOLIndex = goolid;
            EventHook = epc;
            TransHook = tpc;
            CodeHook = cpc;
        }

        public int StateFlags { get; }
        public int BlockFlags { get; }
        public short GOOLIndex { get; }
        public short EventHook { get; }
        public short TransHook { get; }
        public short CodeHook { get; }

        public byte[] Save()
        {
            byte[] result = new byte[16];
            BitConv.ToInt32(result, 0x0, StateFlags);
            BitConv.ToInt32(result, 0x4, BlockFlags);
            BitConv.ToInt16(result, 0x8, GOOLIndex);
            BitConv.ToInt16(result, 0xA, EventHook);
            BitConv.ToInt16(result, 0xC, TransHook);
            BitConv.ToInt16(result, 0xE, CodeHook);
            return result;
        }
    }
}
