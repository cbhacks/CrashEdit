using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class NSF
    {
        private static byte[] ReadChunk(byte[] data, ref int offset, out bool compressed)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0 || offset > data.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (data.Length < offset + 2)
            {
                ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
            }
            byte[] result = new byte[Chunk.Length];
            short magic = BitConv.FromInt16(data, offset);
            if (magic == Chunk.Magic)
            {
                if (data.Length < offset + Chunk.Length)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                compressed = false;
                Array.Copy(data, offset, result, 0, Chunk.Length);
                offset += Chunk.Length;
            }
            else if (magic == Chunk.CompressedMagic)
            {
                if (data.Length < offset + 12)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                compressed = true;
                short zero = BitConv.FromInt16(data, offset + 2);
                int length = BitConv.FromInt32(data, offset + 4);
                int skip = BitConv.FromInt32(data, offset + 8);
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
                int pos = 0;
                while (pos < length)
                {
                    if (data.Length < offset + 1)
                    {
                        ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                    }
                    byte prefix = data[offset];
                    ++offset;
                    if ((prefix & 0x80) != 0)
                    {
                        prefix &= 0x7F;
                        if (data.Length < offset + 1)
                        {
                            ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                        }
                        int seek = data[offset];
                        ++offset;
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
                        for (int i = 0; i < span; ++i)
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
                        Array.Copy(data, offset, result, pos, prefix);
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
                Array.Copy(data, offset, result, pos, Chunk.Length - length);
                offset += (Chunk.Length - length);
            }
            else if (magic == 0) // Fixes some sort of read error
            {
                if (data.Length < offset + Chunk.Length)
                {
                    ErrorManager.SignalError("NSF.ReadChunk: Data is too short");
                }
                compressed = false;
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
                throw new ArgumentNullException(nameof(data));
            var nsf = new NSF();
            int offset = 0;
            int? firstid = null;
            List<UnprocessedChunk> prelude = null;
            List<bool> preludecompression = null;
            List<bool> chunkcompression = new List<bool>();
            while (offset < data.Length)
            {
                byte[] chunkdata = ReadChunk(data, ref offset, out bool compressed);
                UnprocessedChunk chunk = Chunk.Load(chunkdata, nsf);
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
                    foreach (UnprocessedChunk unprocessedchunk in nsf.Chunks)
                    {
                        prelude.Add(unprocessedchunk);
                    }
                    nsf.Chunks.Clear();
                    preludecompression = chunkcompression;
                    chunkcompression = new List<bool>();
                }
                chunkcompression.Add(compressed);
                if (prelude != null && nsf.Chunks.Count < prelude.Count)
                {
                    for (int i = 0; i < Chunk.Length; i++)
                    {
                        if (prelude[nsf.Chunks.Count].Data[i] != chunk.Data[i])
                        {
                            ErrorManager.SignalIgnorableError("NSF: Prelude data mismatch");
                            break;
                        }
                    }
                }
                nsf.Chunks.Add(chunk);
            }
            if (prelude != null)
            {
                if (nsf.Chunks.Count < prelude.Count) // error merging for now
                {
                    ErrorManager.SignalIgnorableError("NSF: Prelude is longer than actual data");
                }
                ErrorManager.SignalIgnorableError("NSF: Prelude saving is not yet implemented");
            }
            foreach (bool compressed in chunkcompression)
            {
                if (compressed)
                {
                    ErrorManager.SignalIgnorableError("NSF: Non-prelude chunk was compressed");
                }
            }
            return nsf;
        }

        public static NSF LoadAndProcess(byte[] data, GameVersion gameversion)
        {
            NSF nsf = Load(data);
            nsf.ProcessAll(gameversion);
            return nsf;
        }

        public NSF()
        {
            Chunks = new EvList<Chunk>();
            EntryMap = new Dictionary<int, IEntry>();
        }

        public EvList<Chunk> Chunks { get; }
        public Dictionary<int, IEntry> EntryMap { get; set; }

        public void ProcessAll(GameVersion gameversion)
        {
            for (int i = 0; i < Chunks.Count; i++)
            {
                if (Chunks[i] is UnprocessedChunk uchunk)
                {
                    ErrorManager.EnterSkipRegion();
                    try
                    {
                        Chunks[i] = uchunk.Process(i * 2 + 1);
                    }
                    catch (LoadSkippedException)
                    {
                    }
                    finally
                    {
                        ErrorManager.ExitSkipRegion();
                    }
                }
                if (Chunks[i] is EntryChunk echunk)
                {
                    echunk.ProcessAll(gameversion);
                }
            }
        }

        public T GetEntry<T>(string ename) where T : class, IEntry
        {
            return GetEntry<T>(Entry.ENameToEID(ename));
        }

        const bool USE_OLD_LOOKUP = true;
        public T GetEntry<T>(int eid) where T : class, IEntry
        {
            if (eid == Entry.NullEID)
                return null;
            if (EntryMap.ContainsKey(eid))
            {
                return EntryMap[eid] as T;
            }
            if (USE_OLD_LOOKUP)
            {
                foreach (Chunk chunk in Chunks)
                {
                    if (chunk is IEntry ientrychunk)
                    {
                        if (ientrychunk.EID == eid && ientrychunk is T)
                        {
                            return (T)ientrychunk;
                        }
                    }
                    if (chunk is EntryChunk entrychunk)
                    {
                        T entry = entrychunk.FindEID<T>(eid);
                        if (entry != null)
                        {
                            return entry;
                        }
                    }
                }
            }
            return null;
        }

        public List<T> GetEntries<T>() where T : IEntry
        {
            List<T> entries = new List<T>();
            foreach (var val in EntryMap.Values)
            {
                if (val is T want)
                {
                    entries.Add(want);
                }
            }
            if (USE_OLD_LOOKUP)
            {
                foreach (Chunk chunk in Chunks)
                {
                    if (chunk is IEntry centry)
                    {
                        if (centry is T c && !entries.Contains(c))
                        {
                            entries.Add(c);
                        }
                    }
                    else if (chunk is EntryChunk entrychunk)
                    {
                        foreach (Entry entry in entrychunk.Entries)
                        {
                            if (entry is T e && !entries.Contains(e))
                            {
                                entries.Add(e);
                            }
                        }
                    }
                }
            }
            return entries;
        }

        public Tuple<OldEntity, OldZoneEntry> GetEntityC1(int id)
        {
            foreach (var zone in GetEntries<OldZoneEntry>())
            {
                for (int i = 0; i < zone.Entities.Count; ++i)
                {
                    var entity = zone.Entities[i];
                    if (entity.ID == id)
                    {
                        return new Tuple<OldEntity, OldZoneEntry>(entity, zone);
                    }
                }
            }
            return null;
        }

        public Tuple<Entity, ZoneEntry> GetEntityC2(int id)
        {
            foreach (var zone in GetEntries<ZoneEntry>())
            {
                for (int i = zone.CameraCount; i < zone.Entities.Count; ++i)
                {
                    var entity = zone.Entities[i];
                    if (entity.ID == id)
                    {
                        return new Tuple<Entity, ZoneEntry>(entity, zone);
                    }
                }
            }
            return null;
        }

        public byte[] Save()
        {
            byte[] data = new byte[Chunks.Count * Chunk.Length];
            for (int i = 0; i < Chunks.Count; i++)
            {
                Chunks[i].Save(i * 2 + 1).CopyTo(data, i * Chunk.Length);
            }
            return data;
        }

        public Tuple<int[], IList<NSDLink>> MakeNSDIndex()
        {
            foreach (Chunk chunk in Chunks)
            {
                if (chunk is EntryChunk entrychunk)
                {
                    List<Entry> entries = new List<Entry>(entrychunk.Entries);
                    entries.Sort(delegate (Entry a, Entry b)
                    {
                        int c = a.HashKey - b.HashKey;
                        if (c == 0)
                        {
                            c = new ENameComparer().Compare(a.EName, b.EName);
                        }
                        return c;
                    });
                    entrychunk.Entries = new EvList<Entry>(entries);
                }
            }
            SortedDictionary<int, SortedDictionary<string, int>> newindex = new SortedDictionary<int, SortedDictionary<string, int>>();
            for (int i = 0; i < Chunks.Count; i++)
            {
                if (Chunks[i] is IEntry ientry)
                {
                    if (!newindex.ContainsKey(ientry.HashKey))
                        newindex.Add(ientry.HashKey, new SortedDictionary<string, int>(new ENameComparer()));
                    newindex[ientry.HashKey].Add(ientry.EName, i * 2 + 1);
                }
                else if (Chunks[i] is EntryChunk chunk)
                {
                    foreach (Entry entry in chunk.Entries)
                    {
                        if (!newindex.ContainsKey(entry.HashKey))
                            newindex.Add(entry.HashKey, new SortedDictionary<string, int>(new ENameComparer()));
                        if (newindex[entry.HashKey].ContainsKey(entry.EName))
                        {
                            ErrorManager.SignalIgnorableError(string.Format("NSF.MakeNSDIndex: Duplicate entry {0}", entry.EName));
                        }
                        else
                        {
                            newindex[entry.HashKey].Add(entry.EName, i * 2 + 1);
                        }
                    }
                }
            }
            var index = new List<NSDLink>();
            int[] hashkeymap = new int[256];
            int curkey = 0;
            foreach (var kvp in newindex)
            {
                while (curkey <= kvp.Key)
                    hashkeymap[curkey++] = index.Count;
                foreach (var link in kvp.Value)
                    index.Add(new NSDLink(link.Value, Entry.ENameToEID(link.Key)));
            }
            while (curkey < 256)
                hashkeymap[curkey++] = index.Count - 1;
            return new Tuple<int[], IList<NSDLink>>(hashkeymap, index);
        }
    }
}
