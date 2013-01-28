using System;
using System.Reflection;

namespace Crash
{
    public static class Registrar
    {
        public static void RegisterAssembly(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            RegisterAssembly(Assembly.Load(name));
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            foreach (Type type in assembly.GetTypes())
            {
                RegisterType(type);
            }
        }

        public static void RegisterType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
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
