using System.Collections.Generic;

namespace Crash
{
    public abstract class EntryChunk : Chunk
    {
        protected byte[] Save(IList<Entry> entries,int unknown1,int unknown2)
        {
            return Save(entries,unknown1,unknown2,4);
        }

        protected byte[] Save(IList<Entry> entries,int unknown1,int unknown2,int align)
        {
            byte[] data = new byte [Length];
            BitConv.ToHalf(data,0,Magic);
            BitConv.ToHalf(data,2,Type);
            BitConv.ToWord(data,4,unknown1);
            BitConv.ToWord(data,8,entries.Count);
            BitConv.ToWord(data,12,unknown2);
            int offset = 20 + entries.Count * 4;
            offset += offset % align;
            BitConv.ToWord(data,16,offset);
            for (int i = 0;i < entries.Count;i++)
            {
                byte[] entrydata = entries[i].Save();
                entrydata.CopyTo(data,offset);
                offset += entrydata.Length;
                offset += offset % align;
                BitConv.ToWord(data,20 + i * 4,offset);
            }
            return data;
        }
    }
}
