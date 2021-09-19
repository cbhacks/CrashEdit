using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldCameraController : LegacyController
    {
        public OldCameraController(ProtoZoneEntryController protozoneentrycontroller,OldCamera camera) : base(protozoneentrycontroller, camera)
        {
            ProtoZoneEntryController = protozoneentrycontroller;
            Camera = camera;
            AddMenu("Delete Camera",Menu_DeleteProto);
            InvalidateNode();
        }

        public OldCameraController(OldZoneEntryController oldzoneentrycontroller,OldCamera camera) : base(oldzoneentrycontroller, camera)
        {
            OldZoneEntryController = oldzoneentrycontroller;
            Camera = camera;
            AddMenu("Delete Camera",Menu_DeleteOld);
            InvalidateNode();
        }

        public void InvalidateNode()
        {
            NodeText = CrashUI.Properties.Resources.OldCameraController_Text;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldCameraBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController { get; }
        public ProtoZoneEntry ProtoZoneEntry => ProtoZoneEntryController.ProtoZoneEntry;
        public OldZoneEntryController OldZoneEntryController { get; }
        public OldZoneEntry OldZoneEntry => OldZoneEntryController.OldZoneEntry;
        public OldCamera Camera { get; }
        
        private void Menu_DeleteProto()
        {
            ProtoZoneEntry.Cameras.Remove(Camera);
            ProtoZoneEntry.CameraCount = ProtoZoneEntry.Cameras.Count;
            RemoveSelf();
        }
        
        private void Menu_DeleteOld()
        {
            OldZoneEntry.Cameras.Remove(Camera);
            RemoveSelf();
        }
    }
}
