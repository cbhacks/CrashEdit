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

        public IList<RIFFItem> Items
        {
            get { return items; }
        }

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

        public override byte[] Save()
        {
            List<byte> data = new List<byte>();
            data.Add((byte)Name[0]);
            data.Add((byte)Name[1]);
            data.Add((byte)Name[2]);
            data.Add((byte)Name[3]);
            foreach (RIFFItem item in items)
            {
                byte[] itemdata = item.Save();
                if (item is RIFF)
                {
                    itemdata[0] = (byte)'L';
                    itemdata[1] = (byte)'I';
                    itemdata[2] = (byte)'S';
                    itemdata[3] = (byte)'T';
                }
                data.AddRange(itemdata);
            }
            byte[] result = new byte [8 + data.Count];
            result[0] = (byte)'R';
            result[1] = (byte)'I';
            result[2] = (byte)'F';
            result[3] = (byte)'F';
            BitConv.ToInt32(result,4,data.Count);
            data.CopyTo(result,8);
            return result;
        }
    }
}
