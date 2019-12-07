namespace Crash.GOOLIns
{
    [GOOLInstruction(40,GameVersion.Crash2)]
    [GOOLInstruction(40,GameVersion.Crash3)]
    public sealed class Efld : GOOLInstruction
    {
        public Efld(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"EFLD";
        public override string Format => DefaultFormat;
        public override string Comment => string.Empty;
    }
}
