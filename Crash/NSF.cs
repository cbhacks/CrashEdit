using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NSF
    {
        private static byte[] ReadChunk(byte[] data,ref int offset)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (offset < 0 || offset > data.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (data.Length < 2)
            {
                throw new LoadException();
            }
            byte[] result = new byte [Chunk.Length];
            short magic = BitConv.FromInt16(data,offset);
            if (magic == Chunk.Magic)
            {
                if (data.Length < offset + Chunk.Length)
                {
                    throw new LoadException();
                }
                Array.Copy(data,offset,result,0,Chunk.Length);
                offset += Chunk.Length;
            }
            else if (magic == Chunk.CompressedMagic)
            {
                int pos = 0;
                if (data.Length < 12)
                {
                    throw new LoadException();
                }
                short zero = BitConv.FromInt16(data,offset + 2);
                int length = BitConv.FromInt32(data,offset + 4);
                int skip = BitConv.FromInt32(data,offset + 8);
                if (zero != 0)
                {
                    throw new LoadException();
                }
                if (length < 0 || length > Chunk.Length)
                {
                    throw new LoadException();
                }
                if (skip < 0)
                {
                    throw new LoadException();
                }
                offset += 12;
                while (pos < length)
                {
                    if (data.Length < offset + 1)
                    {
                        throw new LoadException();
                    }
                    byte prefix = data[offset];
                    offset++;
                    if ((prefix & 0x80) != 0)
                    {
                        prefix &= 0x7F;
                        if (data.Length < offset + 1)
                        {
                            throw new LoadException();
                        }
                        int seek = data[offset];
                        offset++;
                        int span = seek & 7;
                        seek >>= 3;
                        seek |= prefix << 5;
                        if (span == 7)
                        {
                            span = 64;
                        }
                        else
                        {
                            span += 3;
                        }
                        if (pos - seek < 0)
                        {
                            throw new LoadException();
                        }
                        if (pos + span > Chunk.Length)
                        {
                            throw new LoadException();
                        }
                        // Do NOT use Array.Copy
                        // due to possible overlap
                        // this requires memmove semantics
                        for (int i = 0;i < span;i++)
                        {
                            result[pos + i] = result[pos - seek + i];
                        }
                        pos += span;
                    }
                    else
                    {
                        if (data.Length < offset + prefix)
                        {
                            throw new LoadException();
                        }
                        Array.Copy(data,offset,result,pos,prefix);
                        offset += prefix;
                        pos += prefix;
                    }
                }
                if (data.Length < offset + skip)
                {
                    throw new LoadException();
                }
                offset += skip;
                if (data.Length < offset + (Chunk.Length - length))
                {
                    throw new LoadException();
                }
                Array.Copy(data,offset,result,pos,Chunk.Length - length);
                offset += (Chunk.Length - length);
            }
            else
            {
                throw new LoadException();
            }
            return result;
        }

        public static NSF Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            int offset = 0;
            List<Chunk> chunks = new List<Chunk>();
            while (offset < data.Length)
            {
                byte[] chunkdata = ReadChunk(data,ref offset);
                Chunk chunk = Chunk.Load(chunkdata);
                try
                {
                    chunk = ((UnprocessedChunk)chunk).Process(chunks.Count * 2 + 1);
                }
                catch (LoadException)
                {
                }
                chunks.Add(chunk);
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

        public T FindEID<T>(int eid) where T : class,IEntry
        {
            foreach (Chunk chunk in chunks)
            {
                if (chunk is IEntry)
                {
                    IEntry entry = (IEntry)chunk;
                    if (entry.EID == eid && entry is T)
                    {
                        return (T)entry;
                    }
                }
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
