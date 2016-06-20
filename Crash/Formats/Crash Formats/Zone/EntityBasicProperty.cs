using System;
using System.Reflection;
using System.Collections.Generic;

namespace Crash
{
    public abstract class EntityBasicProperty<T> : EntityProperty where T : struct
    {
        private List<EntityPropertyRow<T>> rows;

        public EntityBasicProperty()
        {
            rows = new List<EntityPropertyRow<T>>();
        }

        public EntityBasicProperty(IEnumerable<EntityPropertyRow<T>> rows)
        {
            this.rows = new List<EntityPropertyRow<T>>(rows);
        }

        public override sealed short Unknown
        {
            get { return (short)rows.Count; }
        }

        public override bool IsSparse
        {
            get
            {
                int? lastcount = null;
                foreach (EntityPropertyRow<T> row in rows)
                {
                    if (!lastcount.HasValue)
                    {
                        lastcount = row.Values.Count;
                    }
                    else
                    {
                        if (lastcount.Value != row.Values.Count)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public override bool HasMetaValues
        {
            get
            {
                foreach (EntityPropertyRow<T> row in rows)
                {
                    if (row.MetaValue.HasValue)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public List<EntityPropertyRow<T>> Rows
        {
            get { return rows; }
        }

        internal override void LoadToField(object obj,FieldInfo field)
        {
            if (field.FieldType == typeof(T?))
            {
                if (rows.Count == 1)
                {
                    if (rows[0].MetaValue == null)
                    {
                        if (rows[0].Values.Count == 1)
                        {
                            field.SetValue(obj,rows[0].Values[0]);
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
            else if (field.FieldType == typeof(List<T>))
            {
                if (rows.Count == 1)
                {
                    if (rows[0].MetaValue == null)
                    {
                        List<T> list = new List<T>();
                        list.AddRange(rows[0].Values);
                        field.SetValue(obj,list);
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
            else if (field.FieldType == GetType())
            {
                field.SetValue(obj,this);
            }
            else
            {
                base.LoadToField(obj,field);
            }
        }

        public override byte[] Save()
        {
            if (rows.Count == 0)
                return new byte [0];
            int length;
            if (IsSparse)
            {
                length = rows.Count * 2;
            }
            else
            {
                length = 2;
            }
            if (HasMetaValues)
            {
                length += rows.Count * 2;
            }
            Aligner.Align(ref length,4);
            foreach (EntityPropertyRow<T> row in rows)
            {
                length += row.Values.Count * ElementSize;
            }
            Aligner.Align(ref length,4);
            byte[] data = new byte [length];
            int offset = 0;
            if (IsSparse)
            {
                foreach (EntityPropertyRow<T> row in rows)
                {
                    BitConv.ToInt16(data,offset,(short)row.Values.Count);
                    offset += 2;
                }
            }
            else
            {
                BitConv.ToInt16(data,offset,(short)rows[0].Values.Count);
                offset += 2;
            }
            if (HasMetaValues)
            {
                foreach (EntityPropertyRow<T> row in rows)
                {
                    if (!row.MetaValue.HasValue)
                    {
                        throw new InvalidOperationException("EntityPropertyRow MetaValues must be consistently present or non-present.");
                    }
                    BitConv.ToInt16(data,offset,row.MetaValue.Value);
                    offset += 2;
                }
            }
            Aligner.Align(ref offset,4);
            byte[] elementdata = new byte [ElementSize];
            foreach (EntityPropertyRow<T> row in rows)
            {
                foreach (T value in row.Values)
                {
                    SaveElement(elementdata,value);
                    elementdata.CopyTo(data,offset);
                    offset += ElementSize;
                }
            }
            return data;
        }

        protected abstract void SaveElement(byte[] data,T value);
    }
}
