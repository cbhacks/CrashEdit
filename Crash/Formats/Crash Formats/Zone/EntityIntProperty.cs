using System.Reflection;

namespace CrashEdit.Crash
{
    public sealed class EntityInt8Property : EntityBasicProperty<sbyte>
    {
        public EntityInt8Property()
        {
        }

        public EntityInt8Property(IEnumerable<EntityPropertyRow<sbyte>> rows) : base(rows)
        {
        }

        public EntityInt8Property(sbyte value) : base(value)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.Int8;
        public override byte ElementSize => 1;

        protected override void SaveElement(byte[] data, sbyte value)
        {
            data[0] = (byte)value;
        }
    }

    public sealed class EntityInt16Property : EntityBasicProperty<short>
    {
        public EntityInt16Property()
        {
        }

        public EntityInt16Property(IEnumerable<EntityPropertyRow<short>> rows) : base(rows)
        {
        }

        public EntityInt16Property(short value) : base(value)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.Int16;
        public override byte ElementSize => 2;

        protected override void SaveElement(byte[] data, short value)
        {
            BitConv.ToInt16(data, 0, value);
        }
    }

    public sealed class EntityInt32Property : EntityBasicProperty<int>
    {
        public EntityInt32Property()
        {
        }

        public EntityInt32Property(IEnumerable<EntityPropertyRow<int>> rows) : base(rows)
        {
        }

        public EntityInt32Property(int value) : base(value)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.Int32;
        public override byte ElementSize => 4;

        internal override void LoadToField(object obj, FieldInfo field)
        {
            if (field.FieldType == typeof(EntityID?))
            {
                if (Rows.Count == 1)
                {
                    if (Rows[0].Keyframe == null)
                    {
                        if (Rows[0].Values.Count == 1)
                        {
                            field.SetValue(obj, new EntityID(Rows[0].Values[0]));
                        }
                        else
                        {
                            ErrorManager.SignalError("EntityProperty: Property has more values than expected");
                        }
                    }
                    else
                    {
                        ErrorManager.SignalError("EntityProperty: Property has an unexpected keyframe");
                    }
                }
                else if (Rows.Count == 2)
                {
                    if (Rows[0].Keyframe == null && Rows[1].Keyframe == null)
                    {
                        if (Rows[0].Values.Count == 1 && Rows[1].Values.Count == 1)
                        {
                            field.SetValue(obj, new EntityID(Rows[0].Values[0], Rows[1].Values[0]));
                        }
                        else
                        {
                            ErrorManager.SignalError("EntityProperty: Property has more values than expected");
                        }
                    }
                    else
                    {
                        ErrorManager.SignalError("EntityProperty: Property has an unexpected keyframe");
                    }
                }
                else
                {
                    ErrorManager.SignalError("EntityProperty: Property has more rows than expected");
                }
            }
            else
            {
                base.LoadToField(obj, field);
            }
        }

        protected override void SaveElement(byte[] data, int value)
        {
            BitConv.ToInt32(data, 0, value);
        }
    }

    public sealed class EntityUInt8Property : EntityBasicProperty<byte>
    {
        public EntityUInt8Property()
        {
        }

        public EntityUInt8Property(IEnumerable<EntityPropertyRow<byte>> rows) : base(rows)
        {
        }

        public EntityUInt8Property(byte value) : base(value)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.UInt8;
        public override byte ElementSize => 1;

        internal override void LoadToField(object obj, FieldInfo field)
        {
            if (field.FieldType == typeof(string))
            {
                if (Rows.Count == 1)
                {
                    if (Rows[0].Keyframe == null)
                    {
                        byte[] bytestr = new byte[Rows[0].Values.Count];
                        for (int i = 0; i < bytestr.Length; i++)
                        {
                            bytestr[i] = Rows[0].Values[i];
                        }
                        string str = System.Text.Encoding.UTF8.GetString(bytestr);
                        if (str.EndsWith('\0'))
                        {
                            str = str.Remove(str.Length - 1);
                        }
                        else
                        {
                            ErrorManager.SignalIgnorableError("EntityProperty: String is not null-terminated");
                        }
                        field.SetValue(obj, str);
                    }
                    else
                    {
                        ErrorManager.SignalError("EntityProperty: Property has an unexpected keyframe");
                    }
                }
                else
                {
                    ErrorManager.SignalError("EntityProperty: Property has more rows than expected");
                }
            }
            else
            {
                base.LoadToField(obj, field);
            }
        }

        protected override void SaveElement(byte[] data, byte value)
        {
            data[0] = value;
        }
    }

    public sealed class EntityUInt16Property : EntityBasicProperty<ushort>
    {
        public EntityUInt16Property()
        {
        }

        public EntityUInt16Property(IEnumerable<EntityPropertyRow<ushort>> rows) : base(rows)
        {
        }

        public EntityUInt16Property(ushort value) : base(value)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.UInt16;
        public override byte ElementSize => 2;

        protected override void SaveElement(byte[] data, ushort value)
        {
            BitConv.ToInt16(data, 0, (short)value);
        }
    }

    public sealed class EntityUInt32Property : EntityBasicProperty<uint>
    {
        public EntityUInt32Property()
        {
        }

        public EntityUInt32Property(IEnumerable<EntityPropertyRow<uint>> rows) : base(rows)
        {
        }

        public EntityUInt32Property(uint value) : base(value)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.UInt32;
        public override byte ElementSize => 4;

        protected override void SaveElement(byte[] data, uint value)
        {
            BitConv.ToInt32(data, 0, (int)value);
        }
    }
}
