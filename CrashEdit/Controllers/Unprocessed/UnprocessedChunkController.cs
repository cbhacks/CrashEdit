using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class UnprocessedChunkController : ChunkController
    {
        public UnprocessedChunkController(NSFController nsfcontroller,UnprocessedChunk unprocessedchunk) : base(nsfcontroller,unprocessedchunk)
        {
            UnprocessedChunk = unprocessedchunk;
            AddMenu(CrashUI.Properties.Resources.UnprocessedChunkController_AcProcess,Menu_Process_Chunk);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new MysteryBox(UnprocessedChunk.Data);
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
            ChunkController processedchunkcontroller = NSFController.CreateChunkController(processedchunk);
            NSFController.InsertNode(index, processedchunkcontroller);
            RemoveSelf();
        }
    }
}
