using System;
using System.Collections.Generic;

namespace Crash.Unknown0
{
    public sealed class T4Item
    {
        public static T4Item Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null.");
            if (data.Length < 4)
            {
                throw new Exception();
            }
            short count = BitConv.FromHalf(data,0);
            short unknown1 = BitConv.FromHalf(data,2);
            if (count < 0)
            {
                throw new Exception();
            }
            if (data.Length < 4 + 2 * count)
            {
                throw new Exception();
            }
            short[] values = new short [count];
            for (int i = 0;i < count;i++)
            {
                values[i] = BitConv.FromHalf(data,4 + i * 2);
            }
            return new T4Item(unknown1,values);
        }

        private short unknown1;
        private List<short> values;

        public T4Item(short unknown1,IEnumerable<short> values)
        {
            if (values == null)
                throw new ArgumentNullException("Values cannot be null.");
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
            BitConv.ToHalf(data,0,(short)values.Count);
            BitConv.ToHalf(data,2,unknown1);
            for (int i = 0;i < values.Count;i++)
            {
                BitConv.ToHalf(data,4 + i * 2,values[i]);
            }
            return data;
        }
    }
}
