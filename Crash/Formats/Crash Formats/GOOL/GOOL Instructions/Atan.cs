namespace Crash.GOOLIns
{
    [GOOLInstruction(45,GameVersion.Crash2)]
    [GOOLInstruction(45,GameVersion.Crash3)]
    public sealed class Atan : GOOLInstruction
    {
        public Atan(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ATAN";
        public override string Format => DefaultFormat;
        public override string Comment => $"atan2({GetArg('A')}, {GetArg('B')})";
    }
}
