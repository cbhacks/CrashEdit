using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;

namespace CrashEdit
{
    public sealed class NSFController : Controller
    {
        private NSF nsf;

        public NSFController(NSF nsf)
        {
            this.nsf = nsf;
            Node.Text = "NSF File";
            Node.ImageKey = "nsf";
            Node.SelectedImageKey = "nsf";
            foreach (Chunk chunk in nsf.Chunks)
            {
                if (chunk is NormalChunk)
                {
                    AddNode(new NormalChunkController(this,(NormalChunk)chunk));
                }
                else if (chunk is TextureChunk)
                {
                    AddNode(new LegacyController(chunk));
                }
                else if (chunk is SoundChunk)
                {
                    AddNode(new SoundChunkController(this,(SoundChunk)chunk));
                }
                else if (chunk is WavebankChunk)
                {
                    AddNode(new LegacyController(chunk));
                }
                else if (chunk is SpeechChunk)
                {
                    AddNode(new SpeechChunkController(this,(SpeechChunk)chunk));
                }
                else
                {
                    AddNode(new ErrorController());
                }
            }
        }
    }
}
