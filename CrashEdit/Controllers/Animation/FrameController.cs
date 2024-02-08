using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using Crash;
using System.Windows.Forms;
using CrashEdit.Exporters;
using OpenTK;

namespace CrashEdit
{
    public sealed class FrameController : Controller
    {
        public FrameController(AnimationEntryController animationentrycontroller, Frame frame)
        {
            AnimationEntryController = animationentrycontroller;
            Frame = frame;
            InvalidateNode();
            InvalidateNodeImage();
            AddMenu ("Export as OBJ", Menu_Export_OBJ);
        }

        private void Menu_Export_OBJ ()
        {
            if (!FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any))
                return;
            
            ToOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }
        
        /// <summary>
        /// Exports the model to the OBJ file format ready to be used with other software
        ///
        /// TODO: MAYBE IMPLEMENT AN FBX EXPORT OR SOMETHING ELSE THAT IS A BIT MORE FLEXIBLE?
        ///
        /// This function resides here because access to GameScales is required, and the Frame object does not have access to it
        /// a good improvement might be to move this there
        /// </summary>
        /// <returns></returns>
        public void ToOBJ (string path, string modelname)
        {
            Dictionary <int, int> textureEIDs = new ();
            Dictionary <string, TexInfoUnpacked> objTranslate = new Dictionary <string, TexInfoUnpacked> ();
            
            var exporter = new OBJExporter ();
            
            exporter.AddFrame (AnimationEntryController.NSF, Frame, ref textureEIDs, ref objTranslate);
            exporter.Export (path, modelname);
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
            if (AnimationEntryController.AnimationEntry.IsNew)
                return new Crash3AnimationSelector(AnimationEntryController.NSF, AnimationEntryController.AnimationEntry, Frame);
            else
                return new UndockableControl(new AnimationEntryViewer(AnimationEntryController.NSF, AnimationEntryController.AnimationEntry.EID, AnimationEntryController.AnimationEntry.Frames.IndexOf(Frame)));
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
