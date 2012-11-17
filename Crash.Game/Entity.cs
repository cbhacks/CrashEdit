using System.Reflection;
using System.Collections.Generic;

namespace Crash.Game
{
    public sealed class Entity
    {
        public static Entity Load(byte[] data)
        {
            if (data.Length < 16)
            {
                throw new System.Exception();
            }
            int length = BitConv.FromWord(data,0);
            int blank1 = BitConv.FromWord(data,4);
            int blank2 = BitConv.FromWord(data,8);
            int fieldcount = BitConv.FromWord(data,12);
            if (length != data.Length)
            {
                throw new System.Exception();
            }
            if (blank1 != 0 || blank2 != 0)
            {
                throw new System.Exception();
            }
            if (fieldcount < 0)
            {
                throw new System.Exception();
            }
            if (data.Length < 16 + fieldcount * 8)
            {
                throw new System.Exception();
            }
            EntityField[] fields = new EntityField [fieldcount];
            ushort? lastend = null;
            for (int i = 0;i < fieldcount;i++)
            {
                short type = BitConv.FromHalf(data,16 + i * 8);
                int offset = (ushort)BitConv.FromHalf(data,18 + i * 8) + 12;
                byte unknown1 = data[20 + i * 8];
                byte elementsize = data[21 + i * 8];
                short unknown2 = BitConv.FromHalf(data,22 + i * 8);
                if (data.Length < offset + 4)
                {
                    throw new System.Exception();
                }
                short elementcount = BitConv.FromHalf(data,offset);
                short unknown3 = BitConv.FromHalf(data,offset + 2);
                if (data.Length < offset + 4 + elementcount * elementsize)
                {
                    throw new System.Exception();
                }
                byte[] fielddata = new byte [elementsize * elementcount];
                System.Array.Copy(data,offset + 4,fielddata,0,elementsize * elementcount);
                fields[i] = new EntityField(type,unknown1,elementsize,unknown2,elementcount,unknown3,fielddata);
                if (lastend != null)
                {
                    byte[] lastextradata = new byte [offset - (int)lastend];
                    System.Array.Copy(data,(int)lastend,lastextradata,0,lastextradata.Length);
                    fields[i - 1].ExtraData = lastextradata;
                }
                lastend = (ushort)(offset + 4 + fielddata.Length);
                if (i == fieldcount - 1)
                {
                    byte[] extradata = new byte [data.Length - (int)lastend];
                    System.Array.Copy(data,(int)lastend,extradata,0,extradata.Length);
                    fields[i].ExtraData = extradata;
                }
            }
            return new Entity(fields);
        }

        private List<EntityField> fields;

        public Entity(IEnumerable<EntityField> fields)
        {
            this.fields = new List<EntityField>(fields);
        }

        public string Name
        {
            get
            {
                foreach (EntityField field in fields)
                {
                    if (field.Type == 0x2C)
                    {
                        return System.Text.Encoding.UTF8.GetString(field.Data,0,field.ElementCount - 1);
                    }
                }
                return null;
            }
        }

        public IList<EntityField> Fields
        {
            get { return fields; }
        }

        public byte[] Save()
        {
            int length = 16 + 8 * fields.Count;
            for (int i = 0;i < fields.Count;i++)
            {
                length += 4;
                length += fields[i].ElementSize * fields[i].ElementCount;
                length += fields[i].ExtraData.Length;
                length += length % 4;
            }
            byte[] data = new byte [length];
            BitConv.ToWord(data,0,length);
            BitConv.ToWord(data,4,0);
            BitConv.ToWord(data,8,0);
            BitConv.ToWord(data,12,fields.Count);
            int offset = 16 + 8 * fields.Count;
            for (int i = 0;i < fields.Count;i++)
            {
                EntityField field = fields[i];
                BitConv.ToHalf(data,16 + 8 * i + 0,field.Type);
                BitConv.ToHalf(data,16 + 8 * i + 2,(short)(offset - 12));
                data[16 + 8 * i + 4] = field.Unknown1;
                data[16 + 8 * i + 5] = field.ElementSize;
                BitConv.ToHalf(data,16 + 8 * i + 6,field.Unknown2);
                BitConv.ToHalf(data,offset + 0,field.ElementCount);
                BitConv.ToHalf(data,offset + 2,field.Unknown3);
                offset += 4;
                field.Data.CopyTo(data,offset);
                offset += field.ElementCount * field.ElementSize;
                field.ExtraData.CopyTo(data,offset);
                offset += field.ExtraData.Length;
                offset += offset % 4;
            }
            if (offset != length)
            {
                throw new System.Exception();
            }
            return data;
        }
    }
}
