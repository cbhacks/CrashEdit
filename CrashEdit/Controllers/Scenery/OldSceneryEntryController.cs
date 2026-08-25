using CrashEdit.Crash;
using CrashEdit.Exporters;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldSceneryEntry))]
    public sealed class OldSceneryEntryController : EntryController
    {
        public OldSceneryEntryController(OldSceneryEntry oldsceneryentry, SubcontrollerGroup parentGroup) : base(oldsceneryentry, parentGroup)
        {
            OldSceneryEntry = oldsceneryentry;
            AddMenuSeparator();
            AddMenu("Export as OBJ", Menu_Export_OBJ);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldSceneryEntryViewer(GetNSF(), Entry.EID);
        }

        public OldSceneryEntry OldSceneryEntry { get; }

        private void Menu_Export_OBJ()
        {
            if (!FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any))
                return;
            
            ToOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }

        private void ToOBJ (string path, string modelname)
        {
            var exporter = new OBJExporter ();
            Dictionary <int, int> textureEIDs = new ();
            Dictionary <VertexTexInfo, VertexTexInfo> objTranslate = new Dictionary <VertexTexInfo, VertexTexInfo> ();
            
            exporter.AddScenery (this.GetNSF (), OldSceneryEntry, ref textureEIDs, ref objTranslate);
            exporter.Export (path, modelname);
        }
    }
}
