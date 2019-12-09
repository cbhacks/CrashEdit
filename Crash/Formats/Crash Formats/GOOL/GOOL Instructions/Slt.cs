namespace Crash.GOOLIns
{
    [GOOLInstruction(9,GameVersion.Crash1)]
    [GOOLInstruction(9,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(9,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(9,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(9,GameVersion.Crash2)]
    [GOOLInstruction(9,GameVersion.Crash3)]
    public sealed class Slt : GOOLInstruction
    {
        public Slt(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SLT";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} < {GetArg('R')}";
    }
}
