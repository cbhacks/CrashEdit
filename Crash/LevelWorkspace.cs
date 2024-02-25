
using System;
using System.Collections.Generic;

namespace CrashEdit.Crash {

    public class LevelWorkspace : Workspace {

        public GameVersion GameVersion { get; set; }

        [SubresourceSlot]
        public NSF? NSF { get; set; }

        public Dictionary<int, IEntry> AllEntriesByEid { get; } =
            new Dictionary<int, IEntry>();

        public override void Sync() {
            base.Sync();

            AllEntriesByEid.Clear();
            if (NSF != null) {
                foreach (var chunk in NSF.Chunks) {
                    if (chunk is IEntry ientry) {
                        AllEntriesByEid.Add(ientry.EID, ientry);
                    }

                    if (chunk is EntryChunk ec) {
                        foreach (var entry in ec.Entries) {
                            AllEntriesByEid.Add(entry.EID, entry);
                        }
                    }
                }
            }
        }

    }

}
