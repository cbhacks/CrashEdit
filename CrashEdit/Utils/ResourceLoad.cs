using System.IO;
using System.Reflection;

namespace CrashEdit
{
    public static class ResourceLoad
    {
        public static string[] GetAllFileNames() => Assembly.GetExecutingAssembly().GetManifestResourceNames();

        public static string LoadTextFile(string name)
        {
            var names = GetAllFileNames();
            var exe = Assembly.GetExecutingAssembly();
            var fullname = string.Format("{0}.{1}", exe.GetName().Name, name.Replace("/", "."));
            using var r = new StreamReader(exe.GetManifestResourceStream(fullname));
            return r.ReadToEnd();
        }
    }
}
