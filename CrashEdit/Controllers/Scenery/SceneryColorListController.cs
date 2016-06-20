using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SceneryColorListController : Controller
    {
        private SceneryEntryController sceneryentrycontroller;
        private SceneryColorList colorlist;

        public SceneryColorListController(SceneryEntryController sceneryentrycontroller,SceneryColorList colorlist)
        {
            this.sceneryentrycontroller = sceneryentrycontroller;
            this.colorlist = colorlist;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Color List");
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new SceneryColorListBox(this);
        }

        public SceneryEntryController SceneryEntryController
        {
            get { return sceneryentrycontroller; }
        }

        public SceneryColorList ColorList
        {
            get { return colorlist; }
        }
    }
}
