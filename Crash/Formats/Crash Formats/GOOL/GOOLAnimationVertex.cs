using System;

namespace Crash
{
    public sealed class GOOLAnimationVertex : GOOLAnimation
    {
        public static GOOLAnimationVertex Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            byte type = data[0];
            byte u1 = data[1];
            byte count = data[2];
            byte u2 = data[3];

            int eid = BitConv.FromInt32(data,4);

            return new GOOLAnimationVertex(eid,type,u1,count,u2);
        }

        public GOOLAnimationVertex(int eid,byte type,byte u1,byte count,byte u2) : base(type,u1,count,u2)
        {
            EID = eid;
        }

        public int EID { get; }

        public override byte[] Save()
        {
            byte[] result = new byte[8];
            result[0] = Type;
            result[1] = U1;
            result[2] = Count;
            result[3] = U2;
            BitConv.ToInt32(result,4,EID);
            return result;
        }
    }
}
