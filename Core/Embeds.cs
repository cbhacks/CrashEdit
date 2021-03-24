#nullable enable

using System;
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
                        ImageList.Images.Add(
                            name.Split('.')[1],
                            Image.FromStream(stream));
                    }
                }
            }
        }

        public static ImageList ImageList { get; } =
            new ImageList();

    }

}
