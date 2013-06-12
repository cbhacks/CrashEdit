using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class Entity
    {
        public static Entity Load(byte[] data)
        {
            if (data.Length < 16)
            {
                ErrorManager.SignalError("Entity: Data is too short");
            }
            int length = BitConv.FromInt32(data,0);
            int blank1 = BitConv.FromInt32(data,4);
            int blank2 = BitConv.FromInt32(data,8);
            int fieldcount = BitConv.FromInt32(data,12);
            if (length != data.Length)
            {
                ErrorManager.SignalIgnorableError("Entity: Length field mismatch");
            }
            if (blank1 != 0 || blank2 != 0)
            {
                ErrorManager.SignalIgnorableError("Entity: Blank value is wrong");
            }
            if (fieldcount < 0)
            {
                ErrorManager.SignalError("Entity: Field count is negative");
            }
            if (data.Length < 16 + fieldcount * 8)
            {
                ErrorManager.SignalError("Entity: Data is too short");
            }
            EntityField[] fields = new EntityField [fieldcount];
            ushort? lastend = null;
            for (int i = 0;i < fieldcount;i++)
            {
                short type = BitConv.FromInt16(data,16 + i * 8);
                int offset = (ushort)BitConv.FromInt16(data,18 + i * 8) + 12;
                byte unknown1 = data[20 + i * 8];
                byte elementsize = data[21 + i * 8];
                short unknown2 = BitConv.FromInt16(data,22 + i * 8);
                if (data.Length < offset + 4)
                {
                    ErrorManager.SignalError("Entity: Field begins out of bounds");
                }
                short elementcount = BitConv.FromInt16(data,offset);
                short unknown3 = BitConv.FromInt16(data,offset + 2);
                if (data.Length < offset + 4 + elementcount * elementsize)
                {
                    ErrorManager.SignalError("Entity: Field ends out of bounds");
                }
                byte[] fielddata = new byte [elementsize * elementcount];
                Array.Copy(data,offset + 4,fielddata,0,elementsize * elementcount);
                fields[i] = new EntityField(type,unknown1,elementsize,unknown2,elementcount,unknown3,fielddata);
                if (lastend != null)
                {
                    byte[] lastextradata = new byte [offset - (int)lastend];
                    Array.Copy(data,(int)lastend,lastextradata,0,lastextradata.Length);
                    fields[i - 1].ExtraData = lastextradata;
                }
                lastend = (ushort)(offset + 4 + fielddata.Length);
                if (i == fieldcount - 1)
                {
                    byte[] extradata = new byte [data.Length - (int)lastend];
                    Array.Copy(data,(int)lastend,extradata,0,extradata.Length);
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
                EntityField field = FindField(0x2C);
                if (field != null)
                {
                    return System.Text.Encoding.UTF8.GetString(field.Data,0,field.ElementCount - 1);
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<EntityPosition> Positions
        {
            get
            {
                List<EntityPosition> result = new List<EntityPosition>();
                EntityField field = FindField(0x4B);
                if (field != null)
                {
                    for (int i = 0;i < field.ElementCount;i++)
                    {
                        short x = BitConv.FromInt16(field.Data,6 * i + 0);
                        short y = BitConv.FromInt16(field.Data,6 * i + 2);
                        short z = BitConv.FromInt16(field.Data,6 * i + 4);
                        result.Add(new EntityPosition(x,y,z));
                    }
                }
                return result;
            }
        }

        public int? Type
        {
            get
            {
                EntityField field = FindField(0xA9);
                if (field != null)
                {
                    return BitConv.FromInt32(field.Data,0);
                }
                else
                {
                    return null;
                }
            }
        }

        public int? Subtype
        {
            get
            {
                EntityField field = FindField(0xAA);
                if (field != null)
                {
                    return BitConv.FromInt32(field.Data,0);
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<EntityField> Fields
        {
            get { return fields; }
        }

        public EntityField FindField(short type)
        {
            foreach (EntityField field in fields)
            {
                if (field.Type == type)
                {
                    return field;
                }
            }
            return null;
        }

        public byte[] Save()
        {
            int length = 16 + 8 * fields.Count;
            for (int i = 0;i < fields.Count;i++)
            {
                length += 4;
                length += fields[i].ElementSize * fields[i].ElementCount;
                length += fields[i].ExtraData.Length;
                Aligner.Align(ref length,4);
            }
            byte[] data = new byte [length];
            BitConv.ToInt32(data,0,length);
            BitConv.ToInt32(data,4,0);
            BitConv.ToInt32(data,8,0);
            BitConv.ToInt32(data,12,fields.Count);
            int offset = 16 + 8 * fields.Count;
            for (int i = 0;i < fields.Count;i++)
            {
                EntityField field = fields[i];
                BitConv.ToInt16(data,16 + 8 * i + 0,field.Type);
                BitConv.ToInt16(data,16 + 8 * i + 2,(short)(offset - 12));
                data[16 + 8 * i + 4] = field.Unknown1;
                data[16 + 8 * i + 5] = field.ElementSize;
                BitConv.ToInt16(data,16 + 8 * i + 6,field.Unknown2);
                BitConv.ToInt16(data,offset + 0,field.ElementCount);
                BitConv.ToInt16(data,offset + 2,field.Unknown3);
                offset += 4;
                field.Data.CopyTo(data,offset);
                offset += field.ElementCount * field.ElementSize;
                field.ExtraData.CopyTo(data,offset);
                offset += field.ExtraData.Length;
                Aligner.Align(ref offset,4);
            }
            if (offset != length)
            {
                throw new Exception();
            }
            return data;
        }
    }
}
