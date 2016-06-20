using System;

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
            int checksum = BitConv.FromInt32(data,12);
            int headersize = 20 + entrycount * 4;
            if (id != chunkid)
            {
                ErrorManager.SignalIgnorableError("EntryChunk: Chunk id is incorrect");
            }
            if (entrycount < 0)
            {
                ErrorManager.SignalError("EntryChunk: Entry count is negative");
            }
            if (checksum != Chunk.CalculateChecksum(data))
            {
                ErrorManager.SignalIgnorableError("Chunk: Checksum is wrong");
            }
            if (headersize > data.Length)
            {
                ErrorManager.SignalError("EntryChunk: Data is too short");
            }
            Entry[] entries = new Entry [entrycount];
            byte[] entrydata;
            for (int i = 0;i < entrycount;i++)
            {
                int entrystart = BitConv.FromInt32(data,16 + i * 4);
                int entryend = BitConv.FromInt32(data,20 + i * 4);
                if (entrystart < 0)
                {
                    ErrorManager.SignalError("EntryChunk: Entry begins out of bounds");
                }
                if (entryend < entrystart)
                {
                    ErrorManager.SignalError("EntryChunk: Entry ends before it begins");
                }
                if (entryend > data.Length)
                {
                    ErrorManager.SignalError("EntryChunk: Entry ends out of bounds");
                }
                int entrysize = entryend - entrystart;
                entrydata = new byte [entrysize];
                Array.Copy(data,entrystart,entrydata,0,entrysize);
                entries[i] = Entry.Load(entrydata);
            }
            return Load(entries);
        }

        public abstract Chunk Load(Entry[] entries);
    }
}
