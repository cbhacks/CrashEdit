using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldAnimationEntry))]
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(OldAnimationEntry oldanimationentry, SubcontrollerGroup parentGroup) : base(oldanimationentry, parentGroup)
        {
            OldAnimationEntry = oldanimationentry;
            AddMenu ("Export as OBJ (game geometry)", Menu_Export_OBJ_Game);
            AddMenu ("Export as OBJ (processed geometry)", Menu_Export_OBJ_Processed);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldAnimationEntryViewer(GetNSF(), Entry.EID);
        }

        public OldAnimationEntry OldAnimationEntry { get; }
        

        private void Menu_Export_OBJ_Processed()
        {
            FileUtil.SelectSaveFile (out string output, FileFilters.OBJ, FileFilters.Any);
            
            // modify the path to add a number before the extension
            string ext = Path.GetExtension (output);
            string filename = Path.GetFileNameWithoutExtension (output);
            string path = Path.GetDirectoryName (output);

            int id = 0;

            foreach (Controller node in Modern.SubcontrollerGroups.SelectMany (x => x.Members))
            {
                if (node.Legacy is not OldFrameController frame)
                    continue;

                string final = path + Path.DirectorySeparatorChar + filename + id.ToString () + ext;
                File.WriteAllBytes (final, frame.ToProcessedOBJ ());
                id++;
            }
        }

        private void Menu_Export_OBJ_Game ()
        {
            FileUtil.SelectSaveFile (out string output, FileFilters.OBJ, FileFilters.Any);
            
            // modify the path to add a number before the extension
            string ext = Path.GetExtension (output);
            string filename = Path.GetFileNameWithoutExtension (output);
            string path = Path.GetDirectoryName (output);

            int id = 0;

            foreach (Controller node in Modern.SubcontrollerGroups.SelectMany (x => x.Members))
            {
                if (node.Legacy is not OldFrameController frame)
                    continue;

                string final = path + Path.DirectorySeparatorChar + filename + id.ToString () + ext;
                File.WriteAllBytes (final, frame.ToGameOBJ ());
                id++;
            }
        }
    }
}
