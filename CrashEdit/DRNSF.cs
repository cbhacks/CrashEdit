using System.IO;
using System.Diagnostics;

namespace CrashEdit
{
    public static class DRNSF
    {
        private static string FindEXEInDir(DirectoryInfo dir)
        {
            // Search for "drnsf.exe", "drnsf.cmd", "drnsf", etc.
            foreach (var file in dir.GetFiles()) {
                if (Path.GetFileNameWithoutExtension(file.Name).ToLower() == "drnsf") {
                    return file.FullName;
                }
            }

            // Otherwise, recursively check each subdirectory named "drnsf".
            foreach (var subdir in dir.GetDirectories()) {
                if (subdir.Name.ToLower() != "drnsf")
                    continue;

                var result = FindEXEInDir(subdir);
                if (result != null) {
                    return result;
                }
            }

            // Otherwise, not found.
            return null;
        }

        public static string FindEXE()
        {
            var cePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            var ceDir = Path.GetDirectoryName(cePath);
            var dir = new DirectoryInfo(ceDir);

            var result = FindEXEInDir(dir);
            if (result != null)
                return result;

            result = FindEXEInDir(dir.Parent);
            if (result != null)
                return result;

            throw new FileNotFoundException();
        }

        public static int Invoke(string args)
        {
            var psi = new ProcessStartInfo(FindEXE())
            {
                Arguments = args,
                UseShellExecute = false
            };
            using (var drnsf = Process.Start(psi)) {
                drnsf.WaitForExit();
                return drnsf.ExitCode;
            }
        }
    }
}
