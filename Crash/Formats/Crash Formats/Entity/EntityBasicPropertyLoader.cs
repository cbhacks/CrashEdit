using System;

namespace Crash
{
    public abstract class EntityBasicPropertyLoader<T> : EntityPropertyLoader
    {
        public sealed override EntityProperty Load(byte elementsize,short unknown,byte[] data)
        {
            if (elementsize != ElementSize)
            {
                ErrorManager.SignalError("EntityProperty: Element size is wrong");
            }
            if (unknown < 0)
            {
                ErrorManager.SignalError("EntityProperty: Unknown value is invalid");
            }
            int offset = 0;
            if (offset + 2 > data.Length)
            {
                ErrorManager.SignalError("EntityProperty: Not enough data");
            }
            int elementcount = (ushort)BitConv.FromInt16(data,offset);
            offset += 2;
            Aligner.Align(ref offset,4);
            T[,] values = new T [unknown,elementcount];
            if (offset + values.Length * elementsize > data.Length)
            {
                ErrorManager.SignalError("EntityProperty: Not enough data");
            }
            for (int i = 0;i < unknown;i++)
            {
                for (int j = 0;j < elementcount;j++)
                {
                    byte[] elementdata = new byte [elementsize];
                    Array.Copy(data,offset,elementdata,0,elementsize);
                    values[i,j] = LoadElement(elementdata);
                    offset += elementsize;
                }
            }
            Aligner.Align(ref offset,4);
            if (offset != data.Length)
            {
                ErrorManager.SignalIgnorableError("EntityProperty: More data than expected");
            }
            return Load(values);
        }

        protected abstract byte ElementSize
        {
            get;
        }

        protected abstract T LoadElement(byte[] data);

        protected abstract EntityProperty Load(T[,] values);
    }
}
