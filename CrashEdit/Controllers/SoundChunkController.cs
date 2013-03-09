using Crash;
using Crash.Audio;

namespace CrashEdit
{
    public sealed class SoundChunkController : EntryChunkController
    {
        private SoundChunk soundchunk;

        public SoundChunkController(NSFController nsfcontroller,SoundChunk soundchunk) : base(nsfcontroller,soundchunk)
        {
            this.soundchunk = soundchunk;
            Node.Text = "Sound Chunk";
            Node.ImageKey = "soundchunk";
            Node.SelectedImageKey = "soundchunk";
        }

        public SoundChunk SoundChunk
        {
            get { return soundchunk; }
        }
    }
}
