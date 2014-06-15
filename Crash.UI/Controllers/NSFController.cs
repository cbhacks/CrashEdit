using System;
using System.Drawing;
using System.Windows.Forms;

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
            for (int i = 0;i < nsf.Chunks.Count;i++)
            {
                EvListEventArgs<Chunk> e = new EvListEventArgs<Chunk>();
                e.Index = i;
                e.Item = nsf.Chunks[i];
                Chunks_ItemAdded(null,e);
            }
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
            Subcontrollers[e.Index].Dispose();
            Subcontrollers.RemoveAt(e.Index);
        }

        public override void Dispose()
        {
            nsf.Chunks.ItemAdded -= Chunks_ItemAdded;
            nsf.Chunks.ItemRemoved -= Chunks_ItemRemoved;
            base.Dispose();
        }
    }
}
