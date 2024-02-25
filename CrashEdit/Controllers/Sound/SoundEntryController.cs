using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(SoundEntry))]
    public sealed class SoundEntryController : EntryController
    {
        public SoundEntryController(SoundEntry soundentry, SubcontrollerGroup parentGroup) : base(soundentry, parentGroup)
        {
            SoundEntry = soundentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SoundBox(SoundEntry);
        }

        public SoundEntry SoundEntry { get; }
    }
}
