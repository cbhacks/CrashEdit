using System.Drawing;
using System.Resources;
using System.Reflection;

namespace CrashEdit
{
    internal static class Resources
    {
        private static ResourceManager manager;

        static Resources()
        {
            manager = new ResourceManager("CrashEdit.Resources",Assembly.GetExecutingAssembly());
        }

        public static Image ArrowIcon
        {
            get
            {
                return (Image)manager.GetObject("ArrowIcon");
            }
        }

        public static Image FileIcon
        {
            get
            {
                return (Image)manager.GetObject("FileIcon");
            }
        }

        public static Image FolderIcon
        {
            get
            {
                return (Image)manager.GetObject("FolderIcon");
            }
        }

        public static Image ImageIcon
        {
            get
            {
                return (Image)manager.GetObject("ImageIcon");
            }
        }

        public static Image MusicIcon
        {
            get
            {
                return (Image)manager.GetObject("MusicIcon");
            }
        }

        public static Image OpenIcon
        {
            get
            {
                return (Image)manager.GetObject("OpenIcon");
            }
        }

        public static Image SaveIcon
        {
            get
            {
                return (Image)manager.GetObject("SaveIcon");
            }
        }

        public static Image SpeakerIcon
        {
            get
            {
                return (Image)manager.GetObject("SpeakerIcon");
            }
        }

        public static Image ThingIcon
        {
            get
            {
                return (Image)manager.GetObject("ThingIcon");
            }
        }

        public static Image BlueJournalIcon
        {
            get
            {
                return (Image)manager.GetObject("BlueJournalIcon");
            }
        }

        public static Image WhiteJournalIcon
        {
            get
            {
                return (Image)manager.GetObject("WhiteJournalIcon");
            }
        }

        public static Image YellowJournalIcon
        {
            get
            {
                return (Image)manager.GetObject("YellowJournalIcon");
            }
        }
    }
}
