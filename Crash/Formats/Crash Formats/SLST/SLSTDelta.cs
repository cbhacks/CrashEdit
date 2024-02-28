namespace CrashEdit.Crash
{
    public sealed class SLSTDelta
    {
        public static SLSTDelta Load(byte[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (data.Length < 8)
            {
                ErrorManager.SignalError("SLSTDelta: Data is too short");
            }
            short count = BitConv.FromInt16(data, 0);
            short type = BitConv.FromInt16(data, 2);
            short addnodeindex = BitConv.FromInt16(data, 4);
            short swapnodeindex = BitConv.FromInt16(data, 6);
            if (type != 1)
            {
                ErrorManager.SignalError("SLSTDelta: Type is wrong");
            }
            if (count < 2)
            {
                ErrorManager.SignalError("SLSTDelta: Node count is invalid");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("SLSTDelta: Data is too short");
            }
            if (addnodeindex > count || addnodeindex < 2)
            {
                ErrorManager.SignalError("SLSTDelta: Add node index is out of bounds");
            }
            if (swapnodeindex > count || swapnodeindex < addnodeindex)
            {
                ErrorManager.SignalError("SLSTDelta: Swap node index is out of bounds");
            }
            short[] removenodes = new short[addnodeindex - 2];
            for (int i = 0; i < addnodeindex - 2; ++i)
            {
                removenodes[i] = BitConv.FromInt16(data, 8 + i * 2);
            }
            short[] addnodes = new short[swapnodeindex - addnodeindex];
            for (int i = addnodeindex - 2, j = 0; i < swapnodeindex - 2; ++i, ++j)
            {
                addnodes[j] = BitConv.FromInt16(data, 8 + i * 2);
            }
            short[] swapnodes = new short[count - swapnodeindex];
            for (int i = swapnodeindex - 2, j = 0; i < count - 2; ++i, ++j)
            {
                swapnodes[j] = BitConv.FromInt16(data, 8 + i * 2);
            }
            return new SLSTDelta(removenodes, addnodes, swapnodes);
        }

        private List<short> removenodes;
        private List<short> addnodes;
        private List<short> swapnodes;

        public SLSTDelta(IEnumerable<short> removenodes, IEnumerable<short> addnodes, IEnumerable<short> swapnodes)
        {
            ArgumentNullException.ThrowIfNull(removenodes);
            ArgumentNullException.ThrowIfNull(addnodes);
            ArgumentNullException.ThrowIfNull(swapnodes);
            this.removenodes = new List<short>(removenodes);
            this.addnodes = new List<short>(addnodes);
            this.swapnodes = new List<short>(swapnodes);
        }

        public IList<short> RemoveNodes => removenodes;
        public IList<short> AddNodes => addnodes;
        public IList<short> SwapNodes => swapnodes;

        public byte[] Save()
        {
            int nodecount = removenodes.Count + addnodes.Count + swapnodes.Count + 2;
            if (nodecount > short.MaxValue)
            {
                throw new Exception();
            }
            byte[] data = new byte[4 + nodecount * 2];
            BitConv.ToInt16(data, 0, (short)nodecount);
            BitConv.ToInt16(data, 2, 1);
            BitConv.ToInt16(data, 4, (short)(removenodes.Count + 2));
            BitConv.ToInt16(data, 6, (short)(removenodes.Count + addnodes.Count + 2));
            for (int i = 0; i < removenodes.Count; ++i)
            {
                BitConv.ToInt16(data, 8 + i * 2, removenodes[i]);
            }
            for (int i = 0; i < addnodes.Count; ++i)
            {
                BitConv.ToInt16(data, 8 + i * 2 + removenodes.Count * 2, addnodes[i]);
            }
            for (int i = 0; i < swapnodes.Count; ++i)
            {
                BitConv.ToInt16(data, 8 + i * 2 + removenodes.Count * 2 + addnodes.Count * 2, swapnodes[i]);
            }
            return data;
        }
    }
}
