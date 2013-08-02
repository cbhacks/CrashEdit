using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public sealed class Entity
    {
        private static Dictionary<short,FieldInfo> propertyfields;

        static Entity()
        {
            propertyfields = new Dictionary<short,FieldInfo>();
            foreach (FieldInfo field in typeof(Entity).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (EntityPropertyFieldAttribute attribute in field.GetCustomAttributes(typeof(EntityPropertyFieldAttribute),false))
                {
                    propertyfields.Add(attribute.ID,field);
                }
            }
        }

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

        [EntityPropertyField(0x2C)]
        private string name;
        [EntityPropertyField(0x4B)]
        private List<EntityPosition> positions;
        [EntityPropertyField(0xA9)]
        private int? type;
        [EntityPropertyField(0xAA)]
        private int? subtype;
        private Dictionary<short,EntityProperty> extraproperties;

        public Entity(IDictionary<short,EntityProperty> properties)
        {
            extraproperties = new Dictionary<short,EntityProperty>(properties);
            foreach (KeyValuePair<short,FieldInfo> pair in propertyfields)
            {
                short id = pair.Key;
                FieldInfo field = pair.Value;
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    field.SetValue(this,Activator.CreateInstance(field.FieldType));
                }
                else
                {
                    field.SetValue(this,null);
                }
                if (extraproperties.ContainsKey(id))
                {
                    EntityProperty property = extraproperties[id];
                    if (property.Unknown == 1)
                    {
                        if (field.FieldType == typeof(int?))
                        {
                            if (property is EntityInt32Property)
                            {
                                EntityInt32Property p = (EntityInt32Property)property;
                                if (p.ElementCount == 1)
                                {
                                    field.SetValue(this,p.Values[0,0]);
                                    extraproperties.Remove(id);
                                }
                                else
                                {
                                    ErrorManager.SignalIgnorableError("Entity: Property has more values than expected");
                                }
                            }
                            else
                            {
                                ErrorManager.SignalIgnorableError("Entity: Property type mismatch");
                            }
                        }
                        else if (field.FieldType == typeof(List<int>))
                        {
                            if (property is EntityInt32Property)
                            {
                                EntityInt32Property p = (EntityInt32Property)property;
                                for (int i = 0;i < p.ElementCount;i++)
                                {
                                    ((List<int>)field.GetValue(this)).Add(p.Values[0,i]);
                                }
                                extraproperties.Remove(id);
                            }
                            else
                            {
                                ErrorManager.SignalIgnorableError("Entity: Property type mismatch");
                            }
                        }
                        else if (field.FieldType == typeof(List<EntityPosition>))
                        {
                            if (property is EntityPositionProperty)
                            {
                                EntityPositionProperty p = (EntityPositionProperty)property;
                                for (int i = 0;i < p.ElementCount;i++)
                                {
                                    ((List<EntityPosition>)field.GetValue(this)).Add(p.Values[0,i]);
                                }
                                extraproperties.Remove(id);
                            }
                            else
                            {
                                ErrorManager.SignalIgnorableError("Entity: Property type mismatch");
                            }
                        }
                        else if (field.FieldType == typeof(string))
                        {
                            if (property is EntityUInt8Property)
                            {
                                EntityUInt8Property p = (EntityUInt8Property)property;
                                byte[] str = new byte [p.ElementCount];
                                for (int i = 0;i < str.Length;i++)
                                {
                                    str[i] = p.Values[0,i];
                                }
                                field.SetValue(this,Encoding.UTF8.GetString(str));
                                extraproperties.Remove(id);
                                // TODO :: Error handling on invalid UTF8 data
                            }
                            else
                            {
                                ErrorManager.SignalIgnorableError("Entity: Property type mismatch");
                            }
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        ErrorManager.SignalIgnorableError("Entity: Property is too wide");
                    }
                }
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public IList<EntityPosition> Positions
        {
            get { return positions; }
        }

        public int? Type
        {
            get { return type; }
            set { type = value; }
        }

        public int? Subtype
        {
            get { return subtype; }
            set { subtype = value; }
        }

        public IDictionary<short,EntityProperty> ExtraProperties
        {
            get { return extraproperties; }
        }

        public byte[] Save()
        {
            SortedDictionary<short,EntityProperty> properties = new SortedDictionary<short,EntityProperty>(extraproperties);
            foreach (KeyValuePair<short,FieldInfo> pair in propertyfields)
            {
                short id = pair.Key;
                FieldInfo field = pair.Value;
                if (field.FieldType == typeof(int?))
                {
                    int? value = (int?)field.GetValue(this);
                    if (value != null)
                    {
                        int[,] values = new int [1,1];
                        values[0,0] = value.Value;
                        properties.Add(id,new EntityInt32Property(values));
                    }
                }
                else if (field.FieldType == typeof(List<int>))
                {
                    List<int> list = (List<int>)field.GetValue(this);
                    if (list.Count > 0)
                    {
                        int[,] values = new int [1,list.Count];
                        for (int j = 0;j < list.Count;j++)
                        {
                            values[0,j] = list[j];
                        }
                        properties.Add(id,new EntityInt32Property(values));
                    }
                }
                else if (field.FieldType == typeof(List<EntityPosition>))
                {
                    List<EntityPosition> list = (List<EntityPosition>)field.GetValue(this);
                    if (list.Count > 0)
                    {
                        EntityPosition[,] values = new EntityPosition [1,list.Count];
                        for (int j = 0;j < list.Count;j++)
                        {
                            values[0,j] = list[j];
                        }
                        properties.Add(id,new EntityPositionProperty(values));
                    }
                }
                else if (field.FieldType == typeof(string))
                {
                    string value = (string)field.GetValue(this);
                    if (value != null)
                    {
                        byte[] stringdata = Encoding.UTF8.GetBytes(value);
                        byte[,] values = new byte [1,stringdata.Length];
                        for (int j = 0;j < stringdata.Length;j++)
                        {
                            values[0,j] = stringdata[j];
                        }
                        properties.Add(id,new EntityUInt8Property(values));
                    }
                }
            }
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

        [AttributeUsage(AttributeTargets.Field)]
        private class EntityPropertyFieldAttribute : Attribute
        {
            private short id;

            public EntityPropertyFieldAttribute(short id)
            {
                this.id = id;
            }

            public short ID
            {
                get { return id; }
            }
        }
    }
}
