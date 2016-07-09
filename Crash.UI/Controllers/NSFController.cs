using System;

namespace Crash.UI
{
    public sealed class NSFController : Controller
    {
        private NSF nsf;

        public NSFController(NSF nsf)
        {
            this.nsf = nsf;
            this.nsf.Chunks.ItemAdded += new EvListEventHandler<Chunk>(Chunks_ItemAdded);
            this.nsf.Chunks.ItemRemoved += new EvListEventHandler<Chunk>(Chunks_ItemRemoved);
            this.nsf.Chunks.Populate(Chunks_ItemAdded);
        }

        public NSF NSF
        {
            get { return nsf; }
        }

        public override string ToString()
        {
            return Properties.Resources.NSFController_Text;
        }

        private void Chunks_ItemAdded(object sender,EvListEventArgs<Chunk> e)
        {
            if (e.Item is NormalChunk)
            {
                Subcontrollers.Insert(e.Index,new NormalChunkController(this,(NormalChunk)e.Item));
            }
            else if (e.Item is TextureChunk)
            {
                Subcontrollers.Insert(e.Index,new TextureChunkController(this,(TextureChunk)e.Item));
            }
            else if (e.Item is OldSoundChunk)
            {
                Subcontrollers.Insert(e.Index,new OldSoundChunkController(this,(OldSoundChunk)e.Item));
            }
            else if (e.Item is SoundChunk)
            {
                Subcontrollers.Insert(e.Index,new SoundChunkController(this,(SoundChunk)e.Item));
            }
            else if (e.Item is WavebankChunk)
            {
                Subcontrollers.Insert(e.Index,new WavebankChunkController(this,(WavebankChunk)e.Item));
            }
            else if (e.Item is SpeechChunk)
            {
                Subcontrollers.Insert(e.Index,new SpeechChunkController(this,(SpeechChunk)e.Item));
            }
            else if (e.Item is UnprocessedChunk)
            {
                Subcontrollers.Insert(e.Index,new UnprocessedChunkController(this,(UnprocessedChunk)e.Item));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Chunks_ItemRemoved(object sender,EvListEventArgs<Chunk> e)
        {
            Subcontrollers.RemoveAt(e.Index);
        }

        public override void Dispose()
        {
            nsf.Chunks.ItemAdded -= Chunks_ItemAdded;
            nsf.Chunks.ItemRemoved -= Chunks_ItemRemoved;
            base.Dispose();
        }

        private sealed class AcAddNormalChunk : Action<NSFController>
        {
            protected override string GetText(NSFController c)
            {
                return Properties.Resources.NSFController_AcAddNormalChunk;
            }

            protected override Command Activate(NSFController c)
            {
                return c.NSF.Chunks.CmAdd(new NormalChunk());
            }
        }
    }
}
