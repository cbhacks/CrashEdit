namespace CrashEdit.Crash
{
    public abstract class EntryChunk : Chunk
    {
        public EntryChunk()
        {
            Entries = new List<Entry>();
        }

        public EntryChunk(IEnumerable<Entry> entries)
        {
            ArgumentNullException.ThrowIfNull(entries);
            Entries = new List<Entry>(entries);
        }

        public int ChunkId { get; set; }

        [SubresourceList]
        public List<Entry> Entries { get; set; }

        public abstract int Alignment { get; }

        public void ProcessAll(GameVersion gameversion)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i] is UnprocessedEntry)
                {
                    ErrorManager.EnterSkipRegion();
                    ErrorManager.EnterSubject(Entries[i]);
                    try
                    {
                        Entries[i] = ((UnprocessedEntry)Entries[i]).Process(gameversion);
                    }
                    catch (LoadSkippedException)
                    {
                    }
                    finally
                    {
                        ErrorManager.ExitSkipRegion();
                        ErrorManager.ExitSubject();
                    }
                }
            }
        }

        public T FindEID<T>(int eid) where T : class, IEntry
        {
            foreach (Entry entry in Entries)
            {
                if (entry.EID == eid && entry is T)
                {
                    return (T)(object)entry;
                }
            }
            return null;
        }

        public override UnprocessedChunk Unprocess()
        {
            byte[] data = new byte[Length];
            BitConv.ToInt16(data, 0, Magic);
            BitConv.ToInt16(data, 2, Type);
            BitConv.ToInt32(data, 4, ChunkId);
            BitConv.ToInt32(data, 8, Entries.Count);
            // Checksum is here, but calculated later
            int offset = 20 + Entries.Count * 4;
            for (int i = 0; i < Entries.Count; i++)
            {
                UnprocessedEntry entry = Entries[i].Unprocess();
                byte[] entrydata = entry.Save();
                offset += entry.HeaderLength;
                Aligner.Align(ref offset, Alignment);
                offset -= entry.HeaderLength;
                if (offset + entrydata.Length > Length)
                {
                    throw new PackingException(entry.EID);
                }
                BitConv.ToInt32(data, 16 + i * 4, offset);
                entrydata.CopyTo(data, offset);
                offset += entrydata.Length;
            }
            BitConv.ToInt32(data, 16 + Entries.Count * 4, offset);
            int checksum = CalculateChecksum(data);
            BitConv.ToInt32(data, 12, checksum);
            return new UnprocessedChunk(data);
        }
    }
}
