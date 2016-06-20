using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class NewZoneEntryController : EntryController
    {
        private NewZoneEntry zoneentry;

        public NewZoneEntryController(EntryChunkController entrychunkcontroller,NewZoneEntry zoneentry) : base(entrychunkcontroller,zoneentry)
        {
            this.zoneentry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Unknown1));
            AddNode(new ItemController(null,zoneentry.Unknown2));
            foreach (Entity entity in zoneentry.Entities)
            {
                AddNode(new NewEntityController(this,entity));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Zone ({0})",zoneentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(zoneentry.Unknown1,0);
            NewSceneryEntry[] linkedsceneryentries = new NewSceneryEntry [linkedsceneryentrycount];
            for (int i = 0;i < linkedsceneryentrycount;i++)
            {
                linkedsceneryentries[i] = FindEID<NewSceneryEntry>(BitConv.FromInt32(zoneentry.Unknown1,4 + i * 48));
            }
            int linkedzoneentrycount = BitConv.FromInt32(zoneentry.Unknown1,400);
            NewZoneEntry[] linkedzoneentries = new NewZoneEntry [linkedzoneentrycount];
            for (int i = 0;i < linkedzoneentrycount;i++)
            {
                linkedzoneentries[i] = FindEID<NewZoneEntry>(BitConv.FromInt32(zoneentry.Unknown1,404 + i * 4));
            }
            return new UndockableControl(new NewZoneEntryViewer(zoneentry,linkedsceneryentries,linkedzoneentries));
        }

        public NewZoneEntry NewZoneEntry
        {
            get { return zoneentry; }
        }
    }
}
