using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class EntityBasicPropertyLoader<T> : EntityPropertyLoader where T : struct
    {
        public sealed override EntityProperty Load(byte elementsize,short unknown,bool issparse,bool hasmetavalues,byte[] data)
        {
            if (elementsize != ElementSize)
            {
                ErrorManager.SignalError("EntityProperty: Element size is wrong");
            }
            if (unknown < 0)
            {
                ErrorManager.SignalError("EntityProperty: Unknown value is invalid");
            }
            List<EntityPropertyRow<T>> rows = new List<EntityPropertyRow<T>>();
            for (int i = 0;i < unknown;i++)
            {
                rows.Add(new EntityPropertyRow<T>());
            }
            int offset = 0;
            if (issparse)
            {
                if (offset + 2 * unknown > data.Length)
                {
                    ErrorManager.SignalError("EntityProperty: Not enough data");
                }
                foreach (EntityPropertyRow<T> row in rows)
                {
                    int valuecount = (ushort)BitConv.FromInt16(data,offset);
                    offset += 2;
                    for (int i = 0;i < valuecount;i++)
                    {
                        row.Values.Add(new T());
                    }
                }
            }
            else
            {
                if (offset + 2 > data.Length)
                {
                    ErrorManager.SignalError("EntityProperty: Not enough data");
                }
                int valuecount = (ushort)BitConv.FromInt16(data,offset);
                offset += 2;
                foreach (EntityPropertyRow<T> row in rows)
                {
                    for (int i = 0;i < valuecount;i++)
                    {
                        row.Values.Add(new T());
                    }
                }
            }
            if (hasmetavalues)
            {
                if (offset + 2 * unknown > data.Length)
                {
                    ErrorManager.SignalError("EntityProperty: Not enough data");
                }
                foreach (EntityPropertyRow<T> row in rows)
                {
                    short metavalue = BitConv.FromInt16(data,offset);
                    offset += 2;
                    row.MetaValue = metavalue;
                }
            }
            Aligner.Align(ref offset,4);
            byte[] elementdata = new byte [elementsize];
            foreach (EntityPropertyRow<T> row in rows)
            {
                if (offset + row.Values.Count * elementsize > data.Length)
                {
                    ErrorManager.SignalError("EntityProperty: Not enough data");
                }
                for (int i = 0;i < row.Values.Count;i++)
                {
                    Array.Copy(data,offset,elementdata,0,elementsize);
                    offset += elementsize;
                    row.Values[i] = LoadElement(elementdata);
                }
            }
            Aligner.Align(ref offset,4);
            if (offset < data.Length)
            {
                ErrorManager.SignalIgnorableError("EntityProperty: More data than expected");
            }
            else if (offset > data.Length)
            {
                ErrorManager.SignalIgnorableError("EntityProperty: Less data than expected (missing padding)");
            }
            return Load(rows);
        }

        protected abstract byte ElementSize { get; }

        protected abstract T LoadElement(byte[] data);

        protected abstract EntityProperty Load(IEnumerable<EntityPropertyRow<T>> rows);
    }
}
