#nullable enable

using System;
using System.Runtime.CompilerServices;

namespace CrashEdit {

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SubresourceSlotAttribute : SubresourceAttribute {

        public SubresourceSlotAttribute([CallerLineNumber] int order = 0) {
            Order = order;
        }

        public int Order { get; }

    }

}
