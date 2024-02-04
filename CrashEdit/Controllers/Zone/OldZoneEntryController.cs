using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldZoneEntryController : EntryController
    {
        public OldZoneEntryController(EntryChunkController entrychunkcontroller, OldZoneEntry zoneentry)
            : base(entrychunkcontroller, zoneentry)
        {
            OldZoneEntry = zoneentry;
            AddNode(new ItemController(null, zoneentry.Header));
            AddNode(new ItemController(null, zoneentry.Layout));
            foreach (OldCamera camera in zoneentry.Cameras)
            {
                AddNode(new OldCameraController(this, camera));
            }
            foreach (OldEntity entity in zoneentry.Entities)
            {
                AddNode(new OldEntityController(this, entity));
            }
            AddMenu("Add Camera", Menu_AddCamera);
            AddMenu("Add Entity", Menu_AddEntity);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldZoneEntryController_Text, OldZoneEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "violetb";
            Node.SelectedImageKey = "violetb";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new OldZoneEntryViewer(NSF, OldZoneEntry.EID));
        }

        public OldZoneEntry OldZoneEntry { get; }

        void Menu_AddCamera()
        {
            OldCamera newcam = OldCamera.Load(new OldCamera(Entry.ENameToEID("NONE!"), 0, 0, new OldCameraNeighbor[4], 0, 0, 0, 0, 1600, 0, 0, 0, 0, 0, 0, new List<OldCameraPosition>(), 0).Save());
            OldZoneEntry.Cameras.Add(newcam);
            InsertNode(2 + OldZoneEntry.Cameras.Count - 1, new OldCameraController(this, newcam));
        }

        void Menu_AddEntity()
        {
            short id = 6;
            while (true)
            {
                foreach (OldZoneEntry zone in EntryChunkController.NSFController.NSF.GetEntries<OldZoneEntry>())
                {
                    foreach (OldEntity otherentity in zone.Entities)
                    {
                        if (otherentity.ID == id)
                        {
                            goto FOUND_ID;
                        }
                    }
                }
                break;
            FOUND_ID:
                ++id;
                continue;
            }
            OldEntity newentity = OldEntity.Load(new OldEntity(0, 0x0018, 3, 0, id, 0, 0, 0, 0, 0, new List<EntityPosition>() { new EntityPosition(0, 0, 0) }, 0).Save());
            OldZoneEntry.Entities.Add(newentity);
            AddNode(new OldEntityController(this, newentity));
        }
    }
}
