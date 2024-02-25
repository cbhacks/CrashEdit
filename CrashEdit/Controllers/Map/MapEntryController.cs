using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(MapEntry))]
    public sealed class MapEntryController : EntryController
    {
        public MapEntryController(MapEntry mapentry, SubcontrollerGroup parentGroup) : base(mapentry, parentGroup)
        {
            MapEntry = mapentry;
            AddMenu("Add Entity", Menu_AddEntity);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new MapEntryViewer(this);
        }

        public MapEntry MapEntry { get; }

        void Menu_AddEntity()
        {
            short id = 1;
            while (true)
            {
                foreach (MapEntry zone in GetEntries<MapEntry>())
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
            OldEntity newentity = OldEntity.Load(new OldEntity(0x0018, 3, 0, id, 0, 0, 0, 0, 0, new List<EntityPosition>() { new EntityPosition(0, 0, 0) }, 0).Save());
            MapEntry.Entities.Add(newentity);
        }
    }
}
