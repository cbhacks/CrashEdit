using Crash;
using System;

namespace CrashEdit
{
    public abstract class EntryController : Controller
    {
        private EntryChunkController entrychunkcontroller;
        private Entry entry;

        public EntryController(EntryChunkController entrychunkcontroller,Entry entry)
        {
            this.entrychunkcontroller = entrychunkcontroller;
            this.entry = entry;
            AddMenu("Delete Entry",Menu_Delete_Entry);
        }

        public EntryChunkController EntryChunkController
        {
            get { return entrychunkcontroller; }
        }

        public Entry Entry
        {
            get { return entry; }
        }

        public override bool Move(Controller newcontroller,bool commit)
        {
            if (newcontroller is EntryChunkController)
            {
                if (commit)
                {
                    entrychunkcontroller.EntryChunk.Entries.Remove(entry);
                    Node.Remove();
                    entrychunkcontroller = (EntryChunkController)newcontroller;
                    entrychunkcontroller.EntryChunk.Entries.Add(entry);
                    entrychunkcontroller.Node.Nodes.Add(Node);
                }
                return true;
            }
            else
            {
                return base.Move(newcontroller,commit);
            }
        }

        protected T FindEID<T>(int eid) where T : Entry
        {
            return entrychunkcontroller.NSFController.NSF.FindEID<T>(eid);
        }

        private void Menu_Delete_Entry(object sender,EventArgs e)
        {
            entrychunkcontroller.EntryChunk.Entries.Remove(entry);
            Dispose();
        }
    }
}
