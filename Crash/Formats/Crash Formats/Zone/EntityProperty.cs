using System;
using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public abstract class EntityProperty
    {
        private static Dictionary<byte,EntityPropertyLoader> loaders;

        static EntityProperty()
        {
            loaders = new Dictionary<byte,EntityPropertyLoader>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (EntityPropertyTypeAttribute attribute in type.GetCustomAttributes(typeof(EntityPropertyTypeAttribute),false))
                {
                    EntityPropertyLoader loader = (EntityPropertyLoader)Activator.CreateInstance(type);
                    loaders.Add(attribute.Type,loader);
                }
            }
        }

        public static EntityProperty Load(byte type,byte elementsize,short unknown,bool last,byte[] data)
        {
            if (((type & 128) != 0) != last)
            {
                ErrorManager.SignalIgnorableError("EntityProperty: Flag 128 has an unexpected value");
            }
            bool issparse = (type & 64) != 0;
            bool hasmetavalues = (type & 32) != 0;
            type &= 31;
            if (loaders.ContainsKey(type))
            {
                return loaders[type].Load(elementsize,unknown,issparse,hasmetavalues,data);
            }
            else
            {
                return new EntityUnknownProperty(type,elementsize,unknown,issparse,hasmetavalues,data);
            }
        }

        private static bool LoadFromFieldOf<T>(out EntityProperty property,object obj,Type type) where T : struct
        {
            if (obj is T?)
            {
                T? value = (T?)obj;
                if (value.HasValue)
                {
                    EntityBasicProperty<T> p = (EntityBasicProperty<T>)Activator.CreateInstance(type);
                    EntityPropertyRow<T> row = new EntityPropertyRow<T>();
                    row.Values.Add(value.Value);
                    p.Rows.Add(row);
                    property = p;
                }
                else
                {
                    property = null;
                }
                return true;
            }
            else if (obj is List<T>)
            {
                List<T> values = (List<T>)obj;
                if (values.Count > 0)
                {
                    EntityBasicProperty<T> p = (EntityBasicProperty<T>)Activator.CreateInstance(type);
                    EntityPropertyRow<T> row = new EntityPropertyRow<T>();
                    foreach (T value in values)
                    {
                        row.Values.Add(value);
                    }
                    p.Rows.Add(row);
                    property = p;
                }
                else
                {
                    property = null;
                }
                return true;
            }
            else
            {
                property = null;
                return false;
            }
        }

        internal static EntityProperty LoadFromField(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else if (obj is EntityProperty)
            {
                return (EntityProperty)obj;
            }
            else if (obj is string)
            {
                List<byte> bytestr = new List<byte>(System.Text.Encoding.UTF8.GetBytes((string)obj));
                bytestr.Add(0);
                return LoadFromField(bytestr);
            }
            else if (obj is EntityID?)
            {
                EntityID? value = (EntityID?)obj;
                if (value.HasValue)
                {
                    EntityInt32Property p = new EntityInt32Property();
                    EntityPropertyRow<int> row = new EntityPropertyRow<int>();
                    row.Values.Add(value.Value.ID);
                    p.Rows.Add(row);
                    if (value.Value.AlternateID.HasValue)
                    {
                        EntityPropertyRow<int> row2 = new EntityPropertyRow<int>();
                        row2.Values.Add(value.Value.AlternateID.Value);
                        p.Rows.Add(row2);
                    }
                    return p;
                }
                else
                {
                    return null;
                }
            }
            EntityProperty property;
            if (
                LoadFromFieldOf<byte>(out property,obj,typeof(EntityUInt8Property)) ||
                LoadFromFieldOf<EntityVictim>(out property,obj,typeof(EntityVictimProperty)) ||
                LoadFromFieldOf<int>(out property,obj,typeof(EntityInt32Property)) ||
                LoadFromFieldOf<EntitySetting>(out property,obj,typeof(EntitySettingProperty)) ||
                LoadFromFieldOf<EntityPosition>(out property,obj,typeof(EntityPositionProperty)))
            {
                return property;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public abstract byte Type
        {
            get;
        }

        public abstract byte ElementSize
        {
            get;
        }

        public abstract short Unknown
        {
            get;
        }

        public abstract bool IsSparse
        {
            get;
        }

        public abstract bool HasMetaValues
        {
            get;
        }
        
        internal virtual void LoadToField(object obj,FieldInfo field)
        {
            ErrorManager.SignalError("EntityProperty: Type mismatch");
        }

        public abstract byte[] Save();
    }
}
