namespace Crash.GOOLIns
{
    [GOOLInstruction(2,GameVersion.Crash1)]
    [GOOLInstruction(2,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(2,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(2,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(2,GameVersion.Crash2)]
    [GOOLInstruction(2,GameVersion.Crash3)]
    public sealed class Mul : GOOLInstruction
    {
        public Mul(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MUL";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} * {GetArg('R')}";
    }
}
