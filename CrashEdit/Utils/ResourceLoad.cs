using System.Reflection;

namespace CrashEdit.CE
{
    public static class ResourceLoad
    {
        public static string[] GetAllFileNames() => Assembly.GetExecutingAssembly().GetManifestResourceNames();

        public static string LoadTextFile(string name)
        {
            var names = GetAllFileNames();
            var exe = Assembly.GetExecutingAssembly();
            var fullname = string.Format("{0}.{1}", "CrashEdit.CE", name.Replace("/", "."));
            using var r = new StreamReader(exe.GetManifestResourceStream(fullname));
            return r.ReadToEnd();
        }
    }
}
