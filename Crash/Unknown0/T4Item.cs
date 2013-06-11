using System;
using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T4Item
    {
        public static T4Item Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 4)
            {
                ErrorManager.SignalError("T4Item: Data is too short");
            }
            short count = BitConv.FromInt16(data,0);
            short unknown1 = BitConv.FromInt16(data,2);
            if (count < 0)
            {
                ErrorManager.SignalError("T4Item: Value count is negative");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("T4Item: Data is too short");
            }
            short[] values = new short [count];
            for (int i = 0;i < count;i++)
            {
                values[i] = BitConv.FromInt16(data,4 + i * 2);
            }
            return new T4Item(unknown1,values);
        }

        private short unknown1;
        private List<short> values;

        public T4Item(short unknown1,IEnumerable<short> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            this.unknown1 = unknown1;
            this.values = new List<short>(values);
        }

        public short Unknown1
        {
            get { return unknown1; }
        }

        public IList<short> Values
        {
            get { return values; }
        }

        public byte[] Save()
        {
            byte[] data = new byte [4 + values.Count * 2];
            if (values.Count > short.MaxValue)
            {
                throw new Exception();
            }
            BitConv.ToInt16(data,0,(short)values.Count);
            BitConv.ToInt16(data,2,unknown1);
            for (int i = 0;i < values.Count;i++)
            {
                BitConv.ToInt16(data,4 + i * 2,values[i]);
            }
            return data;
        }
    }
}
