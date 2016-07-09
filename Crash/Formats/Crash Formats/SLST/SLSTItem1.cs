using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SLSTItem1
    {
        public static SLSTItem1 Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 8)
            {
                ErrorManager.SignalError("SLSTItem: Data is too short");
            }
            short count = BitConv.FromInt16(data,0);
            short unknown1 = BitConv.FromInt16(data,2);
            short removenodeindex = BitConv.FromInt16(data, 4);
            short swapnodeindex = BitConv.FromInt16(data, 6);
            if (count < 0)
            {
                ErrorManager.SignalError("SLSTItem: Value count is negative");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("SLSTItem: Data is too short");
            }
            short[] values = new short [count - 2];
            for (int i = 0;i < count - 2;i++)
            {
                values[i] = BitConv.FromInt16(data,8 + i * 2);
            }
            return new SLSTItem1(unknown1,removenodeindex,swapnodeindex,values);
        }
        
        private short unknown1;
        private short removenodeindex;
        private short swapnodeindex;
        private List<short> values;

        public SLSTItem1(short unknown1,short removenodeindex,short swapnodeindex,IEnumerable<short> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            this.unknown1 = unknown1;
            this.removenodeindex = removenodeindex;
            this.swapnodeindex = swapnodeindex;
            this.values = new List<short>(values);
        }

        public short Unknown1
        {
            get { return unknown1; }
        }

        public short RemoveNodeIndex
        {
            get { return removenodeindex; }
        }

        public short SwapNodeIndex
        {
            get { return swapnodeindex; }
        }

        public IList<short> Values
        {
            get { return values; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [8 + values.Count * 2];
            if (values.Count > short.MaxValue)
            {
                throw new Exception();
            }
            BitConv.ToInt16(data,0,(short)values.Count);
            BitConv.ToInt16(data,2,unknown1);
            BitConv.ToInt16(data,4,removenodeindex);
            BitConv.ToInt16(data,6,swapnodeindex);
            for (int i = 0;i < values.Count - 2;i++)
            {
                BitConv.ToInt16(data,8 + i * 2,values[i]);
            }
            return data;
        }
    }
}
