using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class RIFF : RIFFItem
    {
        private List<RIFFItem> items;

        public RIFF(string name) : base(name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            items = new List<RIFFItem>();
        }

        public IList<RIFFItem> Items => items;

        public override int Length
        {
            get
            {
                int result = 12;
                foreach (RIFFItem item in items)
                {
                    result += item.Length;
                }
                return result;
            }
        }

        public override byte[] Save(Endianness endianness)
        {
            byte[] data = SaveBody(endianness);
            byte[] result = new byte [12 + data.Length];
            result[0] = (byte)'R';
            result[1] = (byte)'I';
            result[2] = (byte)'F';
            result[3] = (byte)'F';
            AutoBitConv.ToInt32(endianness,result,4,data.Length + 4);
            result[8] = (byte)Name[0];
            result[9] = (byte)Name[1];
            result[10] = (byte)Name[2];
            result[11] = (byte)Name[3];
            data.CopyTo(result,12);
            return result;
        }

        public byte[] SaveBody(Endianness endianness)
        {
            List<byte> data = new List<byte>();
            foreach (RIFFItem item in items)
            {
                byte[] itemdata = item.Save(endianness);
                if (item is RIFF)
                {
                    itemdata[0] = (byte)'L';
                    itemdata[1] = (byte)'I';
                    itemdata[2] = (byte)'S';
                    itemdata[3] = (byte)'T';
                }
                data.AddRange(itemdata);
            }
            return data.ToArray();
        }
    }
}
