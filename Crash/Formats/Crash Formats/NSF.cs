using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NSF
    {
        private static byte[] ReadChunk(byte[] data,ref int offset,out bool compressed)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (offset < 0 || offset > data.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (data.Length < offset + 2)
            {
                ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
            }
            byte[] result = new byte [Chunk.Length];
            short magic = BitConv.FromInt16(data,offset);
            if (magic == Chunk.Magic)
            {
                compressed = false;
                if (data.Length < offset + Chunk.Length)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                Array.Copy(data,offset,result,0,Chunk.Length);
                offset += Chunk.Length;
            }
            else if (magic == Chunk.CompressedMagic)
            {
                compressed = true;
                int pos = 0;
                if (data.Length < offset + 12)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                short zero = BitConv.FromInt16(data,offset + 2);
                int length = BitConv.FromInt32(data,offset + 4);
                int skip = BitConv.FromInt32(data,offset + 8);
                if (zero != 0)
                {
                    ErrorManager.SignalIgnorableError("NSF.ReadChunk: Zero value is wrong");
                }
                if (length < 0 || length > Chunk.Length)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Length field is invalid");
                }
                if (skip < 0)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Skip value is negative");
                }
                offset += 12;
                while (pos < length)
                {
                    if (data.Length < offset + 1)
                    {
                        ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                    }
                    byte prefix = data[offset];
                    offset++;
                    if ((prefix & 0x80) != 0)
                    {
                        prefix &= 0x7F;
                        if (data.Length < offset + 1)
                        {
                            ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
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
                            ErrorManager.SignalError("NSF.ReadChunk: Repeat begins out of bounds");
                        }
                        if (pos + span > Chunk.Length)
                        {
                            ErrorManager.SignalError("NSF.ReadChunk: Repeat ends out of bounds");
                        }
                        // Do NOT use Array.Copy as
                        // overlap is possible i.e. span
                        // may be greater than seek
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
                            ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                        }
                        Array.Copy(data,offset,result,pos,prefix);
                        offset += prefix;
                        pos += prefix;
                    }
                }
                if (data.Length < offset + skip)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                offset += skip;
                if (data.Length < offset + (Chunk.Length - length))
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                Array.Copy(data,offset,result,pos,Chunk.Length - length);
                offset += (Chunk.Length - length);
            }
            else if (magic == 0) // Fixes some sort of read error
            {
                compressed = false;
                if (data.Length < offset + Chunk.Length)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                Array.Copy(data, offset, result, 0, Chunk.Length);
                offset += Chunk.Length;
            }
            else
            {
                compressed = false; // Fixes a stupid compile error
                ErrorManager.SignalError("NSF.ReadChunk: Unknown magic number");
            }
            return result;
        }

        public static NSF Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            int offset = 0;
            int? firstid = null;
            List<UnprocessedChunk> prelude = null;
            List<Chunk> chunks = new List<Chunk>();
            List<bool> preludecompression = null;
            List<bool> chunkcompression = new List<bool>();
            while (offset < data.Length)
            {
                bool compressed;
                byte[] chunkdata = ReadChunk(data,ref offset,out compressed);
                UnprocessedChunk chunk = Chunk.Load(chunkdata);
                if (firstid == null)
                {
                    firstid = chunk.ID;
                }
                else if (firstid == chunk.ID)
                {
                    if (prelude != null)
                    {
                        ErrorManager.SignalError("NSF: Double prelude");
                    }
                    prelude = new List<UnprocessedChunk>();
                    foreach (UnprocessedChunk unprocessedchunk in chunks)
                    {
                        prelude.Add(unprocessedchunk);
                    }
                    chunks.Clear();
                    preludecompression = chunkcompression;
                    chunkcompression = new List<bool>();
                }
                chunkcompression.Add(compressed);
                if (prelude != null && chunks.Count < prelude.Count)
                {
                    for (int i = 0;i < Chunk.Length;i++)
                    {
                        if (prelude[chunks.Count].Data[i] != chunk.Data[i])
                        {
                            ErrorManager.SignalIgnorableError("NSF: Prelude data mismatch");
                            break;
                        }
                    }
                }
                chunks.Add(chunk);
            }
            if (prelude != null && chunks.Count < prelude.Count)
            {
                ErrorManager.SignalIgnorableError("NSF: Prelude is longer than actual data");
            }
            if (prelude != null)
            {
                ErrorManager.SignalIgnorableError("NSF: Prelude saving is not yet implemented");
            }
            foreach (bool compressed in chunkcompression)
            {
                if (compressed)
                {
                    ErrorManager.SignalIgnorableError("NSF: Non-prelude chunk was compressed");
                }
            }
            return new NSF(chunks);
        }

        public static NSF LoadAndProcess(byte[] data,GameVersion gameversion)
        {
            NSF nsf = Load(data);
            nsf.ProcessAll(gameversion);
            return nsf;
        }

        private EvList<Chunk> chunks;

        public NSF(IEnumerable<Chunk> chunks)
        {
            if (chunks == null)
                throw new ArgumentNullException("chunks");
            this.chunks = new EvList<Chunk>(chunks);
        }

        public EvList<Chunk> Chunks
        {
            get { return chunks; }
        }

        public void ProcessAll(GameVersion gameversion)
        {
            for (int i = 0;i < chunks.Count;i++)
            {
                if (chunks[i] is UnprocessedChunk)
                {
                    ErrorManager.EnterSkipRegion();
                    try
                    {
                        chunks[i] = ((UnprocessedChunk)chunks[i]).Process(i * 2 + 1);
                    }
                    catch (LoadSkippedException)
                    {
                    }
                    finally
                    {
                        ErrorManager.ExitSkipRegion();
                    }
                }
                if (chunks[i] is EntryChunk)
                {
                    ((EntryChunk)chunks[i]).ProcessAll(gameversion);
                }
            }
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
