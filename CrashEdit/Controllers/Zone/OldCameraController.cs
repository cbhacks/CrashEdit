using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldCamera))]
    public sealed class OldCameraController : LegacyController
    {
        public OldCameraController(OldCamera camera, SubcontrollerGroup parentGroup) : base(parentGroup, camera)
        {
            Camera = camera;
            AddMenu("Delete Camera", Menu_Delete);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldCameraBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController => Modern.Parent.Legacy as ProtoZoneEntryController;
        public ProtoZoneEntry ProtoZoneEntry => ProtoZoneEntryController.ProtoZoneEntry;
        public OldZoneEntryController OldZoneEntryController => Modern.Parent.Legacy as OldZoneEntryController;
        public OldZoneEntry OldZoneEntry => OldZoneEntryController.OldZoneEntry;
        public OldCamera Camera { get; }

        private void Menu_Delete()
        {
            if (ProtoZoneEntryController != null)
            {
                ProtoZoneEntry.Cameras.Remove(Camera);
                ProtoZoneEntry.CameraCount = ProtoZoneEntry.Cameras.Count;
            }
            else
            {
                OldZoneEntry.Cameras.Remove(Camera);
            }
        }
    }
}
