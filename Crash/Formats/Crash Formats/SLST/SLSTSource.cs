using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SLSTSource
    {
        public static SLSTSource Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length < 4)
            {
                ErrorManager.SignalError("SLSTSource: Data is too short");
            }
            short count = BitConv.FromInt16(data,0);
            short type = BitConv.FromInt16(data,2);
            if (count < 0)
            {
                ErrorManager.SignalError("SLSTSource: Value count is negative");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("SLSTSource: Data is too short");
            }
            if (type != 0)
            {
                ErrorManager.SignalError("SLSTSource: Type is wrong");
            }
            SLSTPolygonID[] polygons = new SLSTPolygonID[count];
            for (int i = 0;i < count;i++)
            {
                short id = (short)(BitConv.FromInt16(data,4 + i * 2) & 0x07FF);
                byte state = (byte)((data[5+i*2] >> 3) & 0x3);
                byte world = (byte)((data[5+i*2] >> 5) & 0x7);
                polygons[i] = new SLSTPolygonID(id,state,world);
            }
            return new SLSTSource(polygons);
        }

        private List<SLSTPolygonID> polygons;

        public SLSTSource(IEnumerable<SLSTPolygonID> polygons)
        {
            if (polygons == null)
                throw new ArgumentNullException("polygons");
            this.polygons = new List<SLSTPolygonID>(polygons);
        }

        public IList<SLSTPolygonID> Polygons => polygons;

        public byte[] Save()
        {
            byte[] data = new byte [4 + polygons.Count * 2];
            if (polygons.Count > short.MaxValue)
            {
                throw new Exception();
            }
            BitConv.ToInt16(data,0,(short)polygons.Count);
            BitConv.ToInt16(data,2,0);
            for (int i = 0;i < polygons.Count;i++)
            {
                BitConv.ToInt16(data,4+i*2,(short)((ushort)polygons[i].ID | polygons[i].State << 11 | polygons[i].World << 13));
            }
            return data;
        }
    }
}
