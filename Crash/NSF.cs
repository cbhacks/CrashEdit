using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NSF
    {
        public static NSF Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length % Chunk.Length != 0)
            {
                throw new LoadException();
            }
            int chunkcount = data.Length / Chunk.Length;
            Chunk[] chunks = new Chunk[chunkcount];
            byte[] chunkdata;
            for (int i = 0;i < chunkcount;i++)
            {
                chunkdata = new byte [Chunk.Length];
                Array.Copy(data,i * Chunk.Length,chunkdata,0,Chunk.Length);
                chunks[i] = Chunk.Load(i * 2 + 1,chunkdata);
            }
            return new NSF(chunks);
        }

        private List<Chunk> chunks;

        public NSF(IEnumerable<Chunk> chunks)
        {
            if (chunks == null)
                throw new ArgumentNullException("chunks");
            this.chunks = new List<Chunk>(chunks);
        }

        public IList<Chunk> Chunks
        {
            get { return chunks; }
        }

        public T FindEID<T>(int eid) where T : Entry
        {
            foreach (Chunk chunk in chunks)
            {
                if (chunk is EntryChunk)
                {
                    EntryChunk entrychunk = (EntryChunk)chunk;
                    T entry = entrychunk.FindEID<T>(eid);
                    if (entry != null)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }

        public byte[] Save()
        {
            byte[] data = new byte [chunks.Count * Chunk.Length];
            for (int i = 0;i < chunks.Count;i++)
            {
                chunks[i].Save(i * 2 + 1).CopyTo(data,i * Chunk.Length);
            }
            return data;
        }
    }
}
