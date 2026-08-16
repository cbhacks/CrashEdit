namespace CrashEdit.Crash
{

    public class LevelWorkspace : Workspace
    {

        public GameVersion GameVersion { get; set; }

        [SubresourceSlot]
        public NSF? NSF { get; set; }

        public override void Sync()
        {
            base.Sync();

            if (NSF != null)
            {
                int chunkid = -1;
                foreach (var chunk in NSF.Chunks)
                {
                    chunkid += 2;
                    chunk.ChunkId = chunkid;
                }
            }
        }

    }

}
