using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class EntryChunkLoader : ChunkLoader
    {
        public sealed override Chunk Load(int chunkid,byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length != Chunk.Length)
                throw new ArgumentException("Data must be 65536 bytes long.");
            int id = BitConv.FromInt32(data,4);
            int entrycount = BitConv.FromInt32(data,8);
            // Checksum is here, ignore it
            int headersize = 20 + entrycount * 4;
            if (id != chunkid)
            {
                throw new LoadException();
            }
            if (entrycount < 0)
            {
                throw new LoadException();
            }
            if (headersize > data.Length)
            {
                throw new LoadException();
            }
            Entry[] entries = new Entry [entrycount];
            byte[] entrydata;
            for (int i = 0;i < entrycount;i++)
            {
                int entrystart = BitConv.FromInt32(data,16 + i * 4);
                int entryend = BitConv.FromInt32(data,20 + i * 4);
                if (entrystart < 0)
                {
                    throw new LoadException();
                }
                if (entryend < entrystart)
                {
                    throw new LoadException();
                }
                if (entryend > data.Length)
                {
                    throw new LoadException();
                }
                int entrysize = entryend - entrystart;
                entrydata = new byte [entrysize];
                Array.Copy(data,entrystart,entrydata,0,entrysize);
                entries[i] = Entry.Load(entrydata);
                try
                {
                    entries[i] = ((UnprocessedEntry)entries[i]).Process();
                }
                catch (LoadException)
                {
                }
            }
            return Load(entries);
        }

        public abstract Chunk Load(Entry[] entries);
    }
}
