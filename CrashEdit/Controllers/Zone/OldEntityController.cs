using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldEntity))]
    public sealed class OldEntityController : LegacyController
    {
        public OldEntityController(OldEntity entity, SubcontrollerGroup parentGroup) : base(parentGroup, entity)
        {
            OldEntity = entity;
            AddMenu("Duplicate Entity", Menu_Duplicate);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldEntityBox(this);
        }

        public OldZoneEntryController OldZoneEntryController => Modern.Parent.Legacy as OldZoneEntryController;
        public MapEntryController MapEntryController => Modern.Parent.Legacy as MapEntryController;

        public OldEntity OldEntity { get; }

        private void Menu_Duplicate()
        {
            if (OldZoneEntryController != null)
                Menu_ZoneDuplicate();
            else if (MapEntryController != null)
                Menu_MapDuplicate();
        }

        private void Menu_ZoneDuplicate()
        {
            short id = 6;
            while (true)
            {
                foreach (OldZoneEntry zone in GetEntries<OldZoneEntry>())
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
            OldEntity newentity = OldEntity.Load(OldEntity.Save());
            newentity.ID = id;
            OldZoneEntryController.OldZoneEntry.Entities.Add(newentity);
        }

        private void Menu_MapDuplicate()
        {
            short id = 6;
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
            OldEntity newentity = OldEntity.Load(OldEntity.Save());
            newentity.ID = id;
            MapEntryController.MapEntry.Entities.Add(newentity);
        }
    }
}
