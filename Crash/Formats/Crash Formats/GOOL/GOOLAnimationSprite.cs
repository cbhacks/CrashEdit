using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class GOOLAnimationSprite : GOOLAnimation
    {
        public static GOOLAnimationSprite Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            byte type = data[0];
            byte u1 = data[1];
            byte count = data[2];
            byte u2 = data[3];

            int eid = BitConv.FromInt32(data,4);

            GOOLSpriteFrame[] frames = new GOOLSpriteFrame[count];
            for (int i = 0; i < count; ++i)
            {
                frames[i] = new GOOLSpriteFrame(BitConv.FromInt32(data,8+i*8+0), BitConv.FromInt32(data,8+i*8+4));
            }

            return new GOOLAnimationSprite(eid,frames,type,u1,count,u2);
        }

        private List<GOOLSpriteFrame> frames;

        public GOOLAnimationSprite(int eid,IEnumerable<GOOLSpriteFrame> frames,byte type,byte u1,byte count,byte u2) : base(type,u1,count,u2)
        {
            EID = eid;
            this.frames = new List<GOOLSpriteFrame>(frames);
        }

        public int EID { get; }
        public IList<GOOLSpriteFrame> Frames => frames;

        public override byte[] Save()
        {
            byte[] result = new byte[8 + frames.Count*8];
            result[0] = Type;
            result[1] = U1;
            result[2] = Count;
            result[3] = U2;
            BitConv.ToInt32(result,4,EID);
            for (int i = 0; i < frames.Count; ++i)
            {
                BitConv.ToInt32(result,8 + i*8 + 0,frames[i].CLUT);
                BitConv.ToInt32(result,8 + i*8 + 4,frames[i].Texture);
            }
            return result;
        }
    }
}
