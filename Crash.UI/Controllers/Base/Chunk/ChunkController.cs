using System;
using System.Drawing;
using System.Windows.Forms;

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
    }
}
