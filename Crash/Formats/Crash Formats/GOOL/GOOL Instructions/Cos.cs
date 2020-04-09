namespace Crash.GOOLIns
{
    [GOOLInstruction(44,GameVersion.Crash2)]
    [GOOLInstruction(44,GameVersion.Crash3)]
    public sealed class Cos : GOOLInstruction
    {
        public Cos(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "COS";
        public override string Format => DefaultFormatDS2;
        public override string Comment => $"{GetArg('D')} = cos({GetArg('S')})";
    }
}
