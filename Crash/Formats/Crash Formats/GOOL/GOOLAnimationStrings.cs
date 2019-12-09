using System;
using System.Collections.Generic;
using System.Text;

namespace Crash
{
    public sealed class GOOLAnimationStrings : GOOLAnimation
    {
        public static GOOLAnimationStrings Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            byte type = data[0];
            byte u1 = data[1];
            byte count = data[2];
            byte u2 = data[3];

            int eid = BitConv.FromInt32(data,4);
            int font = BitConv.FromInt32(data,8);

            string[] strings = new string[count];
            int off = 12;
            for (int i = 0; i < count; ++i)
            {
                var chars = Encoding.ASCII.GetChars(data,off,Chunk.Length);
                strings[i] = new string(chars);
                off += Encoding.ASCII.GetByteCount(chars);
            }

            return new GOOLAnimationStrings(eid,strings,type,u1,count,u2);
        }

        private List<string> strings;

        public GOOLAnimationStrings(int eid,IEnumerable<string> strings,byte type,byte u1,byte count,byte u2) : base(type,u1,count,u2)
        {
            EID = eid;
            this.strings = new List<string>(strings);
        }

        public int EID { get; }
        public GOOLSpriteFrame Back { get; }
        public IList<string> Strings => strings;

        public override byte[] Save()
        {
            List<byte> result = new List<byte>();
            result.Add(Type);
            result.Add(U1);
            result.Add(Count);
            result.Add(U2);
            byte[] temp = new byte [4];
            BitConv.ToInt32(temp,0,EID);
            result.AddRange(temp);
            for (int i = 0; i < strings.Count; ++i)
            {
                result.AddRange(Encoding.ASCII.GetBytes(strings[i]));
            }
            return result.ToArray();
        }
    }
}
