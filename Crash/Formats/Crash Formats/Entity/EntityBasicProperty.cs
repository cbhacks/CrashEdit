using System;

namespace Crash
{
    public abstract class EntityBasicProperty<T> : EntityProperty
    {
        private T[,] values;

        public EntityBasicProperty(T[,] values)
        {
            this.values = values;
        }

        public override sealed short Unknown
        {
            get { return (short)values.GetLength(0); }
        }

        public short ElementCount
        {
            get { return (short)values.GetLength(1); }
        }

        public T[,] Values
        {
            get { return values; }
        }

        public override byte[] Save()
        {
            int length = 4 + ElementSize * values.Length;
            Aligner.Align(ref length,4);
            byte[] data = new byte [length];
            BitConv.ToInt16(data,0,ElementCount);
            int offset = 4;
            for (int i = 0;i < Unknown;i++)
            {
                for (int j = 0;j < ElementCount;j++)
                {
                    byte[] elementdata = new byte [ElementSize];
                    SaveElement(elementdata,values[i,j]);
                    elementdata.CopyTo(data,offset);
                    offset += ElementSize;
                }
            }
            return data;
        }

        protected abstract void SaveElement(byte[] data,T value);
    }
}
