namespace Crash.GOOLIns
{
    [GOOLInstruction(43,GameVersion.Crash2)]
    public sealed class Sin : GOOLInstruction
    {
        public Sin(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SIN";
        public override string Format => DefaultFormatDS;
        public override string Comment => $"{GetArg('D')} = sin({GetArg('S')})";
    }
    
    [GOOLInstruction(43,GameVersion.Crash3)]
    public sealed class Sin2 : GOOLInstruction
    {
        public Sin2(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SIN";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = sin({GetArg('S')})";
    }
}
