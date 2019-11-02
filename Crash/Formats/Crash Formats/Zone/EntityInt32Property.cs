using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityInt32Property : EntityBasicProperty<int>
    {
        public EntityInt32Property()
        {
        }

        public EntityInt32Property(IEnumerable<EntityPropertyRow<int>> rows) : base(rows)
        {
        }

        public override byte Type => 19;
        public override byte ElementSize => 4;

        internal override void LoadToField(object obj,FieldInfo field)
        {
            if (field.FieldType == typeof(EntityID?))
            {
                if (Rows.Count == 1)
                {
                    if (Rows[0].MetaValue == null)
                    {
                        if (Rows[0].Values.Count == 1)
                        {
                            field.SetValue(obj,new EntityID(Rows[0].Values[0]));
                        }
                        else
                        {
                            ErrorManager.SignalError("EntityProperty: Property has more values than expected");
                        }
                    }
                    else
                    {
                        ErrorManager.SignalError("EntityProperty: Property has an unexpected metavalue");
                    }
                }
                else if (Rows.Count == 2)
                {
                    if (Rows[0].MetaValue == null && Rows[1].MetaValue == null)
                    {
                        if (Rows[0].Values.Count == 1 && Rows[1].Values.Count == 1)
                        {
                            field.SetValue(obj,new EntityID(Rows[0].Values[0],Rows[1].Values[0]));
                        }
                        else
                        {
                            ErrorManager.SignalError("EntityProperty: Property has more values than expected");
                        }
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

        protected override void SaveElement(byte[] data,int value)
        {
            BitConv.ToInt32(data,0,value);
        }
    }
}
