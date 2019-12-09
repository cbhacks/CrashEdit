using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class GOOLAnimationFont : GOOLAnimation
    {
        public static GOOLAnimationFont Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            byte type = data[0];
            byte u1 = data[1];
            byte count = data[2];
            byte u2 = data[3];

            int eid = BitConv.FromInt32(data,4);

            GOOLChar[] charmap = new GOOLChar[count];
            for (int i = 0; i < count; ++i)
            {
                charmap[i] = new GOOLChar(
                    new GOOLSpriteFrame(BitConv.FromInt32(data,8+i*12+0),BitConv.FromInt32(data,8+i*12+4)),
                    BitConv.FromInt16(data,8+i*12+8),BitConv.FromInt16(data,8+i*12+10));
            }

            GOOLSpriteFrame back = new GOOLSpriteFrame(BitConv.FromInt32(data,8+count*12+0),BitConv.FromInt32(data,8+count*12+4));

            return new GOOLAnimationFont(eid,charmap,back,type,u1,count,u2);
        }

        private List<GOOLChar> charmap;

        public GOOLAnimationFont(int eid,IEnumerable<GOOLChar> charmap,GOOLSpriteFrame back,byte type,byte u1,byte count,byte u2) : base(type,u1,count,u2)
        {
            EID = eid;
            this.charmap = new List<GOOLChar>(charmap);
            Back = back;
        }

        public int EID { get; }
        public GOOLSpriteFrame Back { get; }
        public IList<GOOLChar> Frames => charmap;

        public override byte[] Save()
        {
            byte[] result = new byte[16 + charmap.Count*12];
            result[0] = Type;
            result[1] = U1;
            result[2] = Count;
            result[3] = U2;
            BitConv.ToInt32(result,4,EID);
            for (int i = 0; i < charmap.Count; ++i)
            {
                BitConv.ToInt32(result,8 + i*12 + 0,charmap[i].Sprite.CLUT);
                BitConv.ToInt32(result,8 + i*12 + 4,charmap[i].Sprite.Texture);
                BitConv.ToInt16(result,8 + i*12 + 8,charmap[i].Width);
                BitConv.ToInt16(result,8 + i*12 + 10,charmap[i].Height);
            }
            BitConv.ToInt32(result,8 + charmap.Count*12 + 0,Back.CLUT);
            BitConv.ToInt32(result,8 + charmap.Count*12 + 4,Back.Texture);
            return result;
        }
    }
}
