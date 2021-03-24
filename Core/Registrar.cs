#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrashEdit {

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AssemblyProcessorAttribute : Attribute {}

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TypeProcessorAttribute : Attribute {}

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FunctionProcessorAttribute : Attribute {}

    public static class Registrar {

        private static HashSet<Assembly> RegisteredAssemblies { get; } =
            new HashSet<Assembly>();

        private static HashSet<Type> RegisteredTypes { get; } =
            new HashSet<Type>();

        private static HashSet<MethodInfo> RegisteredFunctions { get; } =
            new HashSet<MethodInfo>();

        private delegate void AssemblyProcessor(Assembly assembly);

        private delegate void TypeProcessor(Type type);

        private delegate void FunctionProcessor(MethodInfo methodInfo);

        private static List<AssemblyProcessor> AssemblyProcessors { get; } =
            new List<AssemblyProcessor>();

        private static List<TypeProcessor> TypeProcessors { get; } =
            new List<TypeProcessor>();

        private static List<FunctionProcessor> FunctionProcessors { get; } =
            new List<FunctionProcessor>();

        public static void Init() {
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        public static void RegisterAssembly(Assembly assembly) {
            if (!RegisteredAssemblies.Add(assembly))
                return;

            foreach (var proc in AssemblyProcessors) {
                proc(assembly);
            }

            foreach (var type in assembly.GetTypes()) {
                RegisterType(type);
            }
        }

        private static void RegisterType(Type type) {
            if (!RegisteredTypes.Add(type))
                return;

            foreach (var proc in TypeProcessors) {
                proc(type);
            }

            foreach (var methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)) {
                RegisterFunction(methodInfo);
            }
        }

        private static void RegisterFunction(MethodInfo methodInfo) {
            if (!RegisteredFunctions.Add(methodInfo))
                return;

            foreach (var proc in FunctionProcessors) {
                proc(methodInfo);
            }

            if (methodInfo.IsDefined(typeof(AssemblyProcessorAttribute), false)) {
                var proc = (AssemblyProcessor)methodInfo.CreateDelegate(typeof(AssemblyProcessor));
                AssemblyProcessors.Add(proc);
                foreach (var assembly in RegisteredAssemblies) {
                    proc(assembly);
                }
            }

            if (methodInfo.IsDefined(typeof(TypeProcessorAttribute), false)) {
                var proc = (TypeProcessor)methodInfo.CreateDelegate(typeof(TypeProcessor));
                TypeProcessors.Add(proc);
                foreach (var type in RegisteredTypes) {
                    proc(type);
                }
            }

            if (methodInfo.IsDefined(typeof(FunctionProcessorAttribute), false)) {
                var proc = (FunctionProcessor)methodInfo.CreateDelegate(typeof(FunctionProcessor));
                FunctionProcessors.Add(proc);
                foreach (var function in RegisteredFunctions) {
                    proc(function);
                }
            }
        }
    }

}
