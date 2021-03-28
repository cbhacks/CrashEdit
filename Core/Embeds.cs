#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit {

    public static class Embeds {

        public const string Prefix = "CrashEdit.Main.Embeds.";

        static Embeds() {
            var asm = typeof(Embeds).Assembly;
            foreach (var fullName in asm.GetManifestResourceNames()) {
                if (!fullName.StartsWith(Prefix))
                    continue;

                var name = fullName.Substring(Prefix.Length);
                if (name.StartsWith("Images.")) {
                    using (var stream = asm.GetManifestResourceStream(fullName)) {
                        var bmpName = name.Split('.')[1];
                        var bitmap = new Bitmap(stream);
                        Bitmaps.Add(bmpName, bitmap);
                        ImageList.Images.Add(bmpName, bitmap);
                    }
                }
            }
        }

        public static Dictionary<string, Bitmap> Bitmaps =
            new Dictionary<string, Bitmap>();

        public static Dictionary<string, Icon> Icons =
            new Dictionary<string, Icon>();

        public static ImageList ImageList { get; } =
            new ImageList();

        public static Icon? GetIcon(string imageKey) {
            if (imageKey == null)
                throw new ArgumentNullException();

            if (Icons.TryGetValue(imageKey, out var icon)) {
                return icon;
            }

            if (!Bitmaps.TryGetValue(imageKey, out var bitmap)) {
                return null;
            }

            icon = Icon.FromHandle(bitmap.GetHicon());
            Icons.Add(imageKey, icon);
            return icon;
        }

    }

}
