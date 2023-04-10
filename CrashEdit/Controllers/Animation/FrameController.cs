using System.Globalization;
using CrashEdit.Crash;
using CrashEdit.Exporters;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(Frame))]
    public sealed class FrameController : LegacyController
    {
        public FrameController(Frame frame, SubcontrollerGroup parentGroup) : base(parentGroup, frame)
        {
            Frame = frame;
            AddMenu ("Export as OBJ", Menu_Export_OBJ);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new AnimationEntryViewer(GetNSF(), AnimationEntryController.AnimationEntry.EID, AnimationEntryController.AnimationEntry.Frames.IndexOf(Frame));
        }

        public AnimationEntryController AnimationEntryController => (AnimationEntryController)Modern.Parent.Legacy;
        public Frame Frame { get; }
        

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
    }
}
