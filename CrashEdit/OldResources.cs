using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;

namespace CrashEdit
{
    internal static class OldResources
    {
        [Resource("NSDIcon")]
        private static readonly Icon nsdicon = null;

        [Resource("NSFIcon")]
        private static readonly Icon nsficon = null;

        [Resource("CBHacksIcon")]
        private static readonly Icon cbhacksicon = null;

        [Resource("ArrowImage")]
        private static readonly Image arrowimage = null;

        [Resource("BinocularsImage")]
        private static readonly Image binocularsimage = null;

        [Resource("BinocularsNextImage")]
        private static readonly Image binocularsnextimage = null;

        [Resource("FileImage")]
        private static readonly Image fileimage = null;

        [Resource("FolderImage")]
        private static readonly Image folderimage = null;

        [Resource("ImageImage")]
        private static readonly Image imageimage = null;

        [Resource("MusicImage")]
        private static readonly Image musicimage = null;

        [Resource("MusicRedImage")]
        private static readonly Image musicredimage = null;

        [Resource("MusicYellowImage")]
        private static readonly Image musicyellowimage = null;

        [Resource("OpenImage")]
        private static readonly Image openimage = null;

        [Resource("SaveImage")]
        private static readonly Image saveimage = null;

        [Resource("SpeakerImage")]
        private static readonly Image speakerimage = null;

        [Resource("ThingImage")]
        private static readonly Image thingimage = null;

        [Resource("BlueJournalImage")]
        private static readonly Image bluejournalimage = null;

        [Resource("WhiteJournalImage")]
        private static readonly Image whitejournalimage = null;

        [Resource("YellowJournalImage")]
        private static readonly Image yellowjournalimage = null;

        [Resource("PointTexture")]
        private static readonly Bitmap pointtexture = null;

        [Resource("MaskTexture")]
        [ExternalTexture(10, 3, 2, 2)]
        private static readonly Bitmap masktexture = null;

        [Resource("LifeTexture")]
        [ExternalTexture(0, 4, 2, 1)]
        private static readonly Bitmap lifetexture = null;

        [Resource("AppleTexture")]
        [ExternalTexture(0, 3)]
        private static readonly Bitmap appletexture = null;

        [Resource("TNTBoxTexture")]
        [ExternalTexture(1, 0)]
        private static readonly Bitmap tntboxtexture = null;

        [Resource("TNTBoxTopTexture")]
        [ExternalTexture(0, 0)]
        private static readonly Bitmap tntboxtoptexture = null;

        [Resource("EmptyBoxTexture")]
        [ExternalTexture(5, 0)]
        private static readonly Bitmap emptyboxtexture = null;

        [Resource("SpringBoxTexture")]
        [ExternalTexture(6, 0)]
        private static readonly Bitmap springboxtexture = null;

        [Resource("ContinueBoxTexture")]
        [ExternalTexture(7, 0)]
        private static readonly Bitmap continueboxtexture = null;

        [Resource("IronBoxTexture")]
        [ExternalTexture(8, 0)]
        private static readonly Bitmap ironboxtexture = null;

        [Resource("FruitBoxTexture")]
        [ExternalTexture(9, 0)]
        private static readonly Bitmap fruitboxtexture = null;

        [Resource("ActionBoxTexture")]
        [ExternalTexture(10, 0)]
        private static readonly Bitmap actionboxtexture = null;

        [Resource("LifeBoxTexture")]
        [ExternalTexture(11, 0)]
        private static readonly Bitmap lifeboxtexture = null;

        [Resource("DoctorBoxTexture")]
        [ExternalTexture(12, 0)]
        private static readonly Bitmap doctorboxtexture = null;

        [Resource("PickupBoxTexture")]
        [ExternalTexture(0, 1)]
        private static readonly Bitmap pickupboxtexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(1, 1)]
        private static readonly Bitmap powboxtexture = null;

        [Resource("IronSpringBoxTexture")]
        [ExternalTexture(2, 1)]
        private static readonly Bitmap ironspringboxtexture = null;

        [Resource("NitroBoxTexture")]
        [ExternalTexture(4, 1)]
        private static readonly Bitmap nitroboxtexture = null;

        [Resource("NitroBoxTopTexture")]
        [ExternalTexture(3, 1)]
        private static readonly Bitmap nitroboxtoptexture = null;

        [Resource("SteelBoxTexture")]
        [ExternalTexture(5, 1)]
        private static readonly Bitmap steelboxtexture = null;

        [Resource("ActionNitroBoxTexture")]
        [ExternalTexture(6, 1)]
        private static readonly Bitmap actionnitroboxtexture = null;

        [Resource("ActionNitroBoxTopTexture")]
        [ExternalTexture(8, 0)]
        private static readonly Bitmap actionnitroboxtoptexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(9, 1)]
        private static readonly Bitmap slotboxtexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(3, 2)]
        private static readonly Bitmap time1boxtexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(4, 2)]
        private static readonly Bitmap time2boxtexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(5, 2)]
        private static readonly Bitmap time3boxtexture = null;

        [Resource("UnknownBoxTopTexture")]
        [ExternalTexture(2, 2)]
        private static readonly Bitmap timeboxtoptexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(6, 2)]
        private static readonly Bitmap ironcontinueboxtexture = null;

        [Resource("UnknownBoxTexture")]
        [ExternalTexture(7, 2)]
        private static readonly Bitmap clockboxtexture = null;

        [Resource("UnknownBoxTexture")]
        private static readonly Bitmap unknownboxtexture = null;

        [Resource("UnknownBoxTopTexture")]
        private static readonly Bitmap unknownboxtoptexture = null;

        [Resource("UnknownPickupTexture")]
        private static readonly Bitmap unknownpickuptexture = null;

        [Resource("GreyBuckle")]
        private static readonly Image greybuckle = null;

        [Resource("CodeBuckle")]
        private static readonly Image codebuckle = null;

        [Resource("CrimsonBuckle")]
        private static readonly Image crimsonbuckle = null;

        [Resource("LimeBuckle")]
        private static readonly Image limebuckle = null;

        [Resource("BlueBuckle")]
        private static readonly Image bluebuckle = null;

        [Resource("VioletBuckle")]
        private static readonly Image violetbuckle = null;

        [Resource("RedBuckle")]
        private static readonly Image redbuckle = null;

        [Resource("YellowBuckle")]
        private static readonly Image yellowbuckle = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(1, 3)]
        private static readonly Bitmap fruitlime = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(2, 3)]
        private static readonly Bitmap fruitcoconut = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(12, 3, 1, 2)]
        private static readonly Bitmap fruitpineapple = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(3, 3)]
        private static readonly Bitmap fruitstrawberry = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(4, 3)]
        private static readonly Bitmap fruitmango = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(5, 3)]
        private static readonly Bitmap fruitlemon = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(6, 3)]
        private static readonly Bitmap fruityyy = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(7, 3)]
        private static readonly Bitmap fruitgrape = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(2, 4, 2, 1)]
        private static readonly Bitmap fruitcortex = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(8, 3, 2, 2)]
        private static readonly Bitmap fruitbrio = null;

        [Resource("UnknownPickupTexture")]
        [ExternalTexture(4, 4, 2, 1)]
        private static readonly Bitmap fruittawna = null;

        private static readonly Bitmap alltex = null;

        static OldResources()
        {
            List<FieldInfo> allfields = new();
            foreach (var field in typeof(OldResources).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (field.Name != "alltex")
                {
                    allfields.Add(field);
                }
            }
            var manager = new ResourceManager("CrashEdit.OldResources", Assembly.GetExecutingAssembly());
            foreach (FieldInfo field in allfields)
            {
                foreach (ResourceAttribute attribute in field.GetCustomAttributes(typeof(ResourceAttribute), false))
                {
                    field.SetValue(null, manager.GetObject(attribute.Name));
                }
            }
            string exefilename = Assembly.GetExecutingAssembly().Location;
            string exedirname = Path.GetDirectoryName(exefilename);
            string texturespngfilename = Path.Combine(exedirname, "Textures.png");
            if (File.Exists(texturespngfilename))
            {
                using (Image texturespng = Image.FromFile(texturespngfilename))
                {
                    foreach (FieldInfo field in allfields)
                    {
                        foreach (ExternalTextureAttribute attribute in field.GetCustomAttributes(typeof(ExternalTextureAttribute), false))
                        {
                            int w = attribute.W * 32;
                            int h = attribute.H * 32;
                            int x = attribute.X * 32;
                            int y = attribute.Y * 32;
                            if (texturespng.Width < x + w || texturespng.Height < y + h)
                                continue;
                            var texture = new Bitmap(w, h);
                            using (Graphics g = Graphics.FromImage(texture))
                            {
                                g.DrawImage(texturespng, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
                            }
                            field.SetValue(null, texture);
                        }
                    }
                }
            }
            List<Bitmap> bmps = new();
            foreach (FieldInfo field in allfields)
            {
                if (field.FieldType == typeof(Bitmap))
                {
                    bmps.Add(field.GetValue(null) as Bitmap);
                }
            }
            int aw = 0, tw = 0, th = 0, mh = int.MaxValue;
            foreach (var bmp in bmps)
            {
                tw = Math.Max(tw, bmp.Width);
                th = Math.Max(th, bmp.Height);
                mh = Math.Min(mh, bmp.Height);
                aw += bmp.Width;
            }
            bmps.Sort((Bitmap a, Bitmap b) =>
            {
                int d = b.Width - a.Width;
                if (d == 0)
                {
                    d = b.Height - a.Height;
                }
                return d;
            });
            alltex = new(aw, th);
            TexMap = new();
            using (Graphics g = Graphics.FromImage(alltex))
            {
                int curx = 0;
                foreach (var bmp in bmps)
                {
                    var destRect = new Rectangle(curx, 0, bmp.Width, bmp.Height);
                    g.DrawImage(bmp, destRect, new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                    TexMap.Add(bmp, destRect);
                    curx += bmp.Width;
                }
            }
            // alltex.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static Bitmap AllTex => alltex;
        public static Dictionary<Bitmap, Rectangle> TexMap { get; }

        public static Icon NSDIcon => nsdicon;
        public static Icon NSFIcon => nsficon;
        public static Icon CBHacksIcon => cbhacksicon;
        public static Image ArrowImage => arrowimage;
        public static Image BinocularsImage => binocularsimage;
        public static Image BinocularsNextImage => binocularsnextimage;
        public static Image FileImage => fileimage;
        public static Image FolderImage => folderimage;
        public static Image ImageImage => imageimage;
        public static Image MusicImage => musicimage;
        public static Image MusicRedImage => musicredimage;
        public static Image MusicYellowImage => musicyellowimage;
        public static Image OpenImage => openimage;
        public static Image SaveImage => saveimage;
        public static Image SpeakerImage => speakerimage;
        public static Image ThingImage => thingimage;
        public static Image BlueJournalImage => bluejournalimage;
        public static Image WhiteJournalImage => whitejournalimage;
        public static Image YellowJournalImage => yellowjournalimage;
        public static Bitmap AppleTexture => appletexture;
        public static Bitmap LifeTexture => lifetexture;
        public static Bitmap MaskTexture => masktexture;
        public static Bitmap PointTexture => pointtexture;
        public static Bitmap ActionBoxTexture => actionboxtexture;
        public static Bitmap ActionNitroBoxTexture => actionnitroboxtexture;
        public static Bitmap ActionNitroBoxTopTexture => actionnitroboxtoptexture;
        public static Bitmap ClockBoxTexture => clockboxtexture;
        public static Bitmap ContinueBoxTexture => continueboxtexture;
        public static Bitmap DoctorBoxTexture => doctorboxtexture;
        public static Bitmap EmptyBoxTexture => emptyboxtexture;
        public static Bitmap FruitBoxTexture => fruitboxtexture;
        public static Bitmap IronBoxTexture => ironboxtexture;
        public static Bitmap IronContinueBoxTexture => ironcontinueboxtexture;
        public static Bitmap IronSpringBoxTexture => ironspringboxtexture;
        public static Bitmap LifeBoxTexture => lifeboxtexture;
        public static Bitmap NitroBoxTexture => nitroboxtexture;
        public static Bitmap NitroBoxTopTexture => nitroboxtoptexture;
        public static Bitmap PickupBoxTexture => pickupboxtexture;
        public static Bitmap POWBoxTexture => powboxtexture;
        public static Bitmap SlotBoxTexture => slotboxtexture;
        public static Bitmap SpringBoxTexture => springboxtexture;
        public static Bitmap SteelBoxTexture => steelboxtexture;
        public static Bitmap Time1BoxTexture => time1boxtexture;
        public static Bitmap Time2BoxTexture => time2boxtexture;
        public static Bitmap Time3BoxTexture => time3boxtexture;
        public static Bitmap TimeBoxTopTexture => timeboxtoptexture;
        public static Bitmap TNTBoxTexture => tntboxtexture;
        public static Bitmap TNTBoxTopTexture => tntboxtoptexture;
        public static Bitmap UnknownBoxTexture => unknownboxtexture;
        public static Bitmap UnknownBoxTopTexture => unknownboxtoptexture;
        public static Bitmap UnknownPickupTexture => unknownpickuptexture;
        public static Image BlueBuckle => bluebuckle;
        public static Image CodeBuckle => codebuckle;
        public static Image CrimsonBuckle => crimsonbuckle;
        public static Image GreyBuckle => greybuckle;
        public static Image LimeBuckle => limebuckle;
        public static Image VioletBuckle => violetbuckle;
        public static Image RedBuckle => redbuckle;
        public static Image YellowBuckle => yellowbuckle;
        public static Bitmap LimeTexture => fruitlime;
        public static Bitmap CoconutTexture => fruitcoconut;
        public static Bitmap StrawberryTexture => fruitstrawberry;
        public static Bitmap MangoTexture => fruitmango;
        public static Bitmap LemonTexture => fruitlemon;
        public static Bitmap YYYTexture => fruityyy;
        public static Bitmap GrapeTexture => fruitgrape;
        public static Bitmap PineappleTexture => fruitpineapple;
        public static Bitmap CortexTexture => fruitcortex;
        public static Bitmap BrioTexture => fruitbrio;
        public static Bitmap TawnaTexture => fruittawna;

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
            public ExternalTextureAttribute(int x, int y, int w = 1, int h = 1)
            {
                X = x;
                Y = y;
                W = w;
                H = h;
            }

            public int X { get; }
            public int Y { get; }
            public int W { get; }
            public int H { get; }
        }
    }
}
