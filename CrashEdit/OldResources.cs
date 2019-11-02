using System;
using System.IO;
using System.Drawing;
using System.Resources;
using System.Reflection;

namespace CrashEdit
{
    internal static class OldResources
    {
        [Resource("NSDIcon")]
        private static Icon nsdicon = null;

        [Resource("NSFIcon")]
        private static Icon nsficon = null;

        [Resource("ArrowImage")]
        private static Image arrowimage = null;

        [Resource("BinocularsImage")]
        private static Image binocularsimage = null;

        [Resource("BinocularsNextImage")]
        private static Image binocularsnextimage = null;

        [Resource("FileImage")]
        private static Image fileimage = null;

        [Resource("FolderImage")]
        private static Image folderimage = null;

        [Resource("ImageImage")]
        private static Image imageimage = null;

        [Resource("MusicImage")]
        private static Image musicimage = null;

        [Resource("OpenImage")]
        private static Image openimage = null;

        [Resource("SaveImage")]
        private static Image saveimage = null;

        [Resource("SpeakerImage")]
        private static Image speakerimage = null;

        [Resource("ThingImage")]
        private static Image thingimage = null;

        [Resource("BlueJournalImage")]
        private static Image bluejournalimage = null;

        [Resource("WhiteJournalImage")]
        private static Image whitejournalimage = null;

        [Resource("YellowJournalImage")]
        private static Image yellowjournalimage = null;

        [Resource("ActivatorBoxTexture")]
        [ExternalTexture(2,1)]
        private static Bitmap activatorboxtexture = null;

        [Resource("AppleTexture")]
        private static Bitmap appletexture = null;

        [Resource("AppleBoxTexture")]
        [ExternalTexture(4,0)]
        private static Bitmap appleboxtexture = null;

        [Resource("ArrowBoxTexture")]
        [ExternalTexture(5,0)]
        private static Bitmap arrowboxtexture = null;

        [Resource("BodyslamBoxTexture")]
        [ExternalTexture(0,1)]
        private static Bitmap bodyslamboxtexture = null;

        [Resource("BoxTexture")]
        [ExternalTexture(0,0)]
        private static Bitmap boxtexture = null;

        [Resource("CheckpointTexture")]
        [ExternalTexture(6,0)]
        private static Bitmap checkpointtexture = null;

        [Resource("DetonatorBoxTexture")]
        [ExternalTexture(3,1)]
        private static Bitmap detonatorboxtexture = null;

        [Resource("DetonatorBoxTopTexture")]
        private static Bitmap detonatorboxtoptexture = null;

        [Resource("IronArrowBoxTexture")]
        [ExternalTexture(4,1)]
        private static Bitmap ironarrowboxtexture = null;

        [Resource("IronBoxTexture")]
        [ExternalTexture(1,1)]
        private static Bitmap ironboxtexture = null;

        [Resource("LifeTexture")]
        private static Bitmap lifetexture = null;

        [Resource("LifeBoxTexture")]
        [ExternalTexture(3,0)]
        private static Bitmap lifeboxtexture = null;

        [Resource("MaskTexture")]
        private static Bitmap masktexture = null;

        [Resource("MaskBoxTexture")]
        [ExternalTexture(2,0)]
        private static Bitmap maskboxtexture = null;

        [Resource("NitroTexture")]
        [ExternalTexture(7,1)]
        private static Bitmap nitrotexture = null;

        [Resource("NitroTopTexture")]
        [ExternalTexture(6,1)]
        private static Bitmap nitrotoptexture = null;

        [Resource("PointTexture")]
        private static Bitmap pointtexture = null;

        [Resource("QuestionMarkBoxTexture")]
        [ExternalTexture(1,0)]
        private static Bitmap questionmarkboxtexture = null;

        [Resource("TNTTexture")]
        [ExternalTexture(8,0)]
        private static Bitmap tnttexture = null;

        [Resource("TNTTopTexture")]
        [ExternalTexture(7,0)]
        private static Bitmap tnttoptexture = null;

        [Resource("UnknownBoxTexture")]
        private static Bitmap unknownboxtexture = null;

        [Resource("UnknownBoxTopTexture")]
        private static Bitmap unknownboxtoptexture = null;

        [Resource("UnknownPickupTexture")]
        private static Bitmap unknownpickuptexture = null;

        static OldResources()
        {
            ResourceManager manager = new ResourceManager("CrashEdit.OldResources",Assembly.GetExecutingAssembly());
            foreach (FieldInfo field in typeof(OldResources).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                foreach (ResourceAttribute attribute in field.GetCustomAttributes(typeof(ResourceAttribute),false))
                {
                    field.SetValue(null,manager.GetObject(attribute.Name));
                }
            }
            string exefilename = Assembly.GetExecutingAssembly().Location;
            string exedirname = Path.GetDirectoryName(exefilename);
            string texturespngfilename = Path.Combine(exedirname,"Textures.png");
            if (File.Exists(texturespngfilename))
            {
                using (Image texturespng = Image.FromFile(texturespngfilename))
                {
                    foreach (FieldInfo field in typeof(OldResources).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    {
                        foreach (ExternalTextureAttribute attribute in field.GetCustomAttributes(typeof(ExternalTextureAttribute),false))
                        {
                            Bitmap texture = new Bitmap(32,32);
                            int x = attribute.X * 33;
                            int y = attribute.Y * 33;
                            using (Graphics g = Graphics.FromImage(texture))
                            {
                                g.DrawImage(texturespng,new Rectangle(0,0,32,32),new Rectangle(x,y,32,32),GraphicsUnit.Pixel);
                            }
                            field.SetValue(null,texture);
                        }
                    }
                }
            }
        }

        public static Icon NSDIcon => nsdicon;
        public static Icon NSFIcon => nsficon;
        public static Image ArrowImage => arrowimage;
        public static Image BinocularsImage => binocularsimage;
        public static Image BinocularsNextImage => binocularsnextimage;
        public static Image FileImage => fileimage;
        public static Image FolderImage => folderimage;
        public static Image ImageImage => imageimage;
        public static Image MusicImage => musicimage;
        public static Image OpenImage => openimage;
        public static Image SaveImage => saveimage;
        public static Image SpeakerImage => speakerimage;
        public static Image ThingImage => thingimage;
        public static Image BlueJournalImage => bluejournalimage;
        public static Image WhiteJournalImage => whitejournalimage;
        public static Image YellowJournalImage => yellowjournalimage;
        public static Bitmap ActivatorBoxTexture => activatorboxtexture;
        public static Bitmap AppleTexture => appletexture;
        public static Bitmap AppleBoxTexture => appleboxtexture;
        public static Bitmap ArrowBoxTexture => arrowboxtexture;
        public static Bitmap BodyslamBoxTexture => bodyslamboxtexture;
        public static Bitmap BoxTexture => boxtexture;
        public static Bitmap CheckpointTexture => checkpointtexture;
        public static Bitmap DetonatorBoxTexture => detonatorboxtexture;
        public static Bitmap DetonatorBoxTopTexture => detonatorboxtoptexture;
        public static Bitmap IronArrowBoxTexture => ironarrowboxtexture;
        public static Bitmap IronBoxTexture => ironboxtexture;
        public static Bitmap LifeTexture => lifetexture;
        public static Bitmap LifeBoxTexture => lifeboxtexture;
        public static Bitmap MaskTexture => masktexture;
        public static Bitmap MaskBoxTexture => maskboxtexture;
        public static Bitmap NitroTexture => nitrotexture;
        public static Bitmap NitroTopTexture => nitrotoptexture;
        public static Bitmap PointTexture => pointtexture;
        public static Bitmap QuestionMarkBoxTexture => questionmarkboxtexture;
        public static Bitmap TNTTexture => tnttexture;
        public static Bitmap TNTTopTexture => tnttoptexture;
        public static Bitmap UnknownBoxTexture => unknownboxtexture;
        public static Bitmap UnknownBoxTopTexture => unknownboxtoptexture;
        public static Bitmap UnknownPickupTexture => unknownpickuptexture;

        [AttributeUsage(AttributeTargets.Field)]
        private class ResourceAttribute : Attribute
        {
            public ResourceAttribute(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class ExternalTextureAttribute : Attribute
        {
            public ExternalTextureAttribute(int x,int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }
    }
}
