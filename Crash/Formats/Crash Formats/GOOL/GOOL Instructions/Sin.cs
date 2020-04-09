namespace Crash.GOOLIns
{
    [GOOLInstruction(43,GameVersion.Crash2)]
    [GOOLInstruction(43,GameVersion.Crash3)]
    public sealed class Sin : GOOLInstruction
    {
        public Sin(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SIN";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = sin({GetArg('S')})";
    }
}
