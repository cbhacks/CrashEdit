using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Crash;
using System.Windows.Forms;
using CrashEdit.Exporters;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class OldFrameController : Controller
    {
        public OldFrameController(OldAnimationEntryController oldanimationentrycontroller, OldFrame oldframe)
        {
            OldAnimationEntryController = oldanimationentrycontroller;
            OldFrame = oldframe;
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.FrameController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            var tbcTabs = new TabControl() { Dock = DockStyle.Fill };

            var framebox = new OldFrameBox(this)
            {
                Dock = DockStyle.Fill
            };
            var entry = OldAnimationEntryController.OldAnimationEntry;
            var viewerbox = new OldAnimationEntryViewer(OldAnimationEntryController.NSF, entry.EID, entry.Frames.IndexOf(OldFrame))
            {
                Dock = DockStyle.Fill
            };

            var edittab = new TabPage("Editor");
            edittab.Controls.Add(framebox);
            var viewertab = new TabPage("Viewer");
            viewertab.Controls.Add(new UndockableControl(viewerbox));

            tbcTabs.TabPages.Add(viewertab);
            tbcTabs.TabPages.Add(edittab);
            tbcTabs.SelectedTab = viewertab;

            return tbcTabs;
        }

        public OldAnimationEntryController OldAnimationEntryController { get; }
        public OldFrame OldFrame { get; }

        private void Menu_Export_OBJ()
        {
            if (!FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any))
                return;
            
            ToOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }

        public void ToOBJ(string path, string modelname)
        {
            Dictionary <int, int> textureEIDs = new Dictionary <int, int> ();
            Dictionary <string, TexInfoUnpacked> objTranslate = new Dictionary <string, TexInfoUnpacked> ();
            
            var exporter = new OBJExporter ();
            
            exporter.AddFrame (OldAnimationEntryController.NSF, OldFrame, ref textureEIDs, ref objTranslate);
            exporter.Export (path, modelname);
        }
    }
}
