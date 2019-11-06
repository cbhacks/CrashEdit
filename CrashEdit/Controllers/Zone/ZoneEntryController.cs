using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ZoneEntryController : EntryController
    {
        public ZoneEntryController(EntryChunkController entrychunkcontroller,ZoneEntry zoneentry) : base(entrychunkcontroller,zoneentry)
        {
            ZoneEntry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Unknown1));
            AddNode(new ItemController(null,zoneentry.Unknown2));
            foreach (Entity entity in zoneentry.Entities)
            {
                AddNode(new EntityController(this,entity));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Zone ({0})",ZoneEntry.EName);
            Node.ImageKey = "violetb";
            Node.SelectedImageKey = "violetb";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(ZoneEntry.Unknown1,0);
            SceneryEntry[] linkedsceneryentries = new SceneryEntry [linkedsceneryentrycount];
            for (int i = 0;i < linkedsceneryentrycount;i++)
            {
                linkedsceneryentries[i] = FindEID<SceneryEntry>(BitConv.FromInt32(ZoneEntry.Unknown1,4 + i * 48));
            }
            int linkedzoneentrycount = BitConv.FromInt32(ZoneEntry.Unknown1,400);
            ZoneEntry[] linkedzoneentries = new ZoneEntry [linkedzoneentrycount];
            for (int i = 0;i < linkedzoneentrycount;i++)
            {
                linkedzoneentries[i] = FindEID<ZoneEntry>(BitConv.FromInt32(ZoneEntry.Unknown1,404 + i * 4));
            }
            return new UndockableControl(new ZoneEntryViewer(ZoneEntry,linkedsceneryentries,linkedzoneentries));
        }

        public ZoneEntry ZoneEntry { get; }
    }
}
