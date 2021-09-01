using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class SoundEntryController : EntryController
    {
        public SoundEntryController(EntryChunkController entrychunkcontroller,SoundEntry soundentry) : base(entrychunkcontroller,soundentry)
        {
            SoundEntry = soundentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.SoundEntryController_Text,SoundEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "SpeakerBlue";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SoundBox(SoundEntry);
        }

        public SoundEntry SoundEntry { get; }
    }
}
