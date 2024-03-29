﻿namespace CrashEdit.Crash
{
    public enum GOOLArgumentTypes
    {
        Value,
        Ref,
        ProcessField,
        DestRef,
        None,
        Signed
    }

    public readonly struct GOOLArgument
    {
        public GOOLArgument(int value, GOOLArgumentTypes type = GOOLArgumentTypes.Value)
        {
            Value = value;
            Type = type;
        }

        public int Value { get; }
        public GOOLArgumentTypes Type { get; }
    }
}
