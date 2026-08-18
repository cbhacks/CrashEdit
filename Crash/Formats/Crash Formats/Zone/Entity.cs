using System.Reflection;

namespace CrashEdit.Crash
{
    public sealed class Entity : IResource
    {
        private static readonly Dictionary<short, FieldInfo> propertyfields;

        static Entity()
        {
            propertyfields = new Dictionary<short, FieldInfo>();
            foreach (FieldInfo field in typeof(Entity).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (EntityPropertyFieldAttribute attribute in field.GetCustomAttributes(typeof(EntityPropertyFieldAttribute), false))
                {
                    propertyfields.Add(attribute.ID, field);
                }
            }
        }

        public static Entity Load(byte[] data)
        {
            if (data.Length < 16)
            {
                ErrorManager.SignalError("Entity: Data is too short");
            }
            int length = BitConv.FromInt32(data, 0);
            int propertycount = BitConv.FromInt32(data, 12);
            if (length != data.Length)
            {
                ErrorManager.SignalIgnorableError("Entity: Length field mismatch");
            }
            if (propertycount < 0 || propertycount > ushort.MaxValue)
            {
                ErrorManager.SignalError("Entity: Property count is invalid");
            }
            if (data.Length < 16 + propertycount * 8)
            {
                ErrorManager.SignalError("Entity: Data is too short");
            }
            Dictionary<short, EntityProperty> properties = new Dictionary<short, EntityProperty>();
            for (int i = 0; i < propertycount; i++)
            {
                short id = BitConv.FromInt16(data, 16 + i * 8);
                int offset = BitConv.FromUInt16(data, 18 + i * 8) + 12;
                int nextoffset = (i == propertycount - 1) ? data.Length : (BitConv.FromUInt16(data, 26 + i * 8) + 12);
                byte type = data[20 + i * 8];
                if (id == 0x103 && type == 0x13) type = 4; // force-fix a stupid bug                                                                                                                          
                if (((type & 128) != 0) != (i == propertycount - 1))
                {
                    ErrorManager.SignalIgnorableError("Entity: Property had terminate flag but wasn't final property");
                }
                byte elementsize = data[21 + i * 8];
                short unknown = BitConv.FromInt16(data, 22 + i * 8);
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
                    byte[] propertydata = new byte[nextoffset - offset];
                    Array.Copy(data, offset, propertydata, 0, propertydata.Length);
                    EntityProperty property = EntityProperty.Load(type, elementsize, unknown, propertydata);
                    properties.Add(id, property);
                }
            }
            return new Entity(properties);
        }

        //[EntityPropertyField(0x29)]
        private byte? mode_c2;
        private int? mode_c3;
        [EntityPropertyField(0x2C)]
        private string? name;
        [EntityPropertyField(0x32)]
        private int? zmod;
        [EntityPropertyField(0x4B)]
        private readonly List<EntityPosition> positions = null;
        [EntityPropertyField(0x9F)]
        private EntityID? id;
        [EntityPropertyField(0xA4)]
        private readonly List<EntityNumber> settings = null;
        [EntityPropertyField(0xA9)]
        private int? type;
        [EntityPropertyField(0xAA)]
        private int? subtype;
        [EntityPropertyField(0xC9)]
        private EntityNumber? avgdist;
        [EntityPropertyField(0x103)]
        private EntityChunkProperty? slst;
        [EntityPropertyField(0x109)]
        private EntityUInt32Property? neighbors = null;
        [EntityPropertyField(0x118)]
        private int? othersettings = null;
        [EntityPropertyField(0x130)]
        private EntityInt16Property? fov = null;
        [EntityPropertyField(0x13B)]
        private EntityInt32Property? drawlista = null;
        [EntityPropertyField(0x13C)]
        private EntityInt32Property? drawlistb = null;
        [EntityPropertyField(0x173)]
        private int? cameraindex = null;
        [EntityPropertyField(0x174)]
        private int? camerasubindex = null;
        [EntityPropertyField(0x208)]
        private EntityChunkProperty? loadlista = null;
        [EntityPropertyField(0x209)]
        private EntityChunkProperty? loadlistb = null;
        [EntityPropertyField(0x277)]
        private int? ddasettings = null;
        [EntityPropertyField(0x287)]
        private readonly List<short>? victims = null;
        [EntityPropertyField(0x288)]
        private int? ddasection = null;
        [EntityPropertyField(0x28B)]
        private EntityNumber? boxcount = null;
        [EntityPropertyField(0x30E)]
        private int? scaling = null;
        [EntityPropertyField(0x336)]
        private int? timetrialreward = null;
        [EntityPropertyField(0x337)]
        private EntityNumber? bonusboxcount = null;

        private readonly Dictionary<short, EntityProperty> extraproperties;

        public Entity(IDictionary<short, EntityProperty> properties)
        {
            // extraproperties has every property. we will remove a property from there and add it to a real field, leaving it with only the field-less ones.
            extraproperties = new Dictionary<short, EntityProperty>(properties);
            foreach (KeyValuePair<short, FieldInfo> pair in propertyfields)
            {
                short id = pair.Key;
                FieldInfo field = pair.Value;
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    field.SetValue(this, Activator.CreateInstance(field.FieldType));
                }
                else
                {
                    field.SetValue(this, null);
                }
                if (extraproperties.TryGetValue(id, out EntityProperty? property))
                {
                    property.LoadToField(this, field);
                    extraproperties.Remove(id);
                }
            }
            // some properties need special handling for stupid reasons
            if (extraproperties.TryGetValue(0x29, out var mode))
            {
                // 0x29 (camera mode) is u8 in Crash 2 and s32 in Crash 3
                if (mode is EntityInt32Property s32)
                {
                    mode_c3 = s32.GetSingleValue();
                }
                else if (mode is EntityUInt8Property u8)
                {
                    mode_c2 = u8.GetSingleValue();
                }
                else
                {
                    throw new Exception("Entity: Camera mode property is in an unknown format");
                }
                extraproperties.Remove(0x29);
            }
        }

        public string Title =>
            (Name != null && ID != null) ? $"{Name} [ID {ID}]" :
            (ID != null) ? $"Entity [ID {ID}]" :
            "Entity";

        public string ImageKey => "Arrow";

        public byte? ModeC2
        {
            get => mode_c2;
            set => mode_c2 = value;
        }

        public int? ModeC3
        {
            get => mode_c3;
            set => mode_c3 = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int? ZMod
        {
            get => zmod;
            set => zmod = value;
        }

        public IList<EntityPosition> Positions => positions;

        public int? ID
        {
            get => id.HasValue ? id.Value.ID : null;
            set
            {
                if (value != null)
                {
                    if (id.HasValue)
                    {
                        id = new EntityID(value.Value, id.Value.AlternateID);
                    }
                    else
                    {
                        id = new EntityID(value.Value);
                    }
                }
                else
                {
                    if (id.HasValue && id.Value.AlternateID.HasValue)
                    {
                        throw new InvalidOperationException();
                    }
                    else
                    {
                        id = null;
                    }
                }
            }
        }

        public int? AlternateID
        {
            get => id.HasValue ? id.Value.AlternateID : null;
            set
            {
                if (id != null)
                {
                    id = new EntityID(id.Value.ID, value);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public IList<EntityNumber> Settings => settings;

        public int? Type
        {
            get => type;
            set => type = value;
        }

        public int? Subtype
        {
            get => subtype;
            set => subtype = value;
        }

        public EntityNumber? AverageDistance
        {
            get => avgdist;
            set => avgdist = value;
        }

        public int? OtherSettings
        {
            get => othersettings;
            set => othersettings = value;
        }

        public EntityInt16Property FOV
        {
            get => fov;
            set => fov = value;
        }

        public EntityInt32Property DrawListA
        {
            get => drawlista;
            set => drawlista = value;
        }

        public EntityInt32Property DrawListB
        {
            get => drawlistb;
            set => drawlistb = value;
        }

        public int? CameraIndex
        {
            get => cameraindex;
            set => cameraindex = value;
        }

        public int? CameraSubIndex
        {
            get => camerasubindex;
            set => camerasubindex = value;
        }

        public EntityChunkProperty SLST
        {
            get => slst;
            set => slst = value;
        }

        public EntityUInt32Property Neighbors
        {
            get => neighbors;
            set => neighbors = value;
        }

        public EntityChunkProperty LoadListA
        {
            get => loadlista;
            set => loadlista = value;
        }

        public EntityChunkProperty LoadListB
        {
            get => loadlistb;
            set => loadlistb = value;
        }

        public int? DDASettings
        {
            get => ddasettings;
            set => ddasettings = value;
        }

        public List<short> Victims => victims;

        public int? DDASection
        {
            get => ddasection;
            set => ddasection = value;
        }

        public EntityNumber? BoxCount
        {
            get => boxcount;
            set => boxcount = value;
        }

        public int? Scaling
        {
            get => scaling;
            set => scaling = value;
        }

        public int? TimeTrialReward
        {
            get => timetrialreward;
            set => timetrialreward = value;
        }

        public EntityNumber? BonusBoxCount
        {
            get => bonusboxcount;
            set => bonusboxcount = value;
        }

        public IDictionary<short, EntityProperty> ExtraProperties => extraproperties;

        public byte[] Save()
        {
            if (LoadListA != null ^ LoadListB != null)
                ErrorManager.SignalIgnorableError("Entity: Entity contains one load list but not the other");
            if (DrawListA != null ^ DrawListB != null)
                ErrorManager.SignalIgnorableError("Entity: Entity contains one draw list but not the other");
            SortedDictionary<short, EntityProperty> properties = new SortedDictionary<short, EntityProperty>(extraproperties);
            foreach (KeyValuePair<short, FieldInfo> pair in propertyfields)
            {
                short id = pair.Key;
                FieldInfo field = pair.Value;
                EntityProperty property = EntityProperty.LoadFromField(field.GetValue(this));
                if (property != null)
                {
                    properties.Add(id, property);
                }
            }
            // special handling here
            if (mode_c2.HasValue)
            {
                properties.Add(0x29, new EntityUInt8Property(mode_c2.Value));
            }
            else if (mode_c3.HasValue)
            {
                properties.Add(0x29, new EntityInt32Property(mode_c3.Value));
            }

            byte[] header = new byte[16 + 8 * properties.Count];
            List<byte> result = new();
            int i = 0;
            int offset = header.Length - 12;
            foreach (KeyValuePair<short, EntityProperty> pair in properties)
            {
                EntityProperty property = pair.Value;
                BitConv.ToInt16(header, 16 + 8 * i + 0, pair.Key);
                unchecked
                {
                    BitConv.ToInt16(header, 16 + 8 * i + 2, (short)offset);
                }
                header[16 + 8 * i + 4] = (byte)((byte)property.Type | ((i == properties.Count - 1) ? 128 : 0) | (property.IsSparse ? 64 : 0) | (property.HasKeyframes ? 32 : 0));
                header[16 + 8 * i + 5] = property.ElementSize;
                BitConv.ToInt16(header, 16 + 8 * i + 6, property.RowCount);
                byte[] propertydata = property.Save();
                i++;
                offset += propertydata.Length;
                result.AddRange(propertydata);
            }
            BitConv.ToInt32(header, 0, offset + 12);
            BitConv.ToInt32(header, 4, 0);
            BitConv.ToInt32(header, 8, 0);
            BitConv.ToInt32(header, 12, properties.Count);
            result.InsertRange(0, header);
            return result.ToArray();
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class EntityPropertyFieldAttribute(short id) : Attribute
        {
            public short ID { get; } = id;
        }
    }
}
