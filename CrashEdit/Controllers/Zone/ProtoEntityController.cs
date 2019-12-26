using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoEntityController : Controller
    {
        public ProtoEntityController(ProtoZoneEntryController oldzoneentrycontroller,ProtoEntity entity)
        {
            ProtoZoneEntryController = oldzoneentrycontroller;
            Entity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            AddMenu("Delete Entity",Menu_Delete);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Proto Entity {0} ({1})",Entity.ID,Entity.Deltas.Count);
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new ProtoEntityBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController { get; }

        public ProtoEntity Entity { get; }

        private void Menu_Duplicate()
        {
            short maxid = 1;
            foreach (Chunk chunk in ProtoZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is ProtoZoneEntry)
                        {
                            foreach (ProtoEntity otherentity in ((ProtoZoneEntry)entry).Entities)
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
            int newindex = ProtoZoneEntryController.ProtoZoneEntry.Entities.Count;
            newindex -= BitConv.FromInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x208);
            int entitycount = BitConv.FromInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C);
            BitConv.ToInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C,entitycount + 1);
            ProtoEntity newentity = ProtoEntity.Load(Entity.Save());
            newentity.ID = maxid;
            ProtoZoneEntryController.ProtoZoneEntry.Entities.Add(newentity);
            ProtoZoneEntryController.AddNode(new ProtoEntityController(ProtoZoneEntryController,newentity));
        }

        private void Menu_Delete()
        {
            int entitycount = BitConv.FromInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C);
            BitConv.ToInt32(ProtoZoneEntryController.ProtoZoneEntry.Header,0x20C,entitycount - 1);
            ProtoZoneEntryController.ProtoZoneEntry.Entities.Remove(Entity);
            Dispose();
        }
    }
}
