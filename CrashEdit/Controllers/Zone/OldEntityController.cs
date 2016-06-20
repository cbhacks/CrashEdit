using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldEntityController : Controller
    {
        private OldZoneEntryController oldzoneentrycontroller;
        private OldEntity entity;

        public OldEntityController(OldZoneEntryController oldzoneentrycontroller,OldEntity entity)
        {
            this.oldzoneentrycontroller = oldzoneentrycontroller;
            this.entity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            AddMenu("Delete Entity",Menu_Delete);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Entity {0} ({1})",entity.ID,entity.Index.Count);
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldEntityBox(this);
        }

        public OldZoneEntryController OldZoneEntryController
        {
            get { return oldzoneentrycontroller; }
        }

        public OldEntity Entity
        {
            get { return entity; }
        }

        private void Menu_Duplicate()
        {
            short maxid = 1;
            foreach (Chunk chunk in oldzoneentrycontroller.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is OldZoneEntry)
                        {
                            foreach (OldEntity otherentity in ((OldZoneEntry)entry).Entities)
                            {
                                if (otherentity.ID.HasValue)
                                {
                                    if (otherentity.ID.Value > maxid)
                                    {
                                        maxid = otherentity.ID.Value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            maxid++;
            int newindex = oldzoneentrycontroller.ZoneEntry.Entities.Count;
            newindex -= BitConv.FromInt32(oldzoneentrycontroller.ZoneEntry.Unknown1,0x208);
            int entitycount = BitConv.FromInt32(oldzoneentrycontroller.ZoneEntry.Unknown1,0x20C);
            BitConv.ToInt32(oldzoneentrycontroller.ZoneEntry.Unknown1,0x20C,entitycount + 1);
            OldEntity newentity = OldEntity.Load(entity.Save());
            newentity.ID = maxid;
            oldzoneentrycontroller.ZoneEntry.Entities.Add(newentity);
            oldzoneentrycontroller.AddNode(new OldEntityController(oldzoneentrycontroller,newentity));
        }

        private void Menu_Delete()
        {
            int entitycount = BitConv.FromInt32(oldzoneentrycontroller.ZoneEntry.Unknown1,0x20C);
            BitConv.ToInt32(oldzoneentrycontroller.ZoneEntry.Unknown1,0x20C,entitycount - 1);
            oldzoneentrycontroller.ZoneEntry.Entities.Remove(entity);
            Dispose();
        }
    }
}
