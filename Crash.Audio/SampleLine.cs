namespace Crash.Audio
{
    public sealed class SampleLine
    {
        public static SampleLine Load(byte[] data)
        {
            if (data.Length != 16)
            {
                throw new System.Exception();
            }
            byte[] newdata = new byte [14];
            System.Array.Copy(data,2,newdata,0,14);
            return new SampleLine(data[0],(SampleLineType)data[1],newdata);
        }

        private byte info;
        private SampleLineType type;
        private byte[] data;

        public SampleLine(byte info,SampleLineType type,byte[] data)
        {
            if (data.Length != 14)
            {
                throw new System.Exception();
            }
            this.info = info;
            this.type = type;
            this.data = data;
        }

        public byte[] Save()
        {
            byte[] result = new byte [16];
            result[0] = info;
            result[1] = (byte)type;
            data.CopyTo(result,2);
            return result;
        }
    }
}
