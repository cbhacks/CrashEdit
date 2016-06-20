using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldFrameController : Controller
    {
        private OldAnimationEntryController oldanimationentrycontroller;
        private OldFrame oldframe;
        private TabControl tbcTabs;

        public OldFrameController(OldAnimationEntryController oldanimationentrycontroller,OldFrame oldframe)
        {
            this.oldanimationentrycontroller = oldanimationentrycontroller;
            this.oldframe = oldframe;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Frame";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
            AddMenu("Export as OBJ",Menu_Export_OBJ);
        }

        protected override Control CreateEditor()
        {
            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;
            OldModelEntry modelentry = OldAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldframe.ModelEID);

            OldFrameBox framebox = new OldFrameBox(this);
            framebox.Dock = DockStyle.Fill;
            OldAnimationEntryViewer viewerbox = new OldAnimationEntryViewer(oldframe,modelentry);
            viewerbox.Dock = DockStyle.Fill;

            TabPage edittab = new TabPage("Editor");
            edittab.Controls.Add(framebox);
            TabPage viewertab = new TabPage("Viewer");
            viewertab.Controls.Add(viewerbox);

            tbcTabs.TabPages.Add(edittab);
            tbcTabs.TabPages.Add(viewertab);
            tbcTabs.SelectedTab = edittab;

            return tbcTabs;
        }

        public OldAnimationEntryController OldAnimationEntryController
        {
            get { return oldanimationentrycontroller; }
        }

        public OldFrame OldFrame
        {
            get { return oldframe; }
        }

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = OldAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldframe.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(oldframe.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
