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
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.UnprocessedChunkController_Text,UnprocessedChunk.Type,GetNSF().Chunks.IndexOf(UnprocessedChunk) * 2 + 1);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "JournalOrange";
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
                processedchunk = UnprocessedChunk.Process(GetNSF().Chunks.IndexOf(UnprocessedChunk) * 2 + 1);
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
