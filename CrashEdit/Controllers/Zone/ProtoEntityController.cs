using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(ProtoEntity))]
    public sealed class ProtoEntityController : LegacyController
    {
        public ProtoEntityController(ProtoEntity entity, SubcontrollerGroup parentGroup) : base(parentGroup, entity)
        {
            Entity = entity;
            //AddMenu("Duplicate Entity",Menu_Duplicate);
            //AddMenu("Delete Entity",Menu_Delete);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new ProtoEntityBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController => (ProtoZoneEntryController)Modern.Parent.Legacy;

        public ProtoEntity Entity { get; }

        //private void Menu_Duplicate()
        //{
        //    short maxid = 1;
        //    foreach (ProtoZoneEntry zone in ProtoZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<ProtoZoneEntry>())
        //    {
        //        foreach (ProtoEntity otherentity in zone.Entities)
        //        {
        //            if (otherentity.ID > maxid)
        //            {
        //                maxid = otherentity.ID;
        //            }
        //        }
        //    }
        //    maxid++;
        //    int newindex = ProtoZoneEntryController.ProtoZoneEntry.Entities.Count;
        //    newindex -= BitConv.FromInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x208);
        //    int entitycount = BitConv.FromInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C);
        //    BitConv.ToInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C,entitycount + 1);
        //    ProtoEntity newentity = ProtoEntity.Load(Entity.Save());
        //    newentity.ID = maxid;
        //    ProtoZoneEntryController.ProtoZoneEntry.Entities.Add(newentity);
        //}

        //private void Menu_Delete()
        //{
        //    int entitycount = BitConv.FromInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C);
        //    BitConv.ToInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C,entitycount - 1);
        //    ProtoZoneEntryController.ProtoZoneEntry.Entities.Remove(Entity);
        //}
    }
}
