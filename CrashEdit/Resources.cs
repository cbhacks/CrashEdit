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

        public static Image ArrowImage
        {
            get
            {
                return (Image)manager.GetObject("ArrowImage");
            }
        }

        public static Image BinocularsImage
        {
            get
            {
                return (Image)manager.GetObject("BinocularsImage");
            }
        }

        public static Image BinocularsNextImage
        {
            get
            {
                return (Image)manager.GetObject("BinocularsNextImage");
            }
        }

        public static Image FileImage
        {
            get
            {
                return (Image)manager.GetObject("FileImage");
            }
        }

        public static Image FolderImage
        {
            get
            {
                return (Image)manager.GetObject("FolderImage");
            }
        }

        public static Image ImageImage
        {
            get
            {
                return (Image)manager.GetObject("ImageImage");
            }
        }

        public static Image MusicImage
        {
            get
            {
                return (Image)manager.GetObject("MusicImage");
            }
        }

        public static Image OpenImage
        {
            get
            {
                return (Image)manager.GetObject("OpenImage");
            }
        }

        public static Image SaveImage
        {
            get
            {
                return (Image)manager.GetObject("SaveImage");
            }
        }

        public static Image SpeakerImage
        {
            get
            {
                return (Image)manager.GetObject("SpeakerImage");
            }
        }

        public static Image ThingImage
        {
            get
            {
                return (Image)manager.GetObject("ThingImage");
            }
        }

        public static Image BlueJournalImage
        {
            get
            {
                return (Image)manager.GetObject("BlueJournalImage");
            }
        }

        public static Image WhiteJournalImage
        {
            get
            {
                return (Image)manager.GetObject("WhiteJournalImage");
            }
        }

        public static Image YellowJournalImage
        {
            get
            {
                return (Image)manager.GetObject("YellowJournalImage");
            }
        }
    }
}
