using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(SpeechEntry))]
    public sealed class SpeechEntryController : EntryController
    {
        public SpeechEntryController(SpeechEntry speechentry, SubcontrollerGroup parentGroup) : base(speechentry, parentGroup)
        {
            SpeechEntry = speechentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SoundBox(SpeechEntry);
        }

        public SpeechEntry SpeechEntry { get; }
    }
}
