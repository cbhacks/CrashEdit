using System;
using System.Collections.Generic;

namespace Crash
{
    public abstract class EntryChunk : Chunk
    {
        public const bool ALLOW_DUPLICATE_ENTRIES = false;

        public EntryChunk(NSF nsf) : base(nsf)
        {
            Entries = new EvList<Entry>();
            Entries.ItemAdded += Entries_ItemAdded;
            Entries.ItemRemoved += Entries_ItemRemoved;
        }

        public EntryChunk(IEnumerable<Entry> entries, NSF nsf) : base(nsf)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");
            Entries = new EvList<Entry>();
            Entries.ItemAdded += Entries_ItemAdded;
            Entries.ItemRemoved += Entries_ItemRemoved;
            foreach (var entry in entries)
            {
                entry.ChunkAddTo(this);
            }
        }

        private void Entries_ItemAdded(object sender, EvListEventArgs<Entry> e)
        {
            if (ALLOW_DUPLICATE_ENTRIES && NSF.EntryMap.ContainsKey(e.Item.EID))
            {
                NSF.EntryMap[e.Item.EID] = e.Item;
            }
            else
            {
                NSF.EntryMap.Add(e.Item.EID, e.Item);
            }
        }

        private void Entries_ItemRemoved(object sender, EvListEventArgs<Entry> e)
        {
            if (NSF.EntryMap[e.Item.EID] == e.Item)
                NSF.EntryMap.Remove(e.Item.EID);
        }

        public EvList<Entry> Entries { get; set; }

        public abstract int Alignment { get; }

        public void ProcessAll(GameVersion gameversion)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i] is UnprocessedEntry entry)
                {
                    ErrorManager.EnterSkipRegion();
                    ErrorManager.EnterSubject(entry);
                    try
                    {
                        entry.Process(gameversion).ChunkReplaceWith(Entries[i]);
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

        public override UnprocessedChunk Unprocess(int chunkid)
        {
            byte[] data = new byte[Length];
            BitConv.ToInt16(data, 0, Magic);
            BitConv.ToInt16(data, 2, Type);
            BitConv.ToInt32(data, 4, chunkid);
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
            return new UnprocessedChunk(data, NSF);
        }
    }
}
