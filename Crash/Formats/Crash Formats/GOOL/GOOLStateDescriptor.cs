namespace Crash
{
    public struct GOOLStateDescriptor
    {
        public GOOLStateDescriptor(int stateflags,int cflags,short goolid,short epc,short tpc,short cpc)
        {
            StateFlags = stateflags;
            CFlags = cflags;
            GOOLID = goolid;
            EPC = epc;
            TPC = tpc;
            CPC = cpc;
        }

        public int StateFlags { get; }
        public int CFlags { get; }
        public short GOOLID { get; }
        public short EPC { get; }
        public short TPC { get; }
        public short CPC { get; }

        public byte[] Save()
        {
            byte[] result = new byte[16];
            BitConv.ToInt32(result,0x0,StateFlags);
            BitConv.ToInt32(result,0x4,CFlags);
            BitConv.ToInt16(result,0x8,GOOLID);
            BitConv.ToInt16(result,0xA,EPC);
            BitConv.ToInt16(result,0xC,TPC);
            BitConv.ToInt16(result,0xE,CPC);
            return result;
        }
    }
}
