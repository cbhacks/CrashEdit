using System;

namespace Crash
{
    public abstract class EntityProperty
    {
        public static EntityProperty Load(byte type,byte elementsize,short unknown,bool last,byte[] data)
        {
            if (((type & 128) != 0) != last)
            {
                ErrorManager.SignalIgnorableError("EntityProperty: Flag 128 has an unexpected value");
            }
            type &= 127;
            return new EntityUnknownProperty(type,elementsize,unknown,data);
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
