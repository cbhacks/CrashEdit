using CrashEdit.Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldFrameController : LegacyController
    {
        public OldFrameController(ProtoAnimationEntryController protoanimationentrycontroller, OldFrame oldframe) : base(protoanimationentrycontroller, oldframe)
        {
            OldFrame = oldframe;
            AddMenu("Export as OBJ", Menu_Export_OBJ);
            InvalidateNode();
        }

        public OldFrameController(OldAnimationEntryController oldanimationentrycontroller,OldFrame oldframe) : base(oldanimationentrycontroller, oldframe)
        {
            OldFrame = oldframe;
            AddMenu("Export as OBJ", Menu_Export_OBJ);
            InvalidateNode();
        }

        public void InvalidateNode()
        {
            NodeText = CrashUI.Properties.Resources.FrameController_Text;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            TabControl tbcTabs = new TabControl() { Dock = DockStyle.Fill };
            EntryController entry = OldAnimationEntryController != null ? (EntryController)OldAnimationEntryController : (EntryController)ProtoAnimationEntryController;
            OldModelEntry modelentry = GetEntry<OldModelEntry>(OldFrame.ModelEID);

            OldFrameBox framebox = new OldFrameBox(this);
            framebox.Dock = DockStyle.Fill;
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            foreach (OldModelStruct str in modelentry.Structs)
                if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                    textures.Add(tex.EID, GetEntry<TextureChunk>(tex.EID));
            OldAnimationEntryViewer viewerbox = new OldAnimationEntryViewer(OldFrame,false,modelentry,textures) { Dock = DockStyle.Fill };

            TabPage edittab = new TabPage("Editor");
            edittab.Controls.Add(framebox);
            TabPage viewertab = new TabPage("Viewer");
            viewertab.Controls.Add(viewerbox);

            tbcTabs.TabPages.Add(viewertab);
            tbcTabs.TabPages.Add(edittab);
            tbcTabs.SelectedTab = viewertab;

            return tbcTabs;
        }

        public ProtoAnimationEntryController ProtoAnimationEntryController => Modern.Parent.Legacy as ProtoAnimationEntryController;
        public OldAnimationEntryController OldAnimationEntryController => Modern.Parent.Legacy as OldAnimationEntryController;
        public OldFrame OldFrame { get; }

        private void Menu_Export_OBJ()
        {
            EntryController entry = OldAnimationEntryController != null ? (EntryController)OldAnimationEntryController : (EntryController)ProtoAnimationEntryController;
            OldModelEntry modelentry = GetEntry<OldModelEntry>(OldFrame.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(OldFrame.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
