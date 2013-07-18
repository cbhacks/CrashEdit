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
            int propertycount = BitConv.FromInt32(data,12);
            if (length != data.Length)
            {
                ErrorManager.SignalIgnorableError("Entity: Length field mismatch");
            }
            if (blank1 != 0 || blank2 != 0)
            {
                ErrorManager.SignalIgnorableError("Entity: Blank value is wrong");
            }
            if (propertycount < 0)
            {
                ErrorManager.SignalError("Entity: Property count is negative");
            }
            if (data.Length < 16 + propertycount * 8)
            {
                ErrorManager.SignalError("Entity: Data is too short");
            }
            Dictionary<short,EntityProperty> properties = new Dictionary<short,EntityProperty>();
            for (int i = 0;i < propertycount;i++)
            {
                short id = BitConv.FromInt16(data,16 + i * 8);
                int offset = (ushort)BitConv.FromInt16(data,18 + i * 8) + 12;
                int nextoffset = (i == propertycount - 1) ? data.Length : ((ushort)BitConv.FromInt16(data,26 + i * 8) + 12);
                byte type = data[20 + i * 8];
                byte elementsize = data[21 + i * 8];
                short unknown = BitConv.FromInt16(data,22 + i * 8);
                if (offset > data.Length)
                {
                    ErrorManager.SignalError("Entity: Property begins out of bounds");
                }
                if (nextoffset < offset)
                {
                    ErrorManager.SignalError("Entity: Property ends before it begins");
                }
                if (nextoffset > data.Length)
                {
                    ErrorManager.SignalError("Entity: Property ends out of bounds");
                }
                if (properties.ContainsKey(id))
                {
                    ErrorManager.SignalIgnorableError("Entity: Duplicate property");
                }
                else
                {
                    byte[] propertydata = new byte [nextoffset - offset];
                    Array.Copy(data,offset,propertydata,0,propertydata.Length);
                    EntityProperty property = EntityProperty.Load(type,elementsize,unknown,i == propertycount - 1,propertydata);
                    properties.Add(id,property);
                }
            }
            return new Entity(properties);
        }

        private Dictionary<short,EntityProperty> properties;

        public Entity(IDictionary<short,EntityProperty> properties)
        {
            this.properties = new Dictionary<short,EntityProperty>(properties);
        }

        public string Name
        {
            get
            {
                /*EntityField field = FindField(0x2C);
                if (field != null)
                {
                    return System.Text.Encoding.UTF8.GetString(field.Data,0,field.ElementCount - 1);
                }
                else*/
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
                /*EntityField field = FindField(0x4B);
                if (field != null)
                {
                    for (int i = 0;i < field.ElementCount;i++)
                    {
                        short x = BitConv.FromInt16(field.Data,6 * i + 0);
                        short y = BitConv.FromInt16(field.Data,6 * i + 2);
                        short z = BitConv.FromInt16(field.Data,6 * i + 4);
                        result.Add(new EntityPosition(x,y,z));
                    }
                }*/
                return result;
            }
        }

        public int? Type
        {
            get
            {
                /*EntityField field = FindField(0xA9);
                if (field != null)
                {
                    return BitConv.FromInt32(field.Data,0);
                }
                else*/
                {
                    return null;
                }
            }
        }

        public int? Subtype
        {
            get
            {
                /*EntityField field = FindField(0xAA);
                if (field != null)
                {
                    return BitConv.FromInt32(field.Data,0);
                }
                else*/
                {
                    return null;
                }
            }
        }

        public IDictionary<short,EntityProperty> Properties
        {
            get { return properties; }
        }

        public byte[] Save()
        {
            byte[] header = new byte [16 + 8 * properties.Count];
            List<byte> result = new List<byte>();
            int i = 0;
            int offset = header.Length - 12;
            foreach (KeyValuePair<short,EntityProperty> pair in properties)
            {
                EntityProperty property = pair.Value;
                BitConv.ToInt16(header,16 + 8 * i + 0,pair.Key);
                unchecked
                {
                    BitConv.ToInt16(header,16 + 8 * i + 2,(short)offset);
                }
                header[16 + 8 * i + 4] = (byte)(property.Type | ((i == properties.Count - 1) ? 128 : 0));
                header[16 + 8 * i + 5] = property.ElementSize;
                BitConv.ToInt16(header,16 + 8 * i + 6,property.Unknown);
                byte[] propertydata = property.Save();
                i++;
                offset += propertydata.Length;
                result.AddRange(propertydata);
            }
            BitConv.ToInt32(header,0,offset + 12);
            BitConv.ToInt32(header,4,0);
            BitConv.ToInt32(header,8,0);
            BitConv.ToInt32(header,12,properties.Count);
            result.InsertRange(0,header);
            return result.ToArray();
        }
    }
}
