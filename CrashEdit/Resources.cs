using System;
using System.IO;
using System.Drawing;
using System.Resources;
using System.Reflection;

namespace CrashEdit.CE
{
    internal static class Resources
    {
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

        static Resources()
        {
            ResourceManager manager = new ResourceManager("CrashEdit.Resources",Assembly.GetExecutingAssembly());
            foreach (FieldInfo field in typeof(Resources).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
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
                    foreach (FieldInfo field in typeof(Resources).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
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

        public static Bitmap ActivatorBoxTexture
        {
            get { return activatorboxtexture; }
        }

        public static Bitmap AppleTexture
        {
            get { return appletexture; }
        }

        public static Bitmap AppleBoxTexture
        {
            get { return appleboxtexture; }
        }

        public static Bitmap ArrowBoxTexture
        {
            get { return arrowboxtexture; }
        }

        public static Bitmap BodyslamBoxTexture
        {
            get { return bodyslamboxtexture; }
        }

        public static Bitmap BoxTexture
        {
            get { return boxtexture; }
        }

        public static Bitmap CheckpointTexture
        {
            get { return checkpointtexture; }
        }

        public static Bitmap DetonatorBoxTexture
        {
            get { return detonatorboxtexture; }
        }

        public static Bitmap DetonatorBoxTopTexture
        {
            get { return detonatorboxtoptexture; }
        }

        public static Bitmap IronArrowBoxTexture
        {
            get { return ironarrowboxtexture; }
        }

        public static Bitmap IronBoxTexture
        {
            get { return ironboxtexture; }
        }

        public static Bitmap LifeTexture
        {
            get { return lifetexture; }
        }

        public static Bitmap LifeBoxTexture
        {
            get { return lifeboxtexture; }
        }

        public static Bitmap MaskTexture
        {
            get { return masktexture; }
        }

        public static Bitmap MaskBoxTexture
        {
            get { return maskboxtexture; }
        }

        public static Bitmap NitroTexture
        {
            get { return nitrotexture; }
        }

        public static Bitmap NitroTopTexture
        {
            get { return nitrotoptexture; }
        }

        public static Bitmap PointTexture
        {
            get { return pointtexture; }
        }

        public static Bitmap QuestionMarkBoxTexture
        {
            get { return questionmarkboxtexture; }
        }

        public static Bitmap TNTTexture
        {
            get { return tnttexture; }
        }

        public static Bitmap TNTTopTexture
        {
            get { return tnttoptexture; }
        }

        public static Bitmap UnknownBoxTexture
        {
            get { return unknownboxtexture; }
        }

        public static Bitmap UnknownBoxTopTexture
        {
            get { return unknownboxtoptexture; }
        }

        public static Bitmap UnknownPickupTexture
        {
            get { return unknownpickuptexture; }
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class ResourceAttribute : Attribute
        {
            private string name;

            public ResourceAttribute(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get { return name; }
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class ExternalTextureAttribute : Attribute
        {
            private int x;
            private int y;

            public ExternalTextureAttribute(int x,int y)
            {
                this.x = x;
                this.y = y;
            }

            public int X
            {
                get { return x; }
            }

            public int Y
            {
                get { return y; }
            }
        }
    }
}
