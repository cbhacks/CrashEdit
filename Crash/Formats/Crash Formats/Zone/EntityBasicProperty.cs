using System.Reflection;

namespace CrashEdit.Crash
{
    public abstract class EntityBasicProperty<T> : EntityProperty where T : struct
    {
        public EntityBasicProperty()
        {
            Rows = new();
        }

        public EntityBasicProperty(IEnumerable<EntityPropertyRow<T>> rows)
        {
            Rows = new(rows);
        }

        public EntityBasicProperty(T value)
        {
            var row = new EntityPropertyRow<T>();
            row.Values.Add(value);
            Rows = [row];
        }

        public override sealed short RowCount => (short)Rows.Count;
        
        /// <summary>
        /// If true, not all rows may be of the same length.
        /// </summary>
        public override bool IsSparse
        {
            get
            {
                int? lastcount = null;
                foreach (EntityPropertyRow<T> row in Rows)
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

        /// <summary>
        /// If true, each row has an assigned keyframe.
        /// </summary>
        public override bool HasKeyframes
        {
            get
            {
                foreach (EntityPropertyRow<T> row in Rows)
                {
                    if (row.Keyframe.HasValue)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public List<EntityPropertyRow<T>> Rows { get; }

        public T GetSingleValue() => Rows[0].Values[0];

        internal override void LoadToField(object obj, FieldInfo field)
        {
            if (field.FieldType == typeof(T?))
            {
                if (Rows.Count == 1)
                {
                    if (Rows[0].Keyframe == null)
                    {
                        if (Rows[0].Values.Count == 1)
                        {
                            field.SetValue(obj, Rows[0].Values[0]);
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
            else if (field.FieldType == typeof(List<T>))
            {
                if (Rows.Count == 1)
                {
                    if (Rows[0].Keyframe == null)
                    {
                        List<T> list = new();
                        list.AddRange(Rows[0].Values);
                        field.SetValue(obj, list);
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
            else if (field.FieldType == GetType())
            {
                field.SetValue(obj, this);
            }
            else
            {
                base.LoadToField(obj, field);
            }
        }

        public override byte[] Save()
        {
            int length = IsSparse ? Rows.Count * 2 : 2;
            if (HasKeyframes)
            {
                length += Rows.Count * 2;
            }
            Aligner.Align(ref length, 4);
            foreach (EntityPropertyRow<T> row in Rows)
            {
                length += row.Values.Count * ElementSize;
            }
            Aligner.Align(ref length, 4);
            byte[] data = new byte[length];
            int offset = 0;
            if (IsSparse)
            {
                foreach (EntityPropertyRow<T> row in Rows)
                {
                    BitConv.ToInt16(data, offset, (short)row.Values.Count);
                    offset += 2;
                }
            }
            else if (Rows.Count == 0)
            {
                BitConv.ToInt16(data, offset, 0);
                offset += 2;
            }
            else
            {
                BitConv.ToInt16(data, offset, (short)Rows[0].Values.Count);
                offset += 2;
            }
            if (HasKeyframes)
            {
                foreach (EntityPropertyRow<T> row in Rows)
                {
                    if (!row.Keyframe.HasValue)
                    {
                        throw new InvalidOperationException("EntityPropertyRow Keyframes must either be consistently present or non-present.");
                    }
                    BitConv.ToInt16(data, offset, row.Keyframe.Value);
                    offset += 2;
                }
            }
            Aligner.Align(ref offset, 4);
            byte[] elementdata = new byte[ElementSize];
            foreach (EntityPropertyRow<T> row in Rows)
            {
                foreach (T value in row.Values)
                {
                    SaveElement(elementdata, value);
                    elementdata.CopyTo(data, offset);
                    offset += ElementSize;
                }
            }
            return data;
        }

        protected abstract void SaveElement(byte[] data, T value);
    }
}
