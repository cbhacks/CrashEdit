using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldEntityController : Controller
    {
        public OldEntityController(OldZoneEntryController oldzoneentrycontroller,OldEntity entity)
        {
            OldZoneEntryController = oldzoneentrycontroller;
            Entity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            AddMenu("Delete Entity",Menu_Delete);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Entity {0} ({1})",Entity.ID,Entity.Positions.Count);
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldEntityBox(this);
        }

        public OldZoneEntryController OldZoneEntryController { get; }

        public OldEntity Entity { get; }

        private void Menu_Duplicate()
        {
            short maxid = 1;
            foreach (Chunk chunk in OldZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is OldZoneEntry)
                        {
                            foreach (OldEntity otherentity in ((OldZoneEntry)entry).Entities)
                            {
                                if (otherentity.ID > maxid)
                                {
                                    maxid = otherentity.ID;
                                }
                            }
                        }
                    }
                }
            }
            maxid++;
            int newindex = OldZoneEntryController.ZoneEntry.Entities.Count;
            newindex -= BitConv.FromInt32(OldZoneEntryController.ZoneEntry.Unknown1,0x208);
            int entitycount = BitConv.FromInt32(OldZoneEntryController.ZoneEntry.Unknown1,0x20C);
            BitConv.ToInt32(OldZoneEntryController.ZoneEntry.Unknown1,0x20C,entitycount + 1);
            OldEntity newentity = OldEntity.Load(Entity.Save());
            newentity.ID = maxid;
            OldZoneEntryController.ZoneEntry.Entities.Add(newentity);
            OldZoneEntryController.AddNode(new OldEntityController(OldZoneEntryController,newentity));
        }

        private void Menu_Delete()
        {
            int entitycount = BitConv.FromInt32(OldZoneEntryController.ZoneEntry.Unknown1,0x20C);
            BitConv.ToInt32(OldZoneEntryController.ZoneEntry.Unknown1,0x20C,entitycount - 1);
            OldZoneEntryController.ZoneEntry.Entities.Remove(Entity);
            Dispose();
        }
    }
}
