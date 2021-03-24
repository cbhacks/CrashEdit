#nullable enable

using System;

namespace CrashEdit.Crash {

    public class LevelWorkspace : Workspace {

        public GameVersion GameVersion { get; set; }

        [SubresourceSlot]
        public NSF? NSF { get; set; }

    }

}
