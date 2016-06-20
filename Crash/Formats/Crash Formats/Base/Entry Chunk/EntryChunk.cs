using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class EntryChunk : Chunk
    {
        private EvList<Entry> entries;

        public EntryChunk()
        {
            entries = new EvList<Entry>();
        }

        public EntryChunk(IEnumerable<Entry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            this.entries = new EvList<Entry>(entries);
        }

        public EvList<Entry> Entries
        {
            get { return entries; }
        }

        protected abstract int Alignment
        {
            get;
        }

        public void ProcessAll(GameVersion gameversion)
        {
            for (int i = 0;i < entries.Count;i++)
            {
                if (entries[i] is UnprocessedEntry)
                {
                    ErrorManager.EnterSkipRegion();
                    try
                    {
                        entries[i] = ((UnprocessedEntry)entries[i]).Process(gameversion);
                    }
                    catch (LoadSkippedException)
                    {
                    }
                    finally
                    {
                        ErrorManager.ExitSkipRegion();
                    }
                }
            }
        }

        public T FindEID<T>(int eid) where T : class,IEntry
        {
            foreach (Entry entry in entries)
            {
                if (entry.EID == eid && entry is T)
                {
                    return (T)(object)entry;
                }
            }
            return null;
        }

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            byte[] data = new byte [Length];
            BitConv.ToInt16(data,0,Magic);
            BitConv.ToInt16(data,2,Type);
            BitConv.ToInt32(data,4,chunkid);
            BitConv.ToInt32(data,8,entries.Count);
            // Checksum is here, but calculated later
            int offset = 20 + entries.Count * 4;
            for (int i = 0;i < entries.Count;i++)
            {
                UnprocessedEntry entry = entries[i].Unprocess();
                byte[] entrydata = entry.Save();
                offset += entry.HeaderLength;
                Aligner.Align(ref offset,Alignment);
                offset -= entry.HeaderLength;
                if (offset + entrydata.Length > Length)
                {
                    throw new PackingException(entry.EID);
                }
                BitConv.ToInt32(data,16 + i * 4,offset);
                entrydata.CopyTo(data,offset);
                offset += entrydata.Length;
            }
            BitConv.ToInt32(data,16 + entries.Count * 4,offset);
            int checksum = CalculateChecksum(data);
            BitConv.ToInt32(data,12,checksum);
            return new UnprocessedChunk(data);
        }
    }
}
