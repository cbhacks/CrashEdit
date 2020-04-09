namespace Crash.GOOLIns
{
    [GOOLInstruction(41,GameVersion.Crash2)]
    [GOOLInstruction(41,GameVersion.Crash3)]
    public sealed class Eflv : GOOLInstruction
    {
        public Eflv(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => $"EFLV";
        public override string Format => DefaultFormat;
        public override string Comment => $"{GetArg('A')} = entity field {GetArg('B')}";
    }
}
