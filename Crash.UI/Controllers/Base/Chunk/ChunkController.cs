namespace Crash.UI
{
    public abstract class ChunkController : Controller
    {
        public ChunkController(NSFController up,Chunk chunk)
        {
            Up = up;
            Chunk = chunk;
        }

        public NSFController Up { get; }

        public Chunk Chunk { get; }

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

            protected override Command Activate(ChunkController c) => c.Up.NSF.Chunks.CmRemove(c.Chunk);
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
                int index = c.Up.NSF.Chunks.IndexOf(c.Chunk);
                UnprocessedChunk unprocessedchunk = c.Chunk.Unprocess(index * 2 + 1);
                return c.Up.NSF.Chunks.CmSet(index,unprocessedchunk);
            }
        }
    }
}
