using System.Reflection;

namespace CrashEdit.Crash
{
    public abstract class EntityProperty
    {
        private static readonly Dictionary<byte, EntityPropertyLoader> loaders;

        static EntityProperty()
        {
            loaders = new();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (EntityPropertyTypeAttribute attribute in type.GetCustomAttributes(typeof(EntityPropertyTypeAttribute), false))
                {
                    EntityPropertyLoader loader = (EntityPropertyLoader)Activator.CreateInstance(type)!;
                    loaders.Add((byte)attribute.Type, loader);
                }
            }
        }

        public static EntityProperty Load(byte type, byte elementsize, short unknown, byte[] data)
        {
            bool issparse = (type & 64) != 0;
            bool haskeyframes = (type & 32) != 0;
            type &= 31;
            if (loaders.TryGetValue(type, out var loader))
            {
                return loader.Load(elementsize, unknown, issparse, haskeyframes, data);
            }
            else
            {
                Console.WriteLine($"Unknown entity property type {type}");
                return new EntityUnknownProperty((EntityPropertyType)type, elementsize, unknown, issparse, haskeyframes, data);
            }
        }

        private static bool LoadFromFieldOf<T>(out EntityProperty property, object obj, Type type) where T : struct
        {
            if (obj is T?)
            {
                T? value = (T?)obj;
                if (value.HasValue)
                {
                    EntityBasicProperty<T> p = (EntityBasicProperty<T>)Activator.CreateInstance(type);
                    EntityPropertyRow<T> row = new();
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
            else if (obj is List<T> values)
            {
                if (values.Count > 0)
                {
                    EntityBasicProperty<T> p = (EntityBasicProperty<T>)Activator.CreateInstance(type);
                    EntityPropertyRow<T> row = new();
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
            else if (obj is EntityProperty prop)
            {
                return prop;
            }
            else if (obj is string str)
            {
                List<byte> bytestr = new(System.Text.Encoding.UTF8.GetBytes(str)) { 0 };
                return LoadFromField(bytestr);
            }
            else if (obj is EntityID?)
            {
                EntityID? value = (EntityID?)obj;
                if (value.HasValue)
                {
                    EntityInt32Property p = new();
                    EntityPropertyRow<int> row = new();
                    row.Values.Add(value.Value.ID);
                    p.Rows.Add(row);
                    if (value.Value.AlternateID.HasValue)
                    {
                        EntityPropertyRow<int> row2 = new();
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
            if (
                LoadFromFieldOf<byte>(out EntityProperty property, obj, typeof(EntityUInt8Property)) ||
                LoadFromFieldOf<ushort>(out property, obj, typeof(EntityUInt16Property)) ||
                LoadFromFieldOf<uint>(out property, obj, typeof(EntityUInt32Property)) ||
                LoadFromFieldOf<sbyte>(out property, obj, typeof(EntityInt8Property)) ||
                LoadFromFieldOf<short>(out property, obj, typeof(EntityInt16Property)) ||
                LoadFromFieldOf<int>(out property, obj, typeof(EntityInt32Property)) ||
                LoadFromFieldOf<EntityNumber>(out property, obj, typeof(EntityNumberProperty)) ||
                LoadFromFieldOf<EntityPosition>(out property, obj, typeof(EntityPositionProperty)) ||
                LoadFromFieldOf<EntityVector32>(out property, obj, typeof(EntityVector32Property)))
            {
                return property;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public abstract EntityPropertyType Type { get; }
        public abstract byte ElementSize { get; }
        public abstract short RowCount { get; }
        public abstract bool IsSparse { get; }
        public abstract bool HasKeyframes { get; }

        internal virtual void LoadToField(object obj, FieldInfo field)
        {
            ErrorManager.SignalError("EntityProperty: Type mismatch");
        }

        public abstract byte[] Save();
    }

    public enum EntityPropertyType : byte
    {
        UInt8 = 1, // 8-bit unsigned (also used by UTF8 strings)
        UInt16 = 2, // 16-bit unsigned
        UInt32 = 3, // 32-bit unsigned
        Chunk = 4, // chunk ID
        Number = 5, // gool number
        Vector = 6, // 16-bit 3-element vector
        Vector32 = 7, // 32-bit 3-element vector
        Int8 = 0x11, // 8-bit signed
        Int16 = 0x12, // 16-bit signed
        Int32 = 0x13, // 32-bit signed (also used by IDs)
    }
}
