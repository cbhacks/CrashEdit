using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SLSTItem0
    {
        public static SLSTItem0 Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 4)
            {
                ErrorManager.SignalError("SLSTItem: Data is too short");
            }
            short count = BitConv.FromInt16(data,0);
            short unknown1 = BitConv.FromInt16(data, 2);
            if (count < 0)
            {
                ErrorManager.SignalError("SLSTItem: Value count is negative");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("SLSTItem: Data is too short");
            }
            SLSTPolygonID[] values = new SLSTPolygonID[count];
            for (int i = 0;i < count;i++)
            {
                short id = (short)(BitConv.FromInt16(data,4 + i * 2) & 0x0FFF);
                short world = (short)((BitConv.FromInt16(data,4 + i * 2) & 0xF000) / 4096);
                values[i] = new SLSTPolygonID(id,world);
            }
            return new SLSTItem0(unknown1,values);
        }

        private short unknown1;
        private List<SLSTPolygonID> values;

        public SLSTItem0(short unknown1,IEnumerable<SLSTPolygonID> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            this.unknown1 = unknown1;
            this.values = new List<SLSTPolygonID>(values);
        }

        public short Unknown1
        {
            get { return unknown1; }
        }

        public IList<SLSTPolygonID> Values
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
                BitConv.ToInt16(data,4 + i * 2,(short)(values[i].ID + values[i].World * 4096));
            }
            return data;
        }
    }
}
