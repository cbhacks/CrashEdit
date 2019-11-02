using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityUInt8Property : EntityBasicProperty<byte>
    {
        public EntityUInt8Property()
        {
        }

        public EntityUInt8Property(IEnumerable<EntityPropertyRow<byte>> rows) : base(rows)
        {
        }

        public override byte Type => 1;
        public override byte ElementSize => 1;

        internal override void LoadToField(object obj,FieldInfo field)
        {
            if (field.FieldType == typeof(string))
            {
                if (Rows.Count == 1)
                {
                    if (Rows[0].MetaValue == null)
                    {
                        byte[] bytestr = new byte [Rows[0].Values.Count];
                        for (int i = 0;i < bytestr.Length;i++)
                        {
                            bytestr[i] = Rows[0].Values[i];
                        }
                        string str = System.Text.Encoding.UTF8.GetString(bytestr);
                        if (str.EndsWith("\0"))
                        {
                            str = str.Remove(str.Length - 1);
                        }
                        else
                        {
                            ErrorManager.SignalIgnorableError("EntityProperty: String is not null-terminated");
                        }
                        field.SetValue(obj,str);
                    }
                    else
                    {
                        ErrorManager.SignalError("EntityProperty: Property has an unexpected metavalue");
                    }
                }
                else
                {
                    ErrorManager.SignalError("EntityProperty: Property has more rows than expected");
                }
            }
            else
            {
                base.LoadToField(obj,field);
            }
        }

        protected override void SaveElement(byte[] data,byte value)
        {
            data[0] = value;
        }
    }
}
