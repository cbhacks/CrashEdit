namespace Crash.UI
{
    public abstract class ChunkController : Controller
    {
        private NSFController up;
        private Chunk chunk;

        public ChunkController(NSFController up,Chunk chunk)
        {
            this.up = up;
            this.chunk = chunk;
        }

        public NSFController Up
        {
            get { return up; }
        }

        public Chunk Chunk
        {
            get { return chunk; }
        }

        private sealed class AcDelete : Action<ChunkController>
        {
            protected override string GetText(ChunkController c)
            {
                if (c is IEntryController)
                {
                    return string.Format(Properties.Resources.ChunkController_AcDeleteWithEName,
                        Entry.EIDToEName(((IEntryController) c).Entry.EID));
                }
                else
                {
                    return Properties.Resources.ChunkController_AcDelete;
                }
            }

            protected override Command Activate(ChunkController c)
            {
                return c.up.NSF.Chunks.CmRemove(c.Chunk);
            }
        }

        private sealed class AcDeprocess : Action<ChunkController>
        {
            protected override bool CheckCompatibility(ChunkController c)
            {
                if (c is UnprocessedChunkController)
                    return false;
                return true;
            }

            protected override string GetText(ChunkController c)
            {
                if (c is IEntryController)
                {
                    return string.Format(Properties.Resources.ChunkController_AcDeprocessWithEName,
                        Entry.EIDToEName(((IEntryController) c).Entry.EID));
                }
                else
                {
                    return Properties.Resources.ChunkController_AcDeprocess;
                }
            }

            protected override Command Activate(ChunkController c)
            {
                int index = c.up.NSF.Chunks.IndexOf(c.chunk);
                UnprocessedChunk unprocessedchunk = c.chunk.Unprocess(index * 2 + 1);
                return c.up.NSF.Chunks.CmSet(index,unprocessedchunk);
            }
        }
    }
}
