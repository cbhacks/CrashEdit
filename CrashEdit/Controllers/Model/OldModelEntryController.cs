using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldModelEntry))]
    public sealed class OldModelEntryController : EntryController
    {
        public OldModelEntryController(OldModelEntry oldmodelentry, SubcontrollerGroup parentGroup) : base(oldmodelentry, parentGroup)
        {
            OldModelEntry = oldmodelentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new Label { Text = string.Format("Polygon count: {0}", BitConv.FromInt32(OldModelEntry.Info, 0)), TextAlign = ContentAlignment.MiddleCenter };
        }

        public OldModelEntry OldModelEntry { get; }
    }
}
