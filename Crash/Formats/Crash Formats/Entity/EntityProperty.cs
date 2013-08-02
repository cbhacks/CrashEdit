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
            type &= 127;
            if (loaders.ContainsKey(type))
            {
                return loaders[type].Load(elementsize,unknown,data);
            }
            else
            {
                return new EntityUnknownProperty(type,elementsize,unknown,data);
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
        
        public abstract byte[] Save();
    }
}
