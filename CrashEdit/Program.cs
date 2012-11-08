using Crash;
using System.Collections.Generic;

using IO = System.IO;
using Reflection = System.Reflection;

using Activator = System.Activator;
using Application = System.Windows.Forms.Application;

namespace CrashEdit
{
    internal static class Program
    {
        [System.STAThread]
        internal static void Main(string[] args)
        {
            RegisterAssembly("Crash");
            RegisterAssembly("Crash.Game");
            RegisterAssembly("Crash.Graphics");
            RegisterAssembly("Crash.Audio");
            RegisterAssembly("Crash.Unknown0");
            RegisterAssembly("Crash.Unknown4");
            RegisterAssembly("Crash.Unknown5");
            using (MainForm mainform = new MainForm())
            {
                foreach (string arg in args)
                {
                    mainform.OpenNSF(arg);
                }
                Application.Run(mainform);
            }
        }

        private static void RegisterAssembly(string name)
        {
            Reflection.Assembly assembly = Reflection.Assembly.Load(name);
            RegisterAssembly(assembly);
        }

        private static void RegisterAssembly(Reflection.Assembly assembly)
        {
            foreach (System.Type type in assembly.GetTypes())
            {
                RegisterType(type);
            }
        }

        private static void RegisterType(System.Type type)
        {
            foreach (ChunkTypeAttribute attribute in type.GetCustomAttributes(typeof(ChunkTypeAttribute),false))
            {
                ChunkLoader loader = (ChunkLoader)Activator.CreateInstance(type);
                Chunk.AddLoader(attribute.Type,loader);
            }
            foreach (EntryTypeAttribute attribute in type.GetCustomAttributes(typeof(EntryTypeAttribute),false))
            {
                EntryLoader loader = (EntryLoader)Activator.CreateInstance(type);
                Entry.AddLoader(attribute.Type,loader);
            }
        }
    }
}
