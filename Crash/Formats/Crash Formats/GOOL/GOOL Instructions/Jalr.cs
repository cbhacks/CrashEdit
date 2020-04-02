namespace Crash.GOOLIns
{
    [GOOLInstruction(71,GameVersion.Crash2)]
    [GOOLInstruction(71,GameVersion.Crash3)]
    public sealed class Jalr : GOOLInstruction
    {
        public Jalr(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"JALR";
        public override string Format => DefaultFormat;
        public override string Comment => $"jump and link to {GetArg('A')} with {GetArg('B')} argument(s)";
    }
}
