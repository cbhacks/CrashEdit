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

        public static Bitmap ActivatorBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("ActivatorBoxTexture");
            }
        }

        public static Bitmap AppleTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("AppleTexture");
            }
        }

        public static Bitmap AppleBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("AppleBoxTexture");
            }
        }

        public static Bitmap ArrowBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("ArrowBoxTexture");
            }
        }

        public static Bitmap BodyslamBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("BodyslamBoxTexture");
            }
        }

        public static Bitmap BoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("BoxTexture");
            }
        }

        public static Bitmap CheckpointTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("CheckpointTexture");
            }
        }

        public static Bitmap DetonatorBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("DetonatorBoxTexture");
            }
        }

        public static Bitmap DetonatorBoxTopTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("DetonatorBoxTopTexture");
            }
        }

        public static Bitmap IronArrowBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("IronArrowBoxTexture");
            }
        }

        public static Bitmap IronBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("IronBoxTexture");
            }
        }

        public static Bitmap LifeTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("LifeTexture");
            }
        }

        public static Bitmap LifeBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("LifeBoxTexture");
            }
        }

        public static Bitmap MaskTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("MaskTexture");
            }
        }

        public static Bitmap MaskBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("MaskBoxTexture");
            }
        }

        public static Bitmap NitroTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("NitroTexture");
            }
        }

        public static Bitmap NitroTopTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("NitroTopTexture");
            }
        }

        public static Bitmap QuestionMarkBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("QuestionMarkBoxTexture");
            }
        }

        public static Bitmap TNTTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("TNTTexture");
            }
        }

        public static Bitmap TNTTopTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("TNTTopTexture");
            }
        }

        public static Bitmap UnknownBoxTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("UnknownBoxTexture");
            }
        }

        public static Bitmap UnknownBoxTopTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("UnknownBoxTopTexture");
            }
        }

        public static Bitmap UnknownPickupTexture
        {
            get
            {
                return (Bitmap)manager.GetObject("UnknownPickupTexture");
            }
        }
    }
}
