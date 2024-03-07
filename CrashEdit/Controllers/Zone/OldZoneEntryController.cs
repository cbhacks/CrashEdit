using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldZoneEntry))]
    public sealed class OldZoneEntryController : EntryController
    {
        public OldZoneEntryController(OldZoneEntry zoneentry, SubcontrollerGroup parentGroup)
            : base(zoneentry, parentGroup)
        {
            OldZoneEntry = zoneentry;
            AddMenu("Add Camera", Menu_AddCamera);
            AddMenu("Add Entity", Menu_AddEntity);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldZoneEntryViewer(GetNSF(), Entry.EID);
        }

        public OldZoneEntry OldZoneEntry { get; }

        void Menu_AddCamera()
        {
            OldCamera newcam = OldCamera.Load(new OldCamera(Entry.ENameToEID("NONE!"), 0, 0, new OldCameraNeighbor[4], 0, 0, 0, 0, 1600, 0, 0, 0, 0, 0, 0, new List<OldCameraPosition>(), 0).Save());
            OldZoneEntry.Cameras.Add(newcam);
        }

        void Menu_AddEntity()
        {
            short id = 6;
            var entities = new List<OldEntity>();

            foreach (OldZoneEntry zone in GetEntries<OldZoneEntry>())
            {
                entities.AddRange(zone.Entities);
            }
            while (entities.Find(x => x.ID == id) != null)
            {
                ++id;
            }

            OldEntity newentity = OldEntity.Load(new OldEntity(0x0018, 3, 0, id, 0, 0, 0, 0, 0, new List<EntityPosition>() { new EntityPosition(0, 0, 0) }, 0).Save());
            OldZoneEntry.Entities.Add(newentity);
        }
    }
}
