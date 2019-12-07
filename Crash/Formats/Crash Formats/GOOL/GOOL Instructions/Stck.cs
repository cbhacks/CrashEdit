namespace Crash.GOOLIns
{
    [GOOLInstruction(71,GameVersion.Crash2)]
    [GOOLInstruction(71,GameVersion.Crash3)]
    public sealed class Stck : GOOLInstruction
    {
        public Stck(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"STCK";
        public override string Format => DefaultFormat;
        public override string Comment => string.Empty;
    }
}
