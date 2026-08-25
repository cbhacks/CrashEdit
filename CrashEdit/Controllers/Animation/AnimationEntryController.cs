using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(AnimationEntry))]
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(AnimationEntry animationentry, SubcontrollerGroup parentGroup) : base(animationentry, parentGroup)
        {
            AnimationEntry = animationentry;
            AddMenu ("Export as OBJ", Menu_Export_OBJ);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new AnimationEntryViewer(GetNSF(), Entry.EID);
        }

        public AnimationEntry AnimationEntry { get; }

        private void Menu_Export_OBJ ()
        {
            if (!FileUtil.SelectSaveFile (out string output, FileFilters.OBJ, FileFilters.Any))
                return;
            
            // modify the path to add a number before the extension
            string ext = Path.GetExtension (output);
            string filename = Path.GetFileNameWithoutExtension (output);
            string path = Path.GetDirectoryName (output);

            int id = 0;
            int count = Modern.SubcontrollerGroups.SelectMany(x => x.Members).Count().ToString().Length;

            foreach (Controller node in Modern.SubcontrollerGroups.SelectMany (x => x.Members))
            {
                if (node.Legacy is not FrameController frame)
                    continue;

                frame.ToOBJ (path, filename + id.ToString().PadLeft (count, '0'));
                id++;
            }
        }
    }
}
