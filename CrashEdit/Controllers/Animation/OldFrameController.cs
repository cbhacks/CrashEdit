using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldFrame))]
    public sealed class OldFrameController : LegacyController
    {
        public OldFrameController(OldFrame oldframe, SubcontrollerGroup parentGroup) : base(parentGroup, oldframe)
        {
            OldFrame = oldframe;
            AddMenu("Export as OBJ", Menu_Export_OBJ);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            TabControl tbcTabs = new TabControl() { Dock = DockStyle.Fill };
            OldModelEntry modelentry = GetEntry<OldModelEntry>(OldFrame.ModelEID);

            var framebox = new OldFrameBox(this)
            {
                Dock = DockStyle.Fill
            };
            var entry = OldAnimationEntryController.OldAnimationEntry;
            var viewerbox = new OldAnimationEntryViewer(OldAnimationEntryController.NSF, entry.EID, entry.Frames.IndexOf(OldFrame))
            {
                Dock = DockStyle.Fill
            };
            framebox.Dock = DockStyle.Fill;

            TabPage edittab = new TabPage("Editor");
            edittab.Controls.Add(framebox);
            TabPage viewertab = new TabPage("Viewer");
            viewertab.Controls.Add(viewerbox);

            tbcTabs.TabPages.Add(viewertab);
            tbcTabs.TabPages.Add(edittab);
            tbcTabs.SelectedTab = viewertab;

            return tbcTabs;
        }

        public OldAnimationEntryController OldAnimationEntryController => Modern.Parent.Legacy as OldAnimationEntryController;
        public ColoredAnimationEntryController ColoredAnimationEntryController => Modern.Parent.Legacy as ColoredAnimationEntryController;
        public OldFrame OldFrame { get; }

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = GetEntry<OldModelEntry>(OldFrame.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?", "Export as OBJ", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(OldFrame.ToOBJ(modelentry), FileFilters.OBJ, FileFilters.Any);
        }
    }
}
