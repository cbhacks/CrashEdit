namespace Crash.GOOLIns
{
    [GOOLInstruction(41,GameVersion.Crash2)]
    [GOOLInstruction(41,GameVersion.Crash3)]
    public sealed class Efls : GOOLInstruction
    {
        public Efls(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"EFLS";
        public override string Format => DefaultFormat;
        public override string Comment => string.Empty;
    }
}
