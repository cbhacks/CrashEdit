namespace CrashEdit.Crash
{

    public class LevelWorkspace : Workspace
    {

        public GameVersion GameVersion { get; set; }

        [SubresourceSlot]
        public NSF? NSF { get; set; }

        public Dictionary<int, IEntry> AllEntriesByEid { get; } = [];
        
        public T? GetEntry<T>(int eid) where T : class
        {
            if (AllEntriesByEid.TryGetValue(eid, out var entry))
                return entry as T; // this will be null if it's not in the type we want
            else
                return null;
        }

        public IEnumerable<T> GetEntries<T>() where T : class
        {
            var list = new List<T>();
            foreach (var entry in AllEntriesByEid.Values)
            {
                if (entry is T want)
                    list.Add(want);
            }
            return list;
        }

        public IEnumerable<int> GetEntriesEID<T>() where T : class
        {
            var list = new List<int>();
            foreach (var kvp in AllEntriesByEid)
            {
                if (kvp.Value is T)
                    list.Add(kvp.Key);
            }
            return list;
        }

        public override void Sync()
        {
            base.Sync();

            AllEntriesByEid.Clear();
            if (NSF != null)
            {
                int chunkid = -1;
                foreach (var chunk in NSF.Chunks)
                {
                    chunkid += 2;
                    chunk.ChunkId = chunkid;
                    if (chunk is IEntry ientry)
                    {
                        AllEntriesByEid.Add(ientry.EID, ientry);
                    }

                    if (chunk is EntryChunk ec)
                    {
                        foreach (var entry in ec.Entries)
                        {
                            AllEntriesByEid.Add(entry.EID, entry);
                        }
                    }
                }
            }
        }

    }

}
