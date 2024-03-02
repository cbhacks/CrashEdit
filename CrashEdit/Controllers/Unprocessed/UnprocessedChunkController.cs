using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(UnprocessedChunk))]
    public sealed class UnprocessedChunkController : ChunkController
    {
        public UnprocessedChunkController(UnprocessedChunk unprocessedchunk, SubcontrollerGroup parentGroup) : base(unprocessedchunk, parentGroup)
        {
            UnprocessedChunk = unprocessedchunk;
            AddMenu(CrashUI.Properties.Resources.UnprocessedChunkController_AcProcess, Menu_Process_Chunk);
        }

        public UnprocessedChunk UnprocessedChunk { get; }

        private void Menu_Process_Chunk()
        {
            Chunk processedchunk;
            try
            {
                processedchunk = UnprocessedChunk.Process();
            }
            catch (LoadAbortedException)
            {
                return;
            }
            int index = GetNSF().Chunks.IndexOf(UnprocessedChunk);
            GetNSF().Chunks[index] = processedchunk;
            if (processedchunk is EntryChunk)
            {
                ((EntryChunk)processedchunk).ProcessAll(NSFController.GameVersion);
            }
        }
    }
}
