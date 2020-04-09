namespace Crash.GOOLIns
{
    [GOOLInstruction(40,GameVersion.Crash2)]
    [GOOLInstruction(40,GameVersion.Crash3)]
    public sealed class Eflr : GOOLInstruction
    {
        public Eflr(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"EFLR";
        public override string Format => DefaultFormat;
        public override string Comment => $"push entity field {GetArg('A')} row {GetArg('B')}";
    }
}
