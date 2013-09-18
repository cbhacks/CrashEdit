using Crash;

namespace CrashEdit
{
    public sealed class SceneryEntryController : MysteryMultiItemEntryController
    {
        private SceneryEntry sceneryentry;

        public SceneryEntryController(EntryChunkController entrychunkcontroller,SceneryEntry sceneryentry) : base(entrychunkcontroller,sceneryentry)
        {
            this.sceneryentry = sceneryentry;
            Node.Text = string.Format("Scenery Entry ({0})",sceneryentry.EIDString);
            Node.ImageKey = "sceneryentry";
            Node.SelectedImageKey = "sceneryentry";
        }

        public SceneryEntry SceneryEntry
        {
            get { return sceneryentry; }
        }
    }
}
